import socket
import os
import sys
import time
import configparser
from structures import Protocol, TCPState, PackageType, HeaderTCP, HeaderUDP, UDPState

config = configparser.ConfigParser()
config.read(['config.cfg'])

common = config['COMMON']
DELIMITER_UDP = common['DELIMITER_UDP']
DELIMITER_TCP = common['DELIMITER_TCP']
END_MARKER = common['END_MARKER']
CONFIRMATION_UDP = bool(common['CONFIRMATION_UDP'])

server = config['SERVER']
HOST = server['HOST']
PORT = int(server['PORT'])
MAX_MESSAGE_SIZE_TCP = int(server['MAX_MESSAGE_SIZE_TCP'])
MAX_MESSAGE_SIZE_UDP = \
    int(server['MAX_MESSAGE_SIZE_UDP']) + len(f'{PackageType.Block}') + 3 * len(DELIMITER_UDP) + 2 * 4
TIMEOUT_UDP = int(server['TIMEOUT_UDP'])


def show_stats(protocol, number_of_messages, number_of_bytes, start_time, end_time):
    print(f'Protocol used: {protocol}')
    print(f'Number of messages received: {number_of_messages}')
    print(f'Number of bytes received: {number_of_bytes}')
    print(f'Time: {end_time - start_time} seconds')


def receive_data_via_tcp(destination_path):
    total_messages_received = 0
    total_bytes_received = 0

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen()
        conn, addr = s.accept()
        with conn:
            print('New connection from', addr)

            while True:
                data = conn.recv(1024)  # enough for the initial header with metadata
                total_messages_received += 1
                total_bytes_received += len(data)
                if data == END_MARKER.encode():
                    # print(f'Received end marker.')
                    break

                header = HeaderTCP(data, DELIMITER_TCP)
                conn.send(int(TCPState.Good).to_bytes(length=1, byteorder='little', signed=False))
                total_bytes_received += len(data)
                total_messages_received += 1

                # print(f'Received file {header.filename} header with #{header.number_of_packages} packages.')

                file_path = os.path.join(destination_path, header.filename)
                with open(file_path, 'wb') as file:

                    package_index = 0
                    while package_index < header.number_of_packages:

                        data = conn.recv(MAX_MESSAGE_SIZE_TCP)
                        total_bytes_received += len(data)
                        total_messages_received += 1

                        if not data:
                            conn.send(int(TCPState.Corrupted).to_bytes(length=1, byteorder='little', signed=False))
                        else:
                            file.write(data)
                            # print(f'Received file {header.filename} package {package_index + 1}/{header.number_of_packages}.')
                            conn.send(int(TCPState.Good).to_bytes(length=1, byteorder='little', signed=False))
                            package_index += 1

    return total_bytes_received, total_messages_received


def get_file_index(file_index, files):
    for i, (header, blocks) in enumerate(files):
        if header.file_index == file_index:
            return i
    return -1


def blocks_comparator(block):
    return block.package_index


def recompose_files(files, files_ids, packages_with_no_header, headless_files, files_without_header_id, destination_path):

    for package in packages_with_no_header:  # try recomposing missing packages from specific files
        if package.file_index in files_ids:
            index = get_file_index(package.file_index, files)
            files[index][1].append(package)
        elif package.file_index in files_without_header_id:
            index = get_file_index(package.file_index, headless_files)
            headless_files[index][1].append(package)
        else:
            p2 = package
            p2.filename = f'HeaderMissing-#{package.file_index}'
            p2.package_type = PackageType.Header
            headless_files.append([p2, [package]])
            files_without_header_id.append(package.file_index)

    files_no = len(files)
    files_corrupted = 0
    for header, packages in files:
        if header.number_of_packages != len(packages):
            header.filename = f'CorruptedFile-{header.filename}'
            files_corrupted += 1

        packages.sort(key=blocks_comparator)
        file_path = os.path.join(destination_path, header.filename)
        with open(file_path, 'wb') as f:
            for package in packages:
                f.write(package.block)
                # print(f'Written package #{package.package_index} of length {len(package.block)}!')

    for header, packages in headless_files:
        packages.sort(key=blocks_comparator)
        file_path = os.path.join(destination_path, header.filename)
        with open(file_path, 'wb') as f:
            for package in packages:
                f.write(package.block)

    print(f'Good files: {files_no - files_corrupted}')
    print(f'Corrupted files (missing packages): {files_corrupted}')
    print(f'Corrupted files (missing header): {len(headless_files)}')


def receive_data_via_udp(destination_path):
    total_messages_received = 0
    total_bytes_received = 0

    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:
        s.bind((HOST, PORT))
        print("UDP server up and listening")
        files = list()
        files_ids = list()
        packages_with_no_header = list()
        headless_files = list()
        files_without_header_id = list()

        while True:
            if CONFIRMATION_UDP:
                while True:
                    try:
                        s.settimeout(TIMEOUT_UDP)
                        data, address = s.recvfrom(MAX_MESSAGE_SIZE_UDP)
                        break
                    except socket.timeout as _:
                        pass
            else:
                data, _ = s.recvfrom(MAX_MESSAGE_SIZE_UDP)

            total_bytes_received += len(data)
            total_messages_received += 1

            if len(data) > 1:  # we know confirmation has been received

                header = HeaderUDP(data, DELIMITER_UDP, END_MARKER)

                if header.done:
                    recompose_files(
                        files,
                        files_ids,
                        packages_with_no_header,
                        headless_files,
                        files_without_header_id,
                        destination_path)
                    break

                if header.package_type == PackageType.Header:
                    files.append((header, []))
                    files_ids.append(header.file_index)

                elif header.package_type == PackageType.Block:
                    if header.file_index in files_ids:
                        index = get_file_index(header.file_index, files)
                        files[index][1].append(header)
                    else:
                        packages_with_no_header.append(header)

                if CONFIRMATION_UDP:
                    s.sendto(int(UDPState.Received).to_bytes(length=1, byteorder='little', signed=False), address)
                    # print(f"Sent {int(UDPState.Received).to_bytes(length=1, byteorder='little', signed=False)} to {address}!")

            else:  # resend the confirmation message (the previous one may have been lost)
                if CONFIRMATION_UDP:
                    s.sendto(int(UDPState.Received).to_bytes(length=1, byteorder='little', signed=False), address)
                    # print(f"Sent {int(UDPState.Received).to_bytes(length=1, byteorder='little', signed=False)} to {address}!")

    return total_bytes_received, total_messages_received


def receive_data(protocol, source_path):
    start = time.time()
    total_messages_send = 0
    total_bytes_sent = 0

    if protocol == Protocol.TCP:
        total_bytes_sent, total_messages_send = receive_data_via_tcp(source_path)
    elif protocol == Protocol.UDP:
        total_bytes_sent, total_messages_send = receive_data_via_udp(source_path)

    show_stats(protocol, total_messages_send, total_bytes_sent, start, time.time())


def show_help():
    print("server.py <protocol> <destination path> <ip server> <port server> <option>")
    print("Example: server.py TCP /mnt/z/test/received 127.0.0.1 7001")


if __name__ == "__main__":
    if len(sys.argv) < 3:
        show_help()
    elif sys.argv[1] == "TCP":
        receive_data(Protocol.TCP, sys.argv[2])
    elif sys.argv[1] == "UDP":
        receive_data(Protocol.UDP, sys.argv[2])
    else:
        show_help()
