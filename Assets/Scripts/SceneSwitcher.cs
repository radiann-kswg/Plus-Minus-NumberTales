using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    string nextSceneName;

    [SerializeField]
    HyakkaCommandTrigger hyakkaC;

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
            if (Input.GetButtonDown("Submit"))
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

        if(TransitionManager.instance.IsReadyToNextSceneNow())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
