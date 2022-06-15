using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelector : MonoBehaviour
{
    int target = 5;

    //[SerializeField]
    bool isUnlockedNo0 = false;

    [SerializeField]
    List<Image> characterCursol = new List<Image>();

    [SerializeField]
    Image characterDispImage;

    [SerializeField]
    string mainSceneName;

    [SerializeField]
    const float AXIS_THRESHOLD = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        isUnlockedNo0 = Messerger.instance.IsUnlockedNo0;

        changeCharacterDisp(true);
        TransitionManager.instance.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TransitionManager.instance.IsTransitionAnimating())
        {
            if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < -AXIS_THRESHOLD)
            {
                changeCharacter(false);

                SE.instance.PlayClip(1);
            }

            if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") >  AXIS_THRESHOLD)
            {
                changeCharacter(true);

                SE.instance.PlayClip(1);
            }

            if (Input.GetButtonDown("Submit"))
            {
                SE.instance.PlayClip(0);
                Messerger.instance.TargetNumMessage = target;

                TransitionManager.instance.FadeOut();
            }
        }

        if (TransitionManager.instance.IsReadyToNextSceneNow())
        {
            SceneManager.LoadScene(mainSceneName);
        }
    }

    void changeCharacter(bool isNext)
    {
        characterCursol[target].color = new Color(191f, 191f, 191f);

        if (isNext)
        {
            ++target;
            if (target >= characterCursol.Count) target = isUnlockedNo0 ? 0 : 1;
        }
        else
        {
            --target;
            if ((target == 0 && !isUnlockedNo0) || target < 0) target = characterCursol.Count - 1;
        }

        changeCharacterDisp();
    }

    void changeCharacterDisp(bool isInit = false)
    {
        characterCursol[0].gameObject.SetActive(isUnlockedNo0);
        characterDispImage.sprite = Messerger.instance.charactersImage[target];
        characterCursol[target].color = new Color(255f, 255f, 0f);
        if (isInit)
        {

            for(int i = isUnlockedNo0 ? 0 : 1; i < characterCursol.Count; ++i)
            {
                if(i != target) characterCursol[i].color = new Color(191f, 191f, 191f);
            }
        }
    }
}
