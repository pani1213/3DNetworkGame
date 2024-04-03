using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitUIManager : Singleton<HitUIManager>
{
    Image myImage;
    public float duration = 0.5f;
    public float currentTime = 0f;
    private void Awake()
    {
        myImage = GetComponent<Image>();
    }
    public IEnumerator FadeImageCoroutine()
    {
        // 0에서 0.5로 알파값 증가
        // 애니메이션 지속 시간 1초

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 0.5f, currentTime / duration);
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        // 목표 알파값을 정확히 0.5로 설정
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0.5f);

        // 0.5에서 0으로 알파값 감소
        currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0.5f, 0f, currentTime / duration);
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        // 목표 알파값을 정확히 0으로 설정
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0f);
    }
}
