using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    string nextSceneName;

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
                SE.instance.PlayClip(0);
                TransitionManager.instance.FadeOut();
            }
        }

        if(TransitionManager.instance.IsReadyToNextSceneNow())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
