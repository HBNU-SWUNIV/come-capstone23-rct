using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image ProgressBar;
    // Start is called before the first frame update
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    public static void LoadScene_back(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Select_device");
    }
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        
        op.allowSceneActivation = false;
        // false로 설정시 씬을 90퍼센트까지만 로드한 상태로 대기하며
        // true로 설정해주면 그제서야 씬을 넘겨줌.
        // scene로딩에 따라 로딩 속도를 변경해주기 위해 false로 설정해주는 것이 필요.

        float timer = 0f;
        while(!op.isDone) //scene로딩이 끝나지 않은 상태일때
        {
            yield return null; // unity로 제어권 넘겨주기
            timer += Time.unscaledDeltaTime;
            ProgressBar.fillAmount = Mathf.Lerp(0, 1f, timer/2);

            // fake loading : 방식의 이름
            if(op.progress < 0.9f)
            {
                ProgressBar.fillAmount = op.progress;
                // timer += Time.unscaledDeltaTime;
                // ProgressBar.fillAmount = Mathf.Lerp(0, 0.9f, timer);
            }
            else
            {
                // timer += Time.unscaledDeltaTime;
                // ProgressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(ProgressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

}
