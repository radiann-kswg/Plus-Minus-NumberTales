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

    float alpha = 0.0f;
    bool isFadingIn = false, isFadingOut = false;

    private void setBlackImageAlpha()
    {
        if (_backImage)
        {
            _backImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }
    public bool Set()
    {
        if (isFadingIn)
        {
            alpha -= Time.deltaTime;
            if (alpha > 0.0f) setBlackImageAlpha();
            else
            {
                alpha = 0.0f;
                setBlackImageAlpha();
                //_backImage.gameObject.SetActive(false);
                _player.gameObject.SetActive(false);
                isFadingIn = false;
            }
            return true;
        }
        if (isFadingOut)
        {
            alpha += Time.deltaTime;
            if (alpha < 1.0f) setBlackImageAlpha();
            else
            {
                alpha = 1.0f;
                setBlackImageAlpha();
                _player.gameObject.SetActive(true);
                isFadingOut = false;
            }
            return true;
        }
        return false;
    }

    public void StartTransition(bool isFadeIn)
    {
        if (isFadeIn) isFadingIn = true;
        else
        {
            //_backImage.gameObject.SetActive(true);
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
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(_PlayPV());
    }

    private IEnumerator _PlayPV()
    {
        if (!_player || !_backImage) yield break;
        isFadingOut = true;
        while (Set())
        {
            if (TransitionManager.instance.IsTransitionAnimating())
                yield break;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _player.Play();
        while (_player.isPlaying)
        {
            if (TransitionManager.instance.IsTransitionAnimating())
                yield break;
            yield return null;
        }
        isFadingIn = true;
        while (Set())
        {
            if (TransitionManager.instance.IsTransitionAnimating())
                yield break;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StartCoroutine(_WaitPlaying());
    }
}
