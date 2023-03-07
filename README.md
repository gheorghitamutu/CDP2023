# CDP2023
Concurrent and Distributed Programming Homeworks (2023)

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

### Command line arguments
Script        | Description
------------- | -------------
client.py     | python3 <protocol> <source folder> <ip> <port> 
client.py     | python3 UDP ./test/source 127.0.0.1 7003 
server.py     | python3 <protocol> <destination folder> <ip> <port> 
server.py     | python3 UDP ./test/destination 127.0.0.1 7003 
  
### Results
Entity        | Description | Messages | Bytes | Time 
------------- | ----------- | -------- | ----- | ---- 
Message       | Batches of 100 bytes (+ UDP header to sort blocks)
Files         | 8798 files with a total size of 381MB
TCP (Client)  | Via TCP protocol | Messages sent: 4020687 | Bytes received: 400869938 | Time to receive/process: 384.31203961372375 seconds
TCP (Server)  | Via TCP protocol | Messages received: 4020687 | Bytes received: 400869938 | Time to receive/process: 387.39903807640076 seconds
UDP (Client)  | Via UDP protocol | Messages sent: 4011889 | Bytes sent: 551118578 | Time to send: 186.1149137020111 seconds | 
UDP (Sever)   | Via UDP protocol | Messages received: 750097 | Bytes received: 102253457 | Time to receive/process: 215.37958908081055 seconds |

### Conclusions
Lost packages via UDP: 81.303146722155077%.
  
Lost packages via TCP: 0%.

### TCP
(+) Correct order of packages with confirmation.
  
(+) Way more reliable and less coding overhead than UDP.
  
(-) Confirmation and authentication takes a lot of time.
  
(-) Actual time was 79% slower than UDP.
  
### UDP 
(+) Faster than TCP (79% faster).
  
(-) Lost packages percentage may vary. Not recommendable for files.
  
(-) Packages are unordered. Even if all of them are received, you need extra data for mapping and sorting (along with the overhead to implement these). If you write these packages to disk, the speed you are winning from using this protocol is lost on IO. If you choose not to, it requires a lot of RAM. Even if you somehow track and write the files to disk on completion (all packages received) you still remain with all the incomplete files (random packages from different files).

