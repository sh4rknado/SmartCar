import socket
import json
import requests
import io

class Receiver:
    def __init__(self, url="10.0.0.1", port=5000):
        self.host = url
        self.port = port
    
    def get_frame(self):
        r = requests.get(f"http://{self.host}:{self.port}",
                                        stream=True)
        #total_length = r.headers['Content-length']
        for chunk in r.iter_content(chunk_size=640):
            print(chunk)
        print(r.json)

class Sender:
    def __init__(self, url="10.0.0.1", port="8080"):
        self.host = url
        self.port = port

    def connect(self):
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