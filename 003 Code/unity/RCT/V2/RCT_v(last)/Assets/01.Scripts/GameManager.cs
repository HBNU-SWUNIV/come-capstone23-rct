using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Unity.WebRTC;
using UnityEngine.SceneManagement;
using NetMQ.Sockets;
using NetMQ;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance=null;

    /// <summary>
    /// Webrtc instance code
    /// </summary>
    // public WebRTC3 webrtcManager;

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
    private string SIGNALING_SERVER_URL = "http://192.168.50.88:19612/signaling";

    private string[] STUNSERVER = { "stun:stun.l.google.com:19302" };
    public UnityWebRequest webRequest;
    public UnityWebRequest postRequest;
    public RTCDataChannel channel;
    public RTCDataChannel receiveChannel;

    public RTCSessionDescription a_desc;

    public RTCPeerConnection peerConnection;
    private RTCConfiguration config;
    public byte[] byte_ ;
    public string loading_t;


    // Update is called once per frame
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 다른 씬으로 전환해도 유지
            Debug.Log("GameManager instance is created and marked as DontDestroyOnLoad.");
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 경우 중복 생성 방지
            Debug.Log("GameManager instance already exists. Destroying the duplicate.");
         
        }
        // Loading Scene 진행중 Webrtc  동기화 진행
        string cuurentScene = SceneManager.GetActiveScene().name;
        // InputDevices.deviceConnected += OnDeviceConnected;
        // TryInitialize();
        // first scene && third scene using controller
        
    }
    private void Start() {
        
        WebRTC.Initialize();
        config = new RTCConfiguration();
        RTCIceServer[] iceServers = { new RTCIceServer { urls = STUNSERVER } };
        config.iceServers = iceServers;

        peerConnection = new RTCPeerConnection(ref config);

        RTCDataChannelInit channelConfig = new RTCDataChannelInit();

        channel = peerConnection.CreateDataChannel("video",channelConfig);
        channel.OnOpen += () =>
        {
            Debug.Log("Data channel Open!");
        };
        channel.OnClose += () =>
        {
            Debug.Log("Data channel closed!");
        };

        channel.OnMessage += (data) =>
        {
            Debug.Log("Data channel received message: " + data);
        };

        StartCoroutine(GetResquest(SIGNALING_SERVER_URL));
        StartCoroutine(Peer_open());

        
        
        // StartCoroutine(Data_load());
    }

    // IEnumerator Controller_connection()
    // {
        
    //     InputDevices.deviceConnected += OnDeviceConnected;
    //     StartCoroutine(TryInitialize());
    //     yield break;
    // }
//     void OnDeviceConnected(InputDevice device)
//     {
//         if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
//         {
//             Debug.Log("leftdevice device Connect");
//             leftController = device;
//         }
//         else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
//         {
//             Debug.Log("rightdevice  device Connect");
//             rightController = device;
//         }
        
//     }

//     void TryInitialize()
//     {
        
//         if (leftController.isValid && rightController.isValid)
//         {
//             return;
//         }
        
//         var leftHandDevices = new List<InputDevice>();
//         InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
//         if (leftHandDevices.Count > 0)
//         {
//             leftController = leftHandDevices[0];
//             Debug.Log("left controller connected\nright controller connected");
//         }

//         var rightHandDevices = new List<InputDevice>();
//         InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
//         if (rightHandDevices.Count > 0)
//         {
//             rightController = rightHandDevices[0];
//             Debug.Log("left controller connected\nright controller connected");
//         }
//         Debug.Log("기기를 찾는 중입니다.");
//     }


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
                // GameManager.Instance.peerConnection1 = peerConnection;
                // LoadScene(nextScene);
                
                
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
                    loading_t = "WebRequest Success";
                    get_next_level();
                    break;
            }
        }
 
    }
    IEnumerator Peer_open()
    {
        
        while(true)
        {
            peerConnection.OnDataChannel = (channel_1) =>
            {
                
                channel = channel_1;

                channel.OnMessage = (byte[] bytes) =>
                {
                    loading_t = "Receive data from channel";
                    byte_ = bytes;
                };
            };
            

            yield return new WaitForSeconds(0.1f);
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
            loading_t = "local/remote peer_connection..";
        }
        else
        {
            Debug.Log("74error");

        }
    }

    async void peer_connection_remote_acces(RTCSessionDescription desc)
    {
        peerConnection.SetRemoteDescription(ref desc);
        await Task.Delay(500); 

    }

    async void  peer_local_connection_access(RTCSessionDescription desc)
    {
        peerConnection.SetLocalDescription(ref desc);
        await Task.Delay(500); 
        StartCoroutine(PostResquest(desc));
    }

//     void OnDestroy()
//     {
// //         InputDevices.deviceConnected -= OnDeviceConnected;
//         if (client != null)
//         {
//             client.Dispose();
            
//         }
//         NetMQConfig.Cleanup(false);

//     }
}
