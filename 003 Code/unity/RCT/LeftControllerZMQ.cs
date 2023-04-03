using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using NetMQ.Sockets;
using NetMQ;

public class LeftControllerZMQ : MonoBehaviour
{
    public InputDevice leftController;
    public InputDevice rightController;

    public PublisherSocket client;
    private bool leftButton1 = false;
    private bool rightButton1 = false;
    private bool rightButton2 = false;

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
        StartCoroutine(manual_stick_data());
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

    // async void manual_stick_data()
    // {
    //     await T
    // }

    IEnumerator manual_stick_data()
    {
        if (leftController.isValid)
        {
  
            Debug.Log("left controller connected and ....");

            Vector2 leftStick;
            if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
            {
                
                if (leftStick[1] > 0.999)
                {
                    Debug.Log("w: "+leftStick[1]);
                    client.SendFrame("w");
                    yield return new WaitForSecondsRealtime(60); 
                }

                if(leftStick[1] < -0.999)
                {
                    Debug.Log("s:"+leftStick[1]);
                    client.SendFrame("s");
                    yield return new WaitForSeconds(60);
                }

                if(leftStick[0] > 0.999)
                {
                    Debug.Log("a:"+leftStick[0]);
                    client.SendFrame("a");
                    yield return new WaitForSeconds(60);
                }

                if(leftStick[0] < -0.999)
                {
                    Debug.Log("d:"+leftStick[0]);
                    client.SendFrame("d");
                    yield return new WaitForSeconds(60);
                }
                
            }

            // Check left button presses
            
            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out leftButton1) && leftButton1)
            {
                if (leftButton1)
                {
                    Debug.Log("Left button 1 pressed");
                    client.SendFrame("ctrl");
                    leftButton1 = false;
                    yield return null;
                }
                
            }

            bool leftButton2;
            if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out leftButton2) && leftButton2)
            {
                Debug.Log("Left button 2 pressed");
                
            }

        }
        leftButton1 = false;
        
        if (rightController.isValid)
            {
                Debug.Log("right controller connected and ....");

                // Get right stick position
                Vector2 rightStick;
                if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
                {
                    Debug.Log("Right stick position: ");
                    if (rightStick[1] > 0.999)
                    {
                 
                        client.SendFrame("up");
                        yield return new WaitForSecondsRealtime(60); 
                    }

                    if(rightStick[1] < -0.999)
                    {
                    
                        client.SendFrame("down");
                        yield return new WaitForSeconds(60);
                    }

                    if(rightStick[0] > 0.999)
                    {
                 
                        client.SendFrame("left");
                        yield return new WaitForSeconds(60);
                    }

                    if(rightStick[0] < -0.999)
                    {
                     
                        client.SendFrame("right");
                        yield return new WaitForSeconds(60);
                    }
                }

                // Check right button presses
                
                if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out rightButton1) && rightButton1)
                {
                    if (rightButton1)
                    {
                        Debug.Log("Right button 1 pressed");
                        client.SendFrame("space");
                        rightButton1 = false;
                        yield return null;
                    }
                    
                }

                
                if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out rightButton2) && rightButton2)
                {
                    if (rightButton2)
                    {
                        Debug.Log("Right button 2 pressed");
                        client.SendFrame("alt");
                        rightButton2 = false;
                        yield return null;
                    }
                    
                }



            }
        rightButton1 = false;
        rightButton2 = false;

        yield return new WaitForSeconds(60);

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
