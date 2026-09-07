using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class How2PlayPVPlayer : MonoBehaviour
{
    [SerializeField]
    private Image _backImage;

    [SerializeField]
    private VideoPlayer _player;

    [SerializeField]
    float alpha = 0.0f;
    bool isFadingIn = false, isFadingOut = false;

    [SerializeField]
    private float pvWaitTime = 10.0f;

    private void setAlpha()
    {
        if(_backImage)
        {
            _backImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
        
        if(_player)
        {
            _player.targetCameraAlpha = alpha;
        }
    }
    public bool Set()
    {
        if (isFadingIn)
        {
            alpha -= Time.deltaTime;
            if (alpha > 0.0f) setAlpha();
            else
            {
                alpha = 0.0f;
                setAlpha();
                isFadingIn = false;
            }
            return true;
        }
        if (isFadingOut)
        {
            alpha += Time.deltaTime;
            if (alpha < 1.0f) setAlpha();
            else
            {
                alpha = 1.0f;
                setAlpha();
                isFadingOut = false;
            }
            return true;
        }
        return false;
    }

    public void StartTransition(bool isFadeIn)
    {
        if (isFadeIn)
        {
            isFadingIn = true;
        }
        else
        {
            _player.Play();
            isFadingOut = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_WaitPlaying());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator _WaitPlaying()
    {
        yield return new WaitForSeconds(pvWaitTime);
        StartCoroutine(_PlayPV());
    }

    private IEnumerator _PlayPV()
    {
        if (!_player || !_backImage) yield break;
        StartTransition(false);
        while (Set())
        {
            if (TransitionManager.instance.IsTransitionAnimating())
                yield break;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (_player.isPlaying)
        {
            if (TransitionManager.instance.IsTransitionAnimating())
                yield break;
            yield return null;
        }
        StartTransition(true);
        while (Set())
        {
            if (TransitionManager.instance.IsTransitionAnimating())
                yield break;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StartCoroutine(_WaitPlaying());
    }
}
