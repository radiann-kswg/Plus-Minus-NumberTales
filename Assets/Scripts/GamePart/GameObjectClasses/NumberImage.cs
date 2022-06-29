using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberImage : MonoBehaviour
{
    [SerializeField]
    Image numImage;
    [SerializeField]
    Image backFlameImage;

    float animTime = 0.0f;
    bool isAnimating = false;

    Color defaultNumColor, defaultFlameColor;

    const float ANIMATION_TIMELENGTH = 0.6f;

    string nowPlaying = "";

    // Start is called before the first frame update
    void Start()
    {
        defaultNumColor = numImage.color;
        if(backFlameImage) defaultFlameColor = backFlameImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating)
        {
            numImage.color = defaultNumColor;
            if(backFlameImage) backFlameImage.color = defaultFlameColor;
        }
    }

    public void SetNumImage(Sprite sprite)
    {
        numImage.sprite = sprite;
    }

    public void ResetEffect()
    {
        if (nowPlaying == null) return;
        StopCoroutine(nowPlaying);
        isAnimating = false;
        nowPlaying = "";
        animTime = 0.0f;
    }

    public IEnumerator PlayBurstEffect()
    {
        ResetEffect();
        isAnimating = true;
        nowPlaying = "PlayBurstEffect";
        animTime = Time.deltaTime;
        while (animTime < ANIMATION_TIMELENGTH)
        {
            float _gain = 1f - (ANIMATION_TIMELENGTH - animTime) * 0.8f;
            numImage.color = new Color(defaultNumColor.r, defaultNumColor.g * _gain, defaultNumColor.b * _gain);
            animTime += Time.deltaTime;
            yield return null;
        }
        animTime = 0.0f;
        numImage.color = defaultNumColor;
        isAnimating = false;
    }

    public IEnumerator PlayFiveEffect()
    {
        if(!backFlameImage) yield break;
        ResetEffect();
        isAnimating = true;
        nowPlaying = "PlayFiveEffect";
        animTime = Time.deltaTime;
        while (isAnimating)
        {
            if (animTime >= ANIMATION_TIMELENGTH * 2f * Mathf.PI) animTime = Mathf.Lerp(0.0f, 2f * Mathf.PI, animTime);
            float _gain = Mathf.Sin(animTime / ANIMATION_TIMELENGTH * 2f * Mathf.PI) * 0.75f;
            backFlameImage.color = new Color(defaultFlameColor.r, defaultFlameColor.g, defaultFlameColor.b * _gain);
            animTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator PlayChangeTargetNumEffect()
    {
        ResetEffect();
        isAnimating = true;
        nowPlaying = "PlayChangeTargetNumEffect";
        animTime = Time.deltaTime;
        while (animTime < ANIMATION_TIMELENGTH)
        {
            float _gainR = 1f - (ANIMATION_TIMELENGTH - animTime) * 0.7f;
            float _gainG = 1f - (ANIMATION_TIMELENGTH - animTime) * 0.4f;
            numImage.color = new Color(defaultNumColor.r * _gainR, defaultNumColor.g * _gainG, defaultNumColor.b);
            animTime += Time.deltaTime;
            yield return null;
        }
        animTime = 0.0f;
        numImage.color = defaultNumColor;
        isAnimating = false;
    }
}
