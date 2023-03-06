import zmq

context = zmq.Context()
socket = context.socket(zmq.SUB)
socket.setsockopt_string(zmq.SUBSCRIBE, "")
socket.connect("tcp://localhost:5555")

while True:
    message = socket.recv_string()
    print(message)