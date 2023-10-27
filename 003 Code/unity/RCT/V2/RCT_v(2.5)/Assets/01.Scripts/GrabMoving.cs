using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;
using UnityEngine.XR;

public class GrabMoving : MonoBehaviour
{
    public InputDevice leftController;
    public InputDevice rightController;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start chair moving");
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 right_controller_position;
        rightController.TryGetFeatureValue(CommonUsages.devicePosition, out right_controller_position);
        Debug.Log("right_controller_position : \n" + right_controller_position);

    }

    void OnDestroy()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
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
