using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using NetMQ.Sockets;
using NetMQ;
using System.Threading.Tasks;
using System.Threading;

public class LeftControllerZMQ : MonoBehaviour
{
    public Transform Player;
    public InputDevice leftController;
    public InputDevice rightController;

    public PublisherSocket client;
    // private bool left1_previousButtonState;
    private bool left1_currentButtonState;
    // private bool left2_previousButtonState;
    private bool left2_currentButtonState;
    // private bool right1_previousButtonState;
    // private bool right1_currentButtonState;
    // private bool right2_previousButtonState;
    // private bool right2_currentButtonState;
    int left_w = 0;
    int left_s = 0;
    int left_a = 0;
    int left_d = 0;
    // int right_up = 0;
    // int right_down = 0;
    // int right_left = 0;
    // int right_right = 0;

    void Start()
    {
        Debug.Log("Start.");
        AsyncIO.ForceDotNet.Force();
        checksocket();
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitialize();
        // left1_previousButtonState = false;
        // left1_currentButtonState = false;
        // left2_previousButtonState = false;
        // left2_currentButtonState = false;
        // right1_previousButtonState = false;
        // right1_currentButtonState = false;
        // right2_previousButtonState = false;
        // right2_currentButtonState = false;

        

        
    }

    void Update()
    {
        manual_stick_data();

        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out left1_currentButtonState);

//         Debug.Log("PUB socket connected");
        if (left1_currentButtonState)
        {
            
            Debug.Log("Transfer Data :  ctrl \n Data 전송 유무 : 전송");
            client.SendFrame("ctrl");
            // left1_previousButtonState = left1_currentButtonState ;
      
        }
//  
        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out left2_currentButtonState);
        if (left2_currentButtonState)
        {
            Debug.Log("Left button 2 pressed");
            // left2_previousButtonState = left2_currentButtonState;
            
        }

    }

    void OnDestroy()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
        client.Dispose();
        NetMQConfig.Cleanup(false);
    }


    void TryInitialize()
    {
        Debug.Log("기기를 찾는 중입니다.");
        if (leftController.isValid && rightController.isValid)
        {
            return;
        }

        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
            Debug.Log("left controller connected\nright controller connected");
        }

        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
            Debug.Log("left controller connected\nright controller connected");
        }
    }

    void checksocket()
    {
        client = new PublisherSocket();
        client.Bind("tcp://*:11012");
        

    } 

    async void manual_stick_data()
    {
        if (leftController.isValid)
        {
            Vector2 leftStick;
            if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
            {
                
                if (leftStick[1] > 0.981)
                {
                    left_s +=1;
                    
                    if (left_s == 9)
                    {
                        // Debug.Log("Left Controller down 범위 0.981 이상 입력 감지 \n Input Data : "+leftStick[1]);
                        Debug.Log("Transfer Data :  s\n Data 전송 유무 : 전송");
                        client.SendFrame("s");
                        left_s = 0;
                    }

                        
                }

                if(leftStick[1] < -0.981)
                {
                    left_w +=1;
                    
                    if (left_w == 9)
                    {
                    //    Debug.Log("Left Controller up 범위 -0.981 이상 입력 감지 \n Input Data : "+leftStick[1]);
                        Debug.Log("Transfer Data :  w\n Data 전송 유무 : 전송");
                        client.SendFrame("w");
                        left_w = 0;
                    }
                }

                if(leftStick[0] > 0.981)
                {
                    left_d +=1;
                    
                    if (left_d == 7)
                    {
                        // Debug.Log("Left Controller right 범위 0.981 이상 입력 감지 \n Input Data : "+leftStick[0]);
                        Debug.Log("Transfer Data :  d\n Data 전송 유무 : 전송");
                        client.SendFrame("d");
                        left_d = 0;
                    }
                }

                if(leftStick[0] < -0.981)
                {
                    left_a +=1;
                    if (left_a == 7)
                    {
                        // Debug.Log("Left Controller left 범위 -0.981 이상 입력 감지 \n Input Data : "+leftStick[0]);
                        Debug.Log("Transfer Data :  a\n Data 전송 유무 : 전송");
                        client.SendFrame("a");
                        left_a = 0;
                    }
                }
            }

        }
        // leftButton1 = false;
        
        if (rightController.isValid)
            {
                // Get right stick position
                Vector2 rightStick;
                if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
                {
                    float rotationSpeed = 50f; // 원하는 회전 속도
                    
                    if (rightStick[1] > 0.95 || rightStick[1] < -0.95)
                    {
                        
                        Debug.Log("??ws:"+rightStick[1]);
                        // Rotate around the y-axis (horizontal)
                        float rotationAmount = rightStick[1] * Time.deltaTime * rotationSpeed;
                        Player.Rotate(Vector3.left, rotationAmount);
                        // client.SendFrame("up");

                   
                    }

                    if(rightStick[0] > 0.95 || rightStick[0] < -0.95)
                    {
                    
                        Debug.Log("??ad:"+rightStick[0]);
                        // client.SendFrame("left");
                        float rotationAmount = rightStick[0] * Time.deltaTime * rotationSpeed;
                        Player.Rotate(Vector3.up, rotationAmount);
                       
                    }
                }

            }
        await Task.CompletedTask;
    }


    void OnDeviceConnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            Debug.Log("leftdevice device Connect");
            leftController = device;
        }
        else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {
            Debug.Log("rightdevice  device Connect");
            rightController = device;
        }
        
    }

}
