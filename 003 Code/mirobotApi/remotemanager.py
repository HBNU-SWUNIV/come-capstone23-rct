import json
import zmq
# import zmq.asyncio
# import asyncio
from mirobotmanager import MirobotManager
from config import *


class Remotemanager(MirobotManager):
    def __init__(self, arm) -> None:
        super().__init__(arm)
        print("ZMQ recieve Initialized")

        ### ZMQ Subscribe
        # asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())
        # self.context = zmq.asyncio.Context()
        self.context = zmq.Context()
        self.socket = self.context.socket(zmq.SUB)
        self.socket.setsockopt_string(zmq.SUBSCRIBE, "")
        self.socket.connect(f"tcp://{HOST}:{PORT}")
        print(f"zmq subscribed at {HOST}:{PORT}....")        
        
        ### ZMQ Response
        # self.context = zmq.Context()
        # self.socket = self.context.socket(zmq.REP)
        # self.socket.bind(f"tcp://{HOST}:{PORT}")
        # print(f"Server listening on port {PORT}...")

    def remoteRecv(self):
        """
        ZMQ Communication
        """
        while True:
            # self.message = await self.socket.recv_string()
            self.message = self.socket.recv_string()
            print(f"Received message: {self.message}")
            
            self._AxisControl(self.message)

            # else:
            # if self.maxAngle(self.message):
            #     print("Robot Angle Out of Range")
                # self.socket.send_string("Robot Angle Out of Range!")  
                
            # else:
                # self.response = json.dumps(self.getCurrentAngle())
                # self.socket.send_string(self.response)            
                # await asyncio.create_task(self.AxisControl(self.message))
                # self._AxisControl(self.message)
                # pass
            
            
            
    
    
