using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using NetMQ.Sockets;
using NetMQ;
using System.Threading.Tasks;
using System.Threading;
public class RobotArm_CT : MonoBehaviour
{
    public Transform Player;
    public InputDevice leftController;
    public InputDevice rightController;

    public PublisherSocket client;

    private bool left1_currentButtonState;

    private bool left2_currentButtonState;

    private bool right1_currentButtonState;

    private bool right2_currentButtonState;
    
    int left_w = 0;
    int left_s = 0;
    int left_a = 0;
    int left_d = 0;
    int right_up = 0;
    int right_down = 0;
    int right_left = 0;
    int right_right = 0;

    void Start()
    {
        Debug.Log("Start.");
        AsyncIO.ForceDotNet.Force();
        checksocket();
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitialize();
    
    }

    void Update()
    {
        manual_stick_data();

        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out left1_currentButtonState);
        if (left1_currentButtonState)
        {
            
            Debug.Log("Transfer Data :  ctrl \n Data 전송 유무 : 전송");
            client.SendFrame("ctrl");
      
        }
 
        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out left2_currentButtonState);
        if (left2_currentButtonState)
        {
            Debug.Log("Left button 2 pressed");
            client.SendFrame("ctrl");
            
        }

        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out right1_currentButtonState);
        {
            if (right1_currentButtonState)
            {
                Debug.Log("Transfer Data :  space \n Data 전송 유무 : 전송");
                client.SendFrame("space");
            }
        }

        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out right2_currentButtonState);
        {
            if (right2_currentButtonState)
            {
                Debug.Log("Transfer Data :  alt \n Data 전송 유무 : 전송");
                client.SendFrame("alt");
               
            }
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
            // 조이스틱 잡기 여부 확인
            if (leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTriggerButton))
            {
                Debug.Log("left grab joystick");
                Vector3 leftStick_g;
                if (leftController.TryGetFeatureValue(CommonUsages.devicePosition, out leftStick_g))
                {
                    // Debug.Log("in grab + controll position :  "+ leftStick_g.z + "\n"+ leftStick_g.x );
                    if (leftStick_g.z > 0.981)
                    {
                        left_s +=1;
                        
                        if (left_s == 9)
                        {
                            Debug.Log("Transfer Data :  s\n Data 전송 유무 : 전송");
                            client.SendFrame("s");
                            left_s = 0;
                        }
                            
                    }

                    if(leftStick_g.z < -0.981)
                    {
                        left_w +=1;
                        
                        if (left_w == 9)
                        {
                            Debug.Log("Transfer Data :  w\n Data 전송 유무 : 전송");
                            client.SendFrame("w");
                            left_w = 0;
                        }
                    }

                    if(leftStick_g.x > 0.981)
                    {
                        left_d +=1;
                        
                        if (left_d == 7)
                        {
                            Debug.Log("Transfer Data :  d\n Data 전송 유무 : 전송");
                            client.SendFrame("d");
                            left_d = 0;
                        }
                    }

                    if(leftStick_g.x < -0.981)
                    {
                        left_a +=1;
                        if (left_a == 7)
                        {
                            Debug.Log("Transfer Data :  a\n Data 전송 유무 : 전송");
                            client.SendFrame("a");
                            left_a = 0;
                        }
                    }
                } 
            }
            else{
                Vector2 leftStick;
                if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
                {
                    
                    if (leftStick[1] > 0.981)
                    {
                        left_s +=1;
                        
                        if (left_s == 9)
                        {
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
                            Debug.Log("Transfer Data :  a\n Data 전송 유무 : 전송");
                            client.SendFrame("a");
                            left_a = 0;
                        }
                    }
                }
            } 
        }
        
        if (rightController.isValid)
            {
                if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTriggerButton))
                {
                    Debug.Log("right grab joystick");
                    // Get right stick position
                    Vector3 rightStick_g;
                    if (rightController.TryGetFeatureValue(CommonUsages.devicePosition, out rightStick_g))
                    {
                        if (rightStick_g.z > 0.981)
                        {
                            right_down +=1;
                            
                            if (right_down == 9)
                            {
                                Debug.Log("Transfer Data :  s\n Data 전송 유무 : 전송");
                                client.SendFrame("s");
                                right_down = 0;
                            }
                                
                        }

                        if(rightStick_g.z < -0.981)
                        {
                            right_up +=1;
                            
                            if (right_up == 9)
                            {
                                Debug.Log("Transfer Data :  w\n Data 전송 유무 : 전송");
                                client.SendFrame("w");
                                right_up = 0;
                            }
                        }

                        if(rightStick_g.x > 0.981)
                        {
                            right_right +=1;
                            
                            if (right_right == 7)
                            {
                                Debug.Log("Transfer Data :  d\n Data 전송 유무 : 전송");
                                client.SendFrame("d");
                                right_right = 0;
                            }
                        }

                        if(rightStick_g.x < -0.981)
                        {
                            right_left +=1;
                            if (right_left == 7)
                            {
                                Debug.Log("Transfer Data :  a\n Data 전송 유무 : 전송");
                                client.SendFrame("a");
                                right_left = 0;
                            }
                        }
                    }
                
                }
                else{
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
