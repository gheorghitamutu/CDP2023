# CDP2023
CDP Homeworks

## Homework 01
The homework is explained in [Requirements file](repo/blob/master/Homework01/docs/PCD_Homework1.pdf)

The project is written in Python. It is a simple client-server application where the client uploads files to a server via TCP (stop-and-wait) or UDP (streaming) protocols.

### Project Structure
File          | Description
------------- | -------------
config.cfg    | Simple config with settings for client, server & common (alternative to the command line arguments)
structures.py | Definition of packages sent via TCP or UDP
client.py     | The client that uploads files once it connects to the server
server.py     | The server that receives the files from the client and writes them to disk

### Structures
Structure     | Description
------------- | -------------
Protocol      | TCP or UDP
TCPState      | Corrupted or Good (package)
PackageType   | Header (file size, number of packages, etc) or Block (actual indexed data)
HeaderTCP     | Filename, file size and number of packages which will be sent
HeaderUDP     | Depending on the type and state => initial header (metadata), actual block or 'Done' (end) marker

### Client
Flow          | Description
------------- | -------------
TCP           | send_data_via_tcp(source_path) => connection & source path folder iteration => for every file split it in packages and send them with confirmation
UDP           | send_data_via_udp(source_path) => connection & source path folder iteration => for every file split it in packages and send them (no confirmation)

### Server
Flow          | Description
------------- | -------------
TCP           | receive_data_via_tcp(destination_path) => connection => for each file received (identification via initial header) write all its packets (iterative, sorted, with confirmation for every one of them)
UDP           | receive_data_via_udp(destination_path) => connection => create a map with all the files and packages received => write everything when the client sends `Done` marker (keeping in mind that packages need to be reordered, some may be missing or maybe the initial header (containing filename, packages count, etc) is missing)
