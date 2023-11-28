using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using NetMQ;
using UnityEngine.UI;
using NetMQ.Sockets;


public class RobotArm_CT : MonoBehaviour
{

    [AddComponentMenu("UI/Effects/Outline", 81)]
    public Transform Player;
    public InputDevice leftController;
    public InputDevice rightController;
    public PublisherSocket client;
    public GameObject canvas;
    public GameObject Back_guide;
    public GameObject no_button;
    private Transform playerCamera;
    private bool left1_currentButtonState;

    private bool left2_currentButtonState;

    private bool right1_currentButtonState;

    private bool right2_currentButtonState;
    private float left_trigger_on;
    private float right_trigger_on;
    // 제일 아래 오브젝트 테두리 
    public GameObject left_T_text; // 왼쪽 트리거 텍스트 : 기능 없음
    public GameObject left_G_n_text; // 왼쪽 가이드 켜기
    public GameObject left_G_f_text; // 왼쪽 가이드 끄기
    public GameObject left_M_text; // 왼쪽 자동차 움직이기
    public GameObject left_Grab_text; // 왼쪽 그랩 암 움직이기
    public GameObject right_T_text; // 오른쪽 트리거 텍스트 : 기능 없음
    public GameObject right_G_n_text; // 오른쪽 가이드 켜기
    public GameObject right_G_f_text; // 오른쪽 가이드 끄기
    public GameObject right_M_text; // 오른쪽 자동차 움직이기
    public GameObject right_Grab_text; // 오른쪽 그랩 암 움직이기
    // 그립 후 조종 테두리 오브젝트
    public GameObject L_grip_go; // 그립 후 앞으로
    public GameObject L_grip_left; // 그립 후 앞으로
    public GameObject L_grip_right; // 그립 후 앞으로
    public GameObject L_grip_back; // 그립 후 앞으로
    public GameObject R_grip_go; // 그립 후 앞으로
    public GameObject R_grip_left; // 그립 후 앞으로
    public GameObject R_grip_right; // 그립 후 앞으로
    public GameObject R_grip_back; // 그립 후 앞으로
    // 조종에 따른 글자 수정 text
    public Text Vehicle_Speed;
    public Text Current_Direction;
    
    int left_w = 0;
    int left_s = 0;
    int left_a = 0;
    int left_d = 0;
    int right_up = 0;
    int right_down = 0;
    int right_left = 0;
    int right_right = 0;
    int button_3 = 0;
    int button_4 = 0;

    int vehicle_speed_gb = 0;
    int vehicle_speed_lr = 0;
    string current_direction_gb = "stop";
    string current_direction_lr = "stop";
    string pre_direction = "default";

    int vehicle_speed_go = 1;
    int vehicle_speed_back = -1;
    int vehicle_speed_left = 1;
    int vehicle_speed_right = -1;


    private void Start()
    {
        Vehicle_Speed.text = vehicle_speed_gb.ToString() + " | " + vehicle_speed_lr.ToString();
        Current_Direction.text = current_direction_gb + " " + current_direction_lr;
//         AsyncIO.ForceDotNet.Force();
//         checksocket();
        canvas.SetActive(true);
        playerCamera = Camera.main.transform;
        canvas.transform.position = playerCamera.position + playerCamera.forward * 0.05f;
        canvas.transform.LookAt(playerCamera);

        leftController = GameManager.Instance.leftController;
        rightController = GameManager.Instance.rightController;
        client = GameManager.Instance.client;

        StartCoroutine(Check_button());
        StartCoroutine(manual_stick_data());

        // Check_button();

    }
    // public void Check_button()
    IEnumerator Check_button()
    {
        while(true)
        {
            leftController.TryGetFeatureValue(CommonUsages.primaryButton, out left1_currentButtonState);
            if (left1_currentButtonState)
            {
                //왼쪽 밑에
                canvas.SetActive(false);

            }
     
            leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out left2_currentButtonState);
            if (left2_currentButtonState)
            {
                
                canvas.SetActive(false);
                canvas.SetActive(true);
                canvas.transform.LookAt(playerCamera);
                
            }

            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out right1_currentButtonState);
            {
                right_G_f_text.GetComponent<Shadow>().effectColor = default;
                if (right1_currentButtonState)
                {
                    right_G_f_text.GetComponent<Shadow>().effectColor = Color.white;
                    
                    button_3 +=1;

                    if (button_3 == 10)
                    {
                        Debug.Log("End Effector on/off");
                        // client.SendFrame("3");
                        button_3 = 0;
                        right1_currentButtonState = false; 
                
                    }
                }
            }

            rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out right2_currentButtonState);
            {
                right_G_n_text.GetComponent<Shadow>().effectColor = default;
                if (right2_currentButtonState)
                {
                    right_G_n_text.GetComponent<Shadow>().effectColor = Color.white;
                    button_4 +=1;

                    if (button_4 == 10)
                    {
                        Debug.Log("End Effector off");
                        // client.SendFrame("4");
                        button_4 = 0;
                        right2_currentButtonState = false;
                
                    }

                }
             }

            leftController.TryGetFeatureValue(CommonUsages.trigger, out left_trigger_on);
            {
                
                if(left_trigger_on != 0)
                {
                    
                    Debug.Log("left_trigger_on");
                    //  // // 텍스트 커지는 애니메이션
                    // left_T_text.fontSize = left_T_text.fontSize + 19;
                    // left_T_text.fontStyle = FontStyle.Bold;
                    // image 테두리 빛나기
                    left_T_text.GetComponent<Shadow>().effectColor = Color.white;
        
                    
                    
                }
                else{
                    // left_T_text.fontSize = 26;
                    // left_T_text.fontStyle = FontStyle.Normal;
                    left_T_text.GetComponent<Shadow>().effectColor = default;
                }
            }
            rightController.TryGetFeatureValue(CommonUsages.trigger, out right_trigger_on);
            {
                
                if(right_trigger_on != 0)
                {
                    
                    Debug.Log("left_trigger_on");
                    //  // // 텍스트 커지는 애니메이션
                    // left_T_text.fontSize = left_T_text.fontSize + 19;
                    // left_T_text.fontStyle = FontStyle.Bold;
                    // image 테두리 빛나기
                    right_T_text.GetComponent<Shadow>().effectColor = Color.white;
        
                    
                    
                }
                else{
                    // left_T_text.fontSize = 26;
                    // left_T_text.fontStyle = FontStyle.Normal;
                    right_T_text.GetComponent<Shadow>().effectColor = default;
                }
            }

            yield return new WaitForSeconds(0.1f);  
        }

    }

    IEnumerator manual_stick_data()
    {
        while(true)
        {
            if (leftController.isValid)
            {
                left_M_text.GetComponent<Shadow>().effectColor = default;
                // car moving 
                Vector2 leftStick;
                if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick))
                {
                    
                    if (leftStick[1] > 0.981)
                    {
                        left_M_text.GetComponent<Shadow>().effectColor = Color.white;
                        left_s +=1;
                        
                        if (left_s == 15)
                        {
                            // Debug.Log("Transfer Data :  2");
                            client.SendFrame("1");
                            setting_str("Go");
                            left_s = 0;
            
                        }
                    }

                    if(leftStick[1] < -0.981)
                    {
                        left_M_text.GetComponent<Shadow>().effectColor = Color.white;
                        left_w +=1;
                        
                        if (left_w == 15)
                        {
                            // Debug.Log("Transfer Data : 1");
                            client.SendFrame("2");
                            setting_str("Back");
                            left_w = 0;
                            
                        }
                    }

                    if(leftStick[0] > 0.981)
                    {
                        left_M_text.GetComponent<Shadow>().effectColor = Color.white;
                        left_d +=1;
                        
                        if (left_d == 15)
                        {
                            Debug.Log("Transfer Data :  4");
                            client.SendFrame("4");
                            setting_str("Right");
                            left_d = 0;
                        }
                    }

                    if(leftStick[0] < -0.981)
                    {
                        left_M_text.GetComponent<Shadow>().effectColor = Color.white;
                        left_a +=1;
                        if (left_a == 15)
                        {
                            Debug.Log("Transfer Data :  3");
                            
                            client.SendFrame("3");
                            setting_str("Left");
                            left_a = 0;
                        }
                    }

                }
                if (leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool leftgripButton))
                {
                    left_Grab_text.GetComponent<Shadow>().effectColor = default;
                    
                    if (leftgripButton){
                        // robot_Arm moving code
                        left_Grab_text.GetComponent<Shadow>().effectColor = Color.white;
                        
                        Vector3 leftStick_g;
                        if (leftController.TryGetFeatureValue(CommonUsages.devicePosition, out leftStick_g))
                        {
                            // Debug.Log("leftstick z : "+leftStick_g.z+ "\n"+"leftstick x : "+leftStick_g.x);///
                            if (leftStick_g.z > 0.228 && leftStick_g.x < -0.144)
                            {
                                left_s +=1;
                                
                                if (left_s == 14)
                                {
                                    Debug.Log("Transfer Data :  6");
                                    client.SendFrame("10");
                                    L_grip_go.GetComponent<Shadow>().effectColor = Color.white;
                                    leftController.SendHapticImpulse(0, (float)0.3,(float)0.3);
                                    left_s = 0;
                                }
                                    
                            }

                            if(leftStick_g.z < -0.228 && leftStick_g.x < -0.144)
                            {
                                left_w +=1;
                                Debug.Log("Transfer Data :  7");
                                if (left_w == 14)
                                {
                                    client.SendFrame("11");
                                    L_grip_back.GetComponent<Shadow>().effectColor = Color.white;
                                    leftController.SendHapticImpulse(0, (float)0.3,(float)0.3);
                                    left_w = 0;
                                }
                            }

                            if(leftStick_g.x > -0.0022 && leftStick_g.z > -0.014 &&  leftStick_g.z < 0.228)
                            {
                                left_d +=1;
                                Debug.Log("Transfer Data :  8");
                                if (left_d == 14)
                                {
                                    client.SendFrame("12");
                                    L_grip_right.GetComponent<Shadow>().effectColor = Color.white;
                                    leftController.SendHapticImpulse(0, (float)0.3,(float)0.3);
                                    left_d = 0;
                                }
                            }

                            if(leftStick_g.x < -0.515 && leftStick_g.z > 0.008)
                            {
                                left_a +=1;
                                Debug.Log("Transfer Data :  9");
                                if (left_a == 14)
                                {
                                    client.SendFrame("13");
                                    L_grip_left.GetComponent<Shadow>().effectColor = Color.white;
                                    leftController.SendHapticImpulse(0, (float)0.3,(float)0.3);
                                    left_a = 0;
                                }
                            }
                        } 
                    }
                    else{
                        L_grip_go.GetComponent<Shadow>().effectColor = default;
                        L_grip_back.GetComponent<Shadow>().effectColor = default;
                        L_grip_left.GetComponent<Shadow>().effectColor = default;
                        L_grip_right.GetComponent<Shadow>().effectColor = default;
                    }
                    
                }
   
            }
        
            if (rightController.isValid)
            {

                // rotate view code
                Vector2 rightStick;
                if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick))
                {
                    right_M_text.GetComponent<Shadow>().effectColor = default;
                    float rotationSpeed = 50f; // 원하는 회전 속도
                    
                    if (rightStick[1] > 0.95 || rightStick[1] < -0.95)
                    {
                        right_M_text.GetComponent<Shadow>().effectColor = Color.white;
                        // Rotate around the y-axis (horizontal)
                        float rotationAmount = rightStick[1] * Time.deltaTime * rotationSpeed;
                        Player.Rotate(Vector3.left, rotationAmount);
                    }
                    

                    if(rightStick[0] > 0.95 || rightStick[0] < -0.95)
                    {
                        right_M_text.GetComponent<Shadow>().effectColor = Color.white;
                        float rotationAmount = rightStick[0] * Time.deltaTime * rotationSpeed;
                        Player.Rotate(Vector3.up, rotationAmount);
            
                    }
                }

                if (rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool rightgripButton))
                {
                    right_Grab_text.GetComponent<Shadow>().effectColor = default;
                    
                    if (rightgripButton)
                    {
                        right_Grab_text.GetComponent<Shadow>().effectColor = Color.white;
                        // robot_arm moving code 2
                        Vector3 rightStick_g;
                        if (rightController.TryGetFeatureValue(CommonUsages.devicePosition, out rightStick_g))
                        {
                            

                            if (rightStick_g.z > 0.228 && rightStick_g.x > 0.30)
                            {
                                right_down +=1;
                                
                                if (right_down == 14)
                                {
                                    Debug.Log("Transfer Data :  10전송");
                                    client.SendFrame("14");
                                    R_grip_go.GetComponent<Shadow>().effectColor = Color.white;
                                    rightController.SendHapticImpulse(1, (float)0.3,(float)0.3);
                                    right_down = 0;
                                }
                                    
                            }

                            if(rightStick_g.z < -0.0619 && rightStick_g.x > 0.285)
                            {
                                right_up +=1;
                                
                                if (right_up == 14)
                                {
                                    Debug.Log("Transfer Data : 11전송");
                                    client.SendFrame("15");
                                    R_grip_back.GetComponent<Shadow>().effectColor = Color.white;
                                    rightController.SendHapticImpulse(1, (float)0.3,(float)0.3);
                                    right_up = 0;
                                }
                            }

                            if(rightStick_g.x > 0.445 && rightStick_g.z > -0.001)
                            {
                                right_right +=1;
                                
                                if (right_right == 14)
                                {
                                    Debug.Log("Transfer Data : 12전송");
                                    client.SendFrame("16");
                                    R_grip_right.GetComponent<Shadow>().effectColor = Color.white;
                                    rightController.SendHapticImpulse(1, (float)0.3,(float)0.3);
                                    right_right = 0;
                                }
                            }

                            if(rightStick_g.x < 0.102 && rightStick_g.z > 0.093)
                            {
                                right_left +=1;
                                if (right_left == 14)
                                {
                                    Debug.Log("Transfer Data :  13 전송");
                                    client.SendFrame("17");
                                    R_grip_left.GetComponent<Shadow>().effectColor = Color.white;
                                    rightController.SendHapticImpulse(1, (float)0.3,(float)0.3);
                                    right_left = 0;
                                }
                            }
                        }
                    }
                    else{
                        R_grip_go.GetComponent<Shadow>().effectColor = default;
                        R_grip_back.GetComponent<Shadow>().effectColor = default;
                        R_grip_left.GetComponent<Shadow>().effectColor = default;
                        R_grip_right.GetComponent<Shadow>().effectColor = default;
                    }
                    
                
                }

            }
            yield return new WaitForSeconds(0.1f);  

        }
        
    }

//     void OnDestroy()
//     {
//         if (GameManager.Instance.client != null)
//         {
//             GameManager.Instance.client.Dispose();
//             NetMQConfig.Cleanup(false);
//         }
//     }



    void setting_str(string direction)
    {
        
        if (pre_direction == "default")
        {
            if (direction == "Go")
            {
                pre_direction = direction;
                current_direction_gb = direction;
                vehicle_speed_gb = vehicle_speed_gb + vehicle_speed_go;
            }
            else if(direction == "Left")
            {
                pre_direction = direction;
                current_direction_lr = direction;
                vehicle_speed_lr = vehicle_speed_lr + vehicle_speed_left;
            }
            else if(direction == "Right")
            {
                pre_direction = direction;
                current_direction_lr = direction;
                vehicle_speed_lr = vehicle_speed_lr + vehicle_speed_right;
            }
            else if(direction == "Back")
            {
                pre_direction = direction;
                current_direction_gb = direction;
                vehicle_speed_gb = vehicle_speed_gb + vehicle_speed_back;
            }

        }
        else
        {
            if(pre_direction == direction)
            {
                if (direction == "Go" )
                {
                    vehicle_speed_gb = vehicle_speed_gb + vehicle_speed_go;
                    current_direction_gb = direction;
                }
                else if(direction == "Back")
                {
                    vehicle_speed_gb = vehicle_speed_gb + vehicle_speed_back;
                    current_direction_gb = direction;
                }
                else if(direction == "Left")
                {
                    vehicle_speed_lr = vehicle_speed_lr + vehicle_speed_left;
                    current_direction_lr = direction;
                }
                else if(direction == "Right")
                {
                    vehicle_speed_lr = vehicle_speed_lr + vehicle_speed_right;
                    current_direction_lr = direction;
                }
                
            }
            else
            {
                if (direction == "Go")
                {
                    vehicle_speed_gb = vehicle_speed_gb + vehicle_speed_go;
                    current_direction_gb = direction;
                }
                else if(direction == "Left")
                {
                    vehicle_speed_lr = vehicle_speed_lr + vehicle_speed_left;
                    current_direction_lr = direction;
                }
                else if(direction == "Right")
                {
                    vehicle_speed_lr = vehicle_speed_lr + vehicle_speed_right;
                    current_direction_lr = direction;
                }
                else if(direction == "Back")
                {
                    vehicle_speed_gb = vehicle_speed_gb + vehicle_speed_back;
                    current_direction_gb = direction;
                    
                }
            }
        } 


        if (vehicle_speed_gb==0 && vehicle_speed_lr ==0) 
        {
            current_direction_gb = "Stop";
            current_direction_lr = "Stop";
        }
        else if(vehicle_speed_gb==0 && vehicle_speed_lr != 0)
        {
            current_direction_gb = "Stop";
        }
        else if(vehicle_speed_gb!=0 && vehicle_speed_lr == 0)
        {
            current_direction_lr = "Stop";
        }

        Vehicle_Speed.text = vehicle_speed_gb.ToString() + " | " + vehicle_speed_lr.ToString();
        Current_Direction.text = current_direction_gb + " | " + current_direction_lr;
        Current_Direction.fontSize = 35;

    }
    
}
