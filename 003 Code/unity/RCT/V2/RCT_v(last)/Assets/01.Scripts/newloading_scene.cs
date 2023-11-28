using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class newloading_scene : MonoBehaviour
{
    public GameObject[] cubes; // 21개의 큐브 오브젝트 배열
    public float blinkDuration = 0.5f; // 발광 지속 시간 (초)
    public float repeatInterval = 1.0f; // 반복 간격 (초)
    public int numberOfBlinks = 10; // 발광 횟수
    public Material glowMaterial; // 발광 머티리얼

    private int currentBlinkCount = 0;
    private int currentCubeIndex = 0;
    // private bool isBlinking = false;
    private Material originalMaterial;
    public GameObject l_text;
    private string nextScene = "chair_scenes"; // 다음 씬 이름

    
    void Start()
    {
        originalMaterial = cubes[0].GetComponent<Renderer>().material;
        StartCoroutine(StartBlinking());
        Debug.Log("in newloading_scene");
        // Debug.Log(GameManager.Instance.loading_t);
        // Debug.Log("in newloading_scene");
        // real_text();
    }

    IEnumerator StartBlinking()
    {
        while (currentBlinkCount < numberOfBlinks)
        {

            cubes[currentCubeIndex].GetComponent<Renderer>().material = glowMaterial;
            yield return new WaitForSeconds(blinkDuration);
            cubes[currentCubeIndex].GetComponent<Renderer>().material = originalMaterial;
            currentCubeIndex = (currentCubeIndex + 1) % cubes.Length;
            currentBlinkCount++;

            if (currentCubeIndex == 0)
            {
                yield return new WaitForSeconds(repeatInterval);
            }

            l_text.GetComponent<Text>().text = GameManager.Instance.loading_t;

            if (currentBlinkCount >= numberOfBlinks)
            {
                
                SceneManager.LoadScene(nextScene);
                
            }
            
        }
    }

    public void real_text()
    {
        Debug.Log("in text");
        while(true)
        {
            if (currentBlinkCount >= numberOfBlinks)
            {
                
                if (GameManager.Instance.loading_t == "Receive data from channel")
                {
                    break;
                }

            }
            else
            {
                l_text.GetComponent<Text>().text = GameManager.Instance.loading_t;
                
            }
        }
    }
    
}
