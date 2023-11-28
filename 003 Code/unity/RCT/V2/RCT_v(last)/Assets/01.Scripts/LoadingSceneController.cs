using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    // Start is called before the first frame update
    public static void LoadScene(string sceneName)
    {
        // nextScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }
    public static void LoadScene_back(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Select_device");
    }

}
