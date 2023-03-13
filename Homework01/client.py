import socket
import os
import sys
import time
import configparser
from structures import Protocol, TCPState, PackageType, UDPState

config = configparser.ConfigParser()
config.read(['config.cfg'])
client = config['CLIENT']

HOST = client['HOST']
PORT = int(client['PORT'])
MAX_MESSAGE_SIZE_TCP = int(client['MAX_MESSAGE_SIZE_TCP'])
MAX_MESSAGE_SIZE_UDP = int(client['MAX_MESSAGE_SIZE_UDP'])
TIMEOUT_UDP = int(client['TIMEOUT_UDP'])

common = config['COMMON']
DELIMITER_UDP = common['DELIMITER_UDP']
DELIMITER_TCP = common['DELIMITER_TCP']
END_MARKER = common['END_MARKER']
CONFIRMATION_UDP = config.getboolean('COMMON', 'CONFIRMATION_UDP')
CONFIRMATION_TCP = config.getboolean('COMMON', 'CONFIRMATION_TCP')
PROTOCOL = common['PROTOCOL']
WRITE_FILES = config.getboolean('COMMON', 'WRITE_FILES')


def show_stats(protocol, number_of_messages, number_of_bytes, start_time, end_time):
    print(f'Protocol used: {protocol}')
    print(f'Number of messages sent: {number_of_messages}')
    print(f'Number of bytes sent: {number_of_bytes}')
    print(f'Time: {end_time - start_time} seconds')


def compute_packages_count(file_path, max_size):
    return os.stat(file_path).st_size // max_size + 1


def construct_header_tcp(filename, file_path, number_of_packages):
    return \
        (f'{filename}{DELIMITER_TCP}'
         f'{os.stat(file_path).st_size}{DELIMITER_TCP}'
         f'{number_of_packages}').encode()


def send_file_via_tcp(s, f, file_path, filename):
    bytes_sent = 0
    messages_send = 0

    number_of_packages = compute_packages_count(file_path, MAX_MESSAGE_SIZE_TCP)
    header = construct_header_tcp(filename, file_path, number_of_packages)

    while True:
        s.send(header)
        bytes_sent += len(header)
        messages_send += 1

        if CONFIRMATION_TCP:
            data = s.recv(1)
            state = TCPState(data[0])

            if state == TCPState.Good:
                break
            elif state == TCPState.Corrupted:  # resend
                continue
        else:
            break

    # print(f'Sent file {filename} header with #{number_of_packages} packages.')

    for i in range(0, number_of_packages):

        package_content = f.read(MAX_MESSAGE_SIZE_TCP)
        bytes_sent += len(package_content)
        messages_send += 1
        s.send(package_content)

        # print(f'Sent file {filename} package {i + 1}/{number_of_packages}.')

        if CONFIRMATION_TCP:
            while True:
                data = s.recv(1)
                state = TCPState(data[0])

                if state == TCPState.Good:
                    break

                elif state == TCPState.Corrupted:  # resend
                    bytes_sent += len(package_content)
                    messages_send += 1
                    s.send(package_content)

    return bytes_sent, messages_send


def send_data_via_tcp(source_path):
    total_bytes_sent = 0
    total_messages_send = 0

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((HOST, PORT))

        for filename in os.listdir(source_path):

            file_path = os.path.join(source_path, filename)
            with open(file_path, 'rb') as f:
                bytes_sent, messages_send = send_file_via_tcp(s, f, file_path, filename)
                total_bytes_sent += bytes_sent
                total_messages_send += messages_send

        s.send(END_MARKER.encode())
        total_bytes_sent += len(END_MARKER)
        total_messages_send += 1

        # print(f'Sent end marker.')

    return total_bytes_sent, total_messages_send


def construct_header_udp(file_index, filename, file_path, number_of_packages):
    return \
        (f'{PackageType.Header}{DELIMITER_UDP}'
         f'{file_index}{DELIMITER_UDP}'
         f'{filename}{DELIMITER_UDP}'
         f'{os.stat(file_path).st_size}{DELIMITER_UDP}'
         f'{number_of_packages}').encode()


def construct_block_udp(file_index, i_package, f):
    header = \
        (f'{PackageType.Block}{DELIMITER_UDP}'
         f'{file_index}{DELIMITER_UDP}'
         f'{i_package}{DELIMITER_UDP}').encode()
    # delta_max_len = MAX_MESSAGE_SIZE_UDP - len(header)
    # block = f.read(delta_max_len)
    block = f.read(MAX_MESSAGE_SIZE_UDP)
    data = header + block
    return data


def send_package_via_udp_with_confirmation(s, data, address):
    bytes_sent = 0
    messages_send = 0

    while True:
        bytes_sent += len(data)
        messages_send += 1
        s.sendto(data, address)
        # print(f"Sent {data} to {address}!")

        s.settimeout(TIMEOUT_UDP)  # now we wait for confirmation
        try:
            confirmation_data, addr = s.recvfrom(1)
            state = UDPState(confirmation_data[0])
            if state == UDPState.Received:
                break
            else:
                pass
        except socket.timeout as _:
            pass

    return bytes_sent, messages_send


def send_package_via_udp_without_confirmation(s, data, address):
    bytes_sent = len(data)
    messages_send = 1

    s.sendto(data, address)

    return bytes_sent, messages_send


def send_file_via_udp(s, f, file_path, file_index, filename, address):
    bytes_sent = 0
    messages_send = 0

    number_of_packages = compute_packages_count(file_path, MAX_MESSAGE_SIZE_UDP)
    data = construct_header_udp(file_index, filename, file_path, number_of_packages)

    if CONFIRMATION_UDP:
        a, b = send_package_via_udp_with_confirmation(s, data, address)
    else:
        a, b = send_package_via_udp_without_confirmation(s, data, address)
    bytes_sent += a
    messages_send += b

    for i in range(0, number_of_packages):
        data = construct_block_udp(file_index, i, f)

        if CONFIRMATION_UDP:
            a, b = send_package_via_udp_with_confirmation(s, data, address)
        else:
            a, b = send_package_via_udp_without_confirmation(s, data, address)
        bytes_sent += a
        messages_send += b

    # print(f'File {filename} was sent to server!')

    return bytes_sent, messages_send


def send_data_via_udp(source_path):
    total_bytes_sent = 0
    total_messages_send = 0
    address = (HOST, PORT)

    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:

        for (i, filename) in enumerate(os.listdir(source_path)):

            file_path = os.path.join(source_path, filename)
            with open(file_path, 'rb') as f:
                bytes_sent, messages_send = send_file_via_udp(s, f, file_path, i, filename, address)
                total_bytes_sent += bytes_sent
                total_messages_send += messages_send

        s.sendto(END_MARKER.encode(), address)
        total_bytes_sent += len(END_MARKER)
        total_messages_send += 1

    return total_bytes_sent, total_messages_send


def send_data(protocol, path_sent):
    total_bytes_sent = 0
    total_messages_send = 0

    start = time.time()
    if protocol == Protocol.TCP:
        total_bytes_sent, total_messages_send = send_data_via_tcp(path_sent)
    elif protocol == Protocol.UDP:
        total_bytes_sent, total_messages_send = send_data_via_udp(path_sent)
    end = time.time()

    show_stats(protocol, total_messages_send, total_bytes_sent, start, end)


def show_help():
    print('client.py <protocol> <source folder>')
    print('Example: client.py /mnt/z/source_folder')


if __name__ == "__main__":
    if len(sys.argv) < 2:
        show_help()
        exit(0)

    source_path = sys.argv[1]
    if PROTOCOL == "TCP":
        send_data(Protocol.TCP, source_path)
    elif PROTOCOL == "UDP":
        send_data(Protocol.UDP, source_path)
    else:
        show_help()
