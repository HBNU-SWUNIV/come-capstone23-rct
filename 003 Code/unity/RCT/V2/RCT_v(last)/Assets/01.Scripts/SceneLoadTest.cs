using UnityEngine;

public class SceneLoadTest : MonoBehaviour
{
    
    public void On_start_click()
    {
        LoadingSceneController.LoadScene("LoadingScene");
    }

}
