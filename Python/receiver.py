import socket
import json

class Catcher:
    def __init__(self, url="10.0.0.1", port="5000"):
        self.url = url
        self.port = port

    def connect(self):
        data = json.dumps(data)

        # Create a socket (SOCK_STREAM means a TCP socket)
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        
        # Connect to server and send data
        sock.connect((self.host, self.port))
        
            
    def get_frame(self):
        self.sock.sendall("GET / HTTP/1.1\r\n\r\n")
        response = self.sock.recv(1024)
        if response:
            print("Response received:", response)
        else:
            print("Failed to receive data.")
            
    def stop(self):
        print("Stopping sender...")