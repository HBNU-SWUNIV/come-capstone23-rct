from aiortc import RTCIceCandidate, RTCPeerConnection, RTCSessionDescription, RTCConfiguration, RTCIceServer, VideoStreamTrack, RTCRtpSender
import json
import asyncio
import requests
import cv2
import numpy as np
import base64
from config import *
import os


ID = "offerer01"
# os.environ["OPENCV_FFMPEG_CAPTURE_OPTIONS"] = "dummy"


async def main():
    
    config = RTCConfiguration(iceServers=[
        RTCIceServer(urls=["stun:stun.l.google.com:19302"])
    ])

    print("Starting")
    peer_connection = RTCPeerConnection(configuration=config)

    channel = peer_connection.createDataChannel("video")
    
    
    async def send_video():
        """
        Opencv를 활용해 실시간으로 웹캠 불러와서 전송
        """
        # cap = cv2.VideoCapture("rtsp://192.168.50.119:8554/live?resolution=1920x960")
        cap = cv2.VideoCapture(0)
        
        while True:
            
            ret, frame = cap.read(1)
            
            #해상도 줄여서 데이터 크기 축소(화질떨어짐)
            frame = cv2.resize(frame,(1024, 720))
            
            if not ret :
                break
            
            # Encode the frame in base64
            
            _, buffer = cv2.imencode('.jpg', frame)
            
        
            img_str = base64.b64encode(buffer).decode('utf-8')
        
            
            channel.send(img_str)
                        
            await asyncio.sleep(0.1)

    @channel.on("open")
    def on_open():
        
        """
        Datachannel Open
        """
        print("channel opened")
        asyncio.ensure_future(send_video())
        

    await peer_connection.setLocalDescription(await peer_connection.createOffer())
    message = {"id": ID, "sdp" : peer_connection.localDescription.sdp, "type" : peer_connection.localDescription.type}
    r = requests.post(SIGNALING_SERVER_URL + '/offer', data = message)
    print(r.status_code)
    
    while True:
        resp = requests.get(SIGNALING_SERVER_URL + "/get_answer")
        if resp.status_code == 503:
            print("Answer not Ready , trying again")
            await asyncio.sleep(1)
        elif resp.status_code == 200:
            data = resp.json()
            if data["type"] == "Answer":
                data["type"] = "answer"
                rd = RTCSessionDescription(sdp = data["sdp"], type=data["type"])
                await peer_connection.setRemoteDescription(rd)
                print(peer_connection.remoteDescription)
                while True:
                    await asyncio.sleep(1)
            else:
                print("Wrong type")
            break
        print(resp.status_code)
    
asyncio.run(main())