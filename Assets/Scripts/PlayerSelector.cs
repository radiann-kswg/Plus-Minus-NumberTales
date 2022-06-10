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

    // Start is called before the first frame update
    void Start()
    {
        changeCharacterDisp(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changeCharacter(false);

            SE.instance.PlayClip(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeCharacter(true);

            SE.instance.PlayClip(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SE.instance.PlayClip(0);

            Messerger.instance.TargetNumMessage = target;
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
