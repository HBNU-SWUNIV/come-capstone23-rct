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
    public InputDevice leftController;
    public InputDevice rightController;

    public PublisherSocket client;
    private bool left1_previousButtonState;
    private bool left1_currentButtonState;
    private bool left2_previousButtonState;
    private bool left2_currentButtonState;
    private bool right1_previousButtonState;
    private bool right1_currentButtonState;
    private bool right2_previousButtonState;
    private bool right2_currentButtonState;
    void Start()
    {
        Debug.Log("Start.");
        AsyncIO.ForceDotNet.Force();
        checksocket();
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitialize();
        left1_previousButtonState = false;
        left1_currentButtonState = false;
        left2_previousButtonState = false;
        left2_currentButtonState = false;
        right1_previousButtonState = false;
        right1_currentButtonState = false;
        right2_previousButtonState = false;
        right2_currentButtonState = false;
        
    }

    void Update()
    {
        manual_stick_data();

        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out left1_currentButtonState);
        
        if (left1_currentButtonState != left1_previousButtonState)
        {
            Debug.Log("Left button 1 pressed");
            client.SendFrame("ctrl");
            left1_previousButtonState = left1_currentButtonState ;
      
        }
 
        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out left2_currentButtonState);
        if (left2_currentButtonState != left2_previousButtonState)
        {
            Debug.Log("Left button 2 pressed");
            left2_previousButtonState = left2_currentButtonState;
            
        }

        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out right1_currentButtonState);
        {
            if (right1_currentButtonState != right1_previousButtonState)
            {
                Debug.Log("Right button 1 pressed");
                client.SendFrame("space");
                right1_previousButtonState = right1_currentButtonState;
            }
        }

        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out right2_currentButtonState);
        {
            if (right2_currentButtonState != right2_previousButtonState)
            {
                Debug.Log("Right button 2 pressed");
                client.SendFrame("alt");
                right2_previousButtonState = right2_currentButtonState;
               
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
        Debug.Log("TryInitialize");
        if (leftController.isValid && rightController.isValid)
        {
            return;
        }

        Debug.Log("not ensure");
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
            Debug.Log("left controller connected");
        }

        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
            Debug.Log("right controller connected");
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
                
                if (leftStick[1] > 0.998)
                {
                    Debug.Log("w: "+leftStick[1]);
                    client.SendFrame("w");
                }

                if(leftStick[1] < -0.998)
                {
                    Debug.Log("s:"+leftStick[1]);
                    client.SendFrame("s");
                }

                if(leftStick[0] > 0.9)
                {
                    Debug.Log("d:"+leftStick[0]);
                    client.SendFrame("d");
                }

                if(leftStick[0] < -0.9)
                {
                    Debug.Log("a:"+leftStick[0]);
                    client.SendFrame("a");
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
                    if (rightStick[1] > 0.998)
                    {
                        client.SendFrame("up");
                    }

                    if(rightStick[1] < -0.998)
                    {
                        client.SendFrame("down");
                    }

                    if(rightStick[0] > 0.998)
                    {
                        client.SendFrame("left");
                    }

                    if(rightStick[0] < -0.998)
                    {
                        client.SendFrame("right");
                    }
                }

            }
        await Task.CompletedTask;
    }

    void OnDeviceConnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            Debug.Log("leftdevice");
            leftController = device;
        }
        else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {
            Debug.Log("rightdevice");
            rightController = device;
        }
        
    }

}
