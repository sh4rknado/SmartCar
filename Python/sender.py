import socket
import json

class Sender:
    def __init__(self, url="10.0.0.100", port="8080"):
        self.url = url
        self.port = port

    def connect(self):
        data = json.dumps(data)

        # Create a socket (SOCK_STREAM means a TCP socket)
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        
        # Connect to server and send data
        sock.connect((self.host, self.port))
        
            
    def forward(self):
        data = {
            "title": "foo",
            "body": "bar",
            "userId": 1
        }
        response = self.send(data)
        if response:
            print("Response received:", response)
        else:
            print("Failed to send data.")
            
    def stop(self):
        print("Stopping sender...")