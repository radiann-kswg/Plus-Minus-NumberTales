using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    string nextSceneName;

    [SerializeField]
    HyakkaCommandTrigger hyakkaC;

    [SerializeField]
    private InputAction _actionSubmit;

    private void OnEnable()
    {
        _actionSubmit?.Enable();
    }

    private void OnDisable()
    {
        _actionSubmit?.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.instance.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TransitionManager.instance.IsTransitionAnimating())
        {
            if (_isSubmitted())
            {
                if (!hyakkaC)
                {
                    SE.instance.PlayClip(0);
                    TransitionManager.instance.FadeOut();
                }
                else
                {
                    SE.instance.PlayClip(hyakkaC.IsSuccessInput() ? 5 : 0);
                    Messerger.instance.IsUnlockedNo0 = hyakkaC.IsSuccessInput();
                    TransitionManager.instance.FadeOut();
                }
            }
        }

        if (TransitionManager.instance.IsReadyToNextSceneNow())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private bool _isSubmitted()
    {
        float _thres = 0.1f;
        return _actionSubmit.ReadValue<float>() > 1f - _thres;
    }
}