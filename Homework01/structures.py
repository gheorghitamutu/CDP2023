from enum import IntEnum


class Protocol(IntEnum):
    TCP = 0,
    UDP = 1


class TCPState(IntEnum):
    Corrupted = 0,
    Good = 1


class UDPState(IntEnum):
    Received = 0,
    Error = 1


class PackageType(IntEnum):
    Header = 0,
    Block = 1


class HeaderTCP:
    def __init__(self, data, delimiter):
        data = data.split(delimiter.encode())

        self.filename = data[0].decode()
        self.file_size = int(data[1])
        self.number_of_packages = int(data[2])


class HeaderUDP:
    def __init__(self, data, delimiter, end_marker):
        data = data.split(delimiter.encode())

        self.done = False
        if len(data) == 1 and data[0] == end_marker.encode():
            self.done = True

        if not self.done:
            self.package_type = PackageType(int(data[0]))
            self.file_index = int(data[1])

            if len(data) == 5:
                self.filename = data[2].decode()
                self.file_size = int(data[3])
                self.number_of_packages = int(data[4])
            else:
                self.package_index = int(data[2])
                self.block = data[3]
