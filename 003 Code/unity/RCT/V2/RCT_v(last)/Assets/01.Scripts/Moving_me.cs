using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class Moving_me : MonoBehaviour
{
    public Transform Player;
    public GameManager gameManager;
    // public InputDevice leftController;
    // public InputDevice rightController;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(manual_stick_data());
        StartCoroutine(WaitForGameManagerInitialization());
        // InputDevices.deviceConnected += OnDeviceConnected;
        // TryInitialize();
    }
    IEnumerator WaitForGameManagerInitialization()
    {
        while (gameManager == null)
        {
            yield return null;
        }

        StartCoroutine(manual_stick_data());
    }

    // async void manual_stick_data()
    IEnumerator manual_stick_data()
    {
        while(true)
        {
            if (gameManager.leftController.isValid)
            {
                Vector2 leftStick;
                if (gameManager.leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
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
            if (gameManager.rightController.isValid)
                {
                    // Get right stick position
                    Vector2 rightStick;
                    if (gameManager.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
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

        }
        
        
        
        // await Task.CompletedTask;
    }

}
