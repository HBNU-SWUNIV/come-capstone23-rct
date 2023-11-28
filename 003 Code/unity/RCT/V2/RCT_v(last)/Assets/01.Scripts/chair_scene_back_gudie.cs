using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chair_scene_back_gudie : MonoBehaviour
{

    public GameObject Back_guide;
    public GameObject yes_bt;
    public GameObject no_bt;
    public void Cancel_to_stay()
    {
    
        Back_guide.SetActive(false);
        // gameObject.SetActive(false);
    }

    public void Back_to_one()
    {
        LoadingSceneController.LoadScene_back("Select_device"); // 이 부분은 그대로 둬도 됩니다.
    }

    public void select_classify()
    {
        

    }
}
