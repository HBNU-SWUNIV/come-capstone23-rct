using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;

public class Moving_me : MonoBehaviour
{
    public Transform Player;
    public InputDevice leftController;
    public InputDevice rightController;

    // Start is called before the first frame update
    void Start()
    {
        
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        manual_stick_data();
        // Vector3 right_controller_position;
        // rightController.TryGetFeatureValue(CommonUsages.devicePosition, out right_controller_position);
        // Debug.Log("right_controller_position : \n" + right_controller_position);

        // if(MoveFlag){
        //     Player.transform.Translate(Vector3.forward * Time.deltaTime * 10f);
        // }
        
    }
    // async void manual_stick_data()
    void manual_stick_data()
    {
        if (leftController.isValid)
        {
            Vector2 leftStick;
            if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
            {
                // 움직이는 속도 조절을 위한 스케일
                float moveSpeed = 0.35f;
                
                if (leftStick[1] > 0.95) 
                {
                    // MoveFlag = true;
                    // Debug.Log("Transfer Data :  s\n Data 전송 유무 : 전송");
                    //전진
                    Vector3 moveDirection = Player.forward * moveSpeed;
                    Player.position += moveDirection;
                   
                }

                if(leftStick[1] < -0.95)
                {
                    // MoveFlag = true;
                    // Debug.Log("Transfer Data :  w\n Data 전송 유무 : 전송");
                    // 후진
                    Vector3 moveDirection = -Player.forward * moveSpeed;
                    Player.position += moveDirection;

                }

                if(leftStick[0] > 0.95)
                {
                //    MoveFlag = true;
                    // Debug.Log("Transfer Data :  d\n Data 전송 유무 : 전송");
                    // 오른쪽 이동
                    Vector3 moveDirection = Player.right * moveSpeed;
                    Player.position += moveDirection;

                }

                if(leftStick[0] < -0.95)
                {
                    // MoveFlag = true;
                    // Debug.Log("Transfer Data :  a\n Data 전송 유무 : 전송");
                    // 왼쪽 이동
                    Vector3 moveDirection = -Player.right * moveSpeed;
                    Player.position += moveDirection;

                }
            }
            // Player.eulerAngles = new Vector3(0, Mathf.Atan2(leftStick[1], leftStick[0]) * Mathf.Rad2Deg, 0);
        }
        if (rightController.isValid)
            {
                // Get right stick position
                Vector2 rightStick;
                if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
                {
                    float rotationSpeed = 50f; // 원하는 회전 속도
                    
                    if (rightStick[1] > 0.95 || rightStick[1] < -0.95)
                    {

                        // Rotate around the y-axis (horizontal)
                        float rotationAmount = rightStick[1] * Time.deltaTime * rotationSpeed;
                        Player.Rotate(Vector3.left, rotationAmount);
                        // client.SendFrame("up");

                   
                    }

                    if(rightStick[0] > 0.95 || rightStick[0] < -0.95)
                    {
                    
                        // client.SendFrame("left");
                        float rotationAmount = rightStick[0] * Time.deltaTime * rotationSpeed;
                        Player.Rotate(Vector3.up, rotationAmount);
                       
                    }
                }

            }
        
        
        // await Task.CompletedTask;
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

 

    void OnDestroy()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
    }
}
