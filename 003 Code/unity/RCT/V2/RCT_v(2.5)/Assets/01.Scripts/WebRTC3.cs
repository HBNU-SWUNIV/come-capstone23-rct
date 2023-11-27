using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using System.Text;

public class WebRTC3 : MonoBehaviour
{

    [System.Serializable]
    private class SignalingMessage
    {
        public string id;
        public RTCSdpType type;
        public string sdp;

    }

    [System.Serializable]
    private class TostringMessage
    {
        public string id;
        public string type;
        public string sdp;
    }

    private string ID = "answerer01";
    private string SIGNALING_SERVER_URL = "http://192.168.50.85:19612/signaling";

    private string[] STUNSERVER = { "stun:stun.l.google.com:19302" };
    public UnityWebRequest webRequest;
    public UnityWebRequest postRequest;
    public RTCDataChannel channel;
    public RTCDataChannel receiveChannel;

    public RTCSessionDescription a_desc;

    private RTCPeerConnection peerConnection;
    private RTCConfiguration config;

    public Renderer cuberenderer;
    public GameManager GM;

    // Start is called before the first frame update
    void Start()
    {

        WebRTC.Initialize();
        config = new RTCConfiguration();
        RTCIceServer[] iceServers = { new RTCIceServer { urls = STUNSERVER } };
        config.iceServers = iceServers;

        peerConnection = new RTCPeerConnection(ref config);

        RTCDataChannelInit channelConfig = new RTCDataChannelInit();

        channel = peerConnection.CreateDataChannel("video",channelConfig);
        channel.OnOpen += () =>
        {
            Debug.Log("Data channel Open ==> 원격 Data 채널 생성 중");
        };
        channel.OnClose += () =>
        {
            Debug.Log("Data channel closed!");
        };

        channel.OnMessage += (data) =>
        {
            Debug.Log("Data channel received message: " + data);
        };

        cuberenderer.enabled = false;
        
        StartCoroutine(GetResquest(SIGNALING_SERVER_URL));

    }

    private void Update() 
    {
       
        peerConnection.OnDataChannel = (channel_1) =>
        {
            Debug.Log("원격 Data 채널 생성 완료.");

            // Assign the channel object to the dataChannel variable
            channel = channel_1;

            // Set up the data channel event handlers
            channel.OnOpen += () =>
            {
                Debug.Log("Data channel open --> remote 생성중.");
            };

            channel.OnClose += () =>
            {
                Debug.Log("Data channel closed!");
            };

            channel.OnMessage += (byte[] bytes) =>
            {
                
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                Debug.Log("카메라 String Data 수신 및 Encoding 중\n");
                
                // byte[] image_byte = Convert.FromBase64String(message);
                // // Decode the received data as an image
                Texture2D texture = new Texture2D(2, 2);
                Debug.Log("Inversphere에 변환 이미지 로드");
                texture.LoadImage(Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(bytes)));

                // Apply the texture to the sphere
                cuberenderer.material.mainTexture = texture;
                cuberenderer.enabled = true;
            };
        };

    }

  
    IEnumerator Createanswer()
    {
        var op = peerConnection.CreateAnswer();
        yield return op;
        peer_local_connection_access(op.Desc);
        
    }

    IEnumerator PostResquest(RTCSessionDescription desc)
    {
        var message = new TostringMessage();
        message.id = ID;
        message.type = desc.type.ToString();
        message.sdp = desc.sdp;
        
        string json = JsonUtility.ToJson(message);
        Debug.Log("Post json\n"+json);
        Debug.Log("id : "+message.id);
        Debug.Log("type : "+message.type);
        Debug.Log("sdp : "+message.sdp);
        using (UnityWebRequest www = UnityWebRequest.Post(SIGNALING_SERVER_URL + "/answer", json))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Offer SDP 수신 완료에 따른 Unity Answer SDP 전송.");
            }
        }

    }

    IEnumerator GetResquest(string uri)
    {
        webRequest = UnityWebRequest.Get(uri + "/get_offer");
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    get_next_level();
                    break;
            }
        }
    }
    
    public void get_next_level()
    {
        var data = JsonUtility.FromJson<SignalingMessage>(webRequest.downloadHandler.text);
        if (data.type == RTCSdpType.Offer)
        {
            RTCSessionDescription desc = new RTCSessionDescription();
            desc.sdp = data.sdp;
            desc.type = data.type;
            peer_connection_remote_acces(desc);

            StartCoroutine(Createanswer());

        }
        else
        {
            Debug.Log("74error");

        }
    }

    async void peer_connection_remote_acces(RTCSessionDescription desc)
    {
        peerConnection.SetRemoteDescription(ref desc);
        await Task.Delay(1000); 

    }

    async void  peer_local_connection_access(RTCSessionDescription desc)
    {
        peerConnection.SetLocalDescription(ref desc);
        await Task.Delay(1000); 
        StartCoroutine(PostResquest(desc));
    }

}