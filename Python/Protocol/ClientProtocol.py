import socket
import json

class Sender:
    def __init__(self, url="10.0.0.100", port="8080"):
        self.url = url
        self.port = port

    def connect(self):
        data = json.dumps(data)

        # Create a socket (SOCK_STREAM means a TCP socket)
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        
        # Connect to server and send data
        self.sock.connect((self.host, self.port))

    
    def send(self, data):
        try:
            # Convert data to JSON format
            json_data = json.dumps(data)
            # Send data
            self.sock.sendall(bytes(json_data, encoding="utf-8"))
            # Receive response
            received = self.sock.recv(1024)
            return received.decode("utf-8")
        except Exception as e:
            print("Error sending data:", e)
            return None
        
            
    def turn(self):
        data = {
            "Type": "Command",
            "Action": "SetDirection",
            "Data": {"Direction":30.0, },
        }
        response = self.send(data)
        if response:
            print("Response received:", response)
        else:
            print("Failed to send data.")
            
    def stop(self):
        print("Stopping sender...")