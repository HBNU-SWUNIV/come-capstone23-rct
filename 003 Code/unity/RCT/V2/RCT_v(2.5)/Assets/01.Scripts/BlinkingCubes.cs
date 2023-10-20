using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlinkingCubes : MonoBehaviour
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
    static string nextScene;

    void Start()
    {
        originalMaterial = cubes[0].GetComponent<Renderer>().material;
        StartCoroutine(StartBlinking());
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
        }

        if (currentBlinkCount >= numberOfBlinks)
        {
            LoadScene(nextScene);
        }
    }

    void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("chair_scenes");
    }

}
