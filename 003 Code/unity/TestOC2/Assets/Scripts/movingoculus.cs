using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class movingoculus : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitialize();
    }

    void OnDestroy()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
    }

    void Update()
    {
       
        if (leftController.isValid)
        {
            Debug.Log("left controller connected and ....");
            Vector3 leftPosition;
            leftController.TryGetFeatureValue(CommonUsages.devicePosition, out leftPosition);

            Quaternion leftRotation;
            leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out leftRotation);

            Vector2 leftStick;
            if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
            {
                Debug.Log("Left stick position: " + leftStick);
            }

            // Check left button presses
            bool leftButton1;
            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out leftButton1) && leftButton1)
            {
                Debug.Log("Left button 1 pressed");
            }

            bool leftButton2;
            if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out leftButton2) && leftButton2)
            {
                Debug.Log("Left button 2 pressed");
            }


            //Debug.Log("Left controller position: " + leftPosition);
            //Debug.Log("Left controller rotation: " + leftRotation);
        }

        if (rightController.isValid)
        {
            Debug.Log("right controller connected and ....");
            Vector3 rightPosition;
            rightController.TryGetFeatureValue(CommonUsages.devicePosition, out rightPosition);

            Quaternion rightRotation;
            rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out rightRotation);

            // Get right stick position
            Vector2 rightStick;
            if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
            {
                Debug.Log("Right stick position: " + rightStick);
            }

            // Check right button presses
            bool rightButton1;
            if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out rightButton1) && rightButton1)
            {
                Debug.Log("Right button 1 pressed");
            }

            bool rightButton2;
            if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out rightButton2) && rightButton2)
            {
                Debug.Log("Right button 2 pressed");
            }

            //Debug.Log("Right controller position: " + rightPosition);
            //Debug.Log("Right controller rotation: " + rightRotation);
        }
    }

    void TryInitialize()
    {
        if (leftController.isValid && rightController.isValid)
        {
            return;
        }

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

    void OnDeviceConnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            leftController = device;
        }
        else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {
            rightController = device;
        }
    }
}