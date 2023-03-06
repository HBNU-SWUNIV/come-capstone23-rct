import zmq
import keyboard
import time

context = zmq.Context()

# Create a publisher socket
publisher = context.socket(zmq.PUB)
publisher.bind("tcp://*:5555")

# def on_key_event(event):
#     # 이벤트가 발생하면 호출되는 콜백 함수
#     message = f"Key {event.name} was {event.event_type}"
#     print(message)
    # publisher.send_string(message)

def on_press_callback(event):
    print(f"Key {event.name} was pressed")
    publisher.send_string(event.name)
    # time.sleep(0.1)

# def on_release_callback(event):
#     time.sleep(0.1)
#     print(f"Key {event.name} was released")

keyboard.on_press(on_press_callback)
# keyboard.on_release(on_release_callback)
keyboard.wait()


