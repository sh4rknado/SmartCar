import socket
import json

from Protocol.ClientProtocol import Sender

def test_post_request():
    host = "10.0.0.100"
    port = 8080
    
    
    json_data = {
        "Type": "Command",
        "Action": "SetDirection",
        "Data": {"Direction":30.0, },
    }
    
    data = json.dumps(json_data)

    # Create a socket (SOCK_STREAM means a TCP socket)
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        # Connect to server and send data
        sock.connect((host, port))
        sock.sendall(bytes(data,encoding="utf-8"))


        # Receive data from the server and shut down
        received = sock.recv(1024)
        received = received.decode("utf-8")
        
        print("Received:", received)

    finally:
        sock.close()
        
def test():
    sender = Sender()
    sender.connect()
    sender.turn()

        
if __name__ == "__main__":
    test()