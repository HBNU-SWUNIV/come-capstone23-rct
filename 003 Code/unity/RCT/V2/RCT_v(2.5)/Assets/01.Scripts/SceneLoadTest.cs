using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class SceneLoadTest : MonoBehaviour
{
    
    public void On_start_click()
    {
        LoadingSceneController.LoadScene("chair_scenes");
    }

}
