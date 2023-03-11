using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    Image blackImage;

    float alpha;

    bool isFadingIn, isFadingOut, isReadyToNextScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void setBlackImageAlpha()
    {
        blackImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }

    private void switchFlag()
    {
        isFadingOut = false;
        isFadingIn = true;
    }

    private void initialize()
    {
        canvas.SetActive(true);
        alpha = 1.0f;
        setBlackImageAlpha();
        switchFlag();
        isReadyToNextScene = false;
    }

    public void FadeOut()
    {
        isFadingOut = true;
    }

    public void Reset()
    {
        if (!isReadyToNextScene) return;

        isReadyToNextScene = false;
        switchFlag();
    }
    public bool IsTransitionAnimating()
    {
        return isFadingIn || isFadingOut;
    }

    public bool IsReadyToNextSceneNow()
    {
        return isReadyToNextScene && isFadingOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyToNextScene) return;

        if (isFadingIn)
        {
            alpha -= Time.deltaTime;
            if (alpha > 0.0f) setBlackImageAlpha();
            else
            {
                alpha = 0.0f;
                setBlackImageAlpha();
                canvas.SetActive(false);
                isFadingIn = false;
            }
        }
        if (isFadingOut)
        {
            canvas.SetActive(true);

            alpha += Time.deltaTime;
            if (alpha < 1.0f) setBlackImageAlpha();
            else
            {
                alpha = 1.0f;
                setBlackImageAlpha();
                isReadyToNextScene = true;
            }
        }
    }
}
