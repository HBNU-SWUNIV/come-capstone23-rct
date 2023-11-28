using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeIn_sphere : MonoBehaviour
{
    public float fadeInTime = 2f; // fade in에 걸리는 시간
    public float disableTime = 5f; // 비활성화까지의 시간

    public Renderer objectRenderer;
    public GameObject fadesphere;
    private Color originalColor;
    private float fadeStartTime;
    private Vector3 startFadePosition;
    // public GameManager GM;

    void Start()
    {
        // objectRenderer = GetComponent<Renderer>();

        // 원래의 색상을 저장
        originalColor = objectRenderer.material.color;

        // 초기 투명도를 1로 설정
        SetObjectAlpha(1f);

        // 시작 시간을 기록
        fadeStartTime = Time.time;
    }

    void Update()
    {
        // 현재까지의 경과 시간 계산
        float elapsedTime = Time.time - fadeStartTime;

        // Fade In
        if (elapsedTime < fadeInTime)
        {
            float currentAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            SetObjectAlpha(currentAlpha);
        }
        else
        {
            // 시간이 지나면 게임 오브젝트 비활성화
            fadesphere.SetActive(false);
        }
    }

    void SetObjectAlpha(float alpha)
    {
        Color newColor = originalColor;
        newColor.a = alpha;
        objectRenderer.material.color = newColor;
    }
   
}
