using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    Color nonSelectionColor = new Color(255f, 255f, 255f);
    [SerializeField]
    Color selectedColor = new Color(255f, 255f, 0f);

    bool isStartingGame = false;

    [SerializeField]
    private InputAction _actionSwitchLeft, _actionSwitchRight, _actionSubmit;

    private void OnEnable()
    {
        _actionSwitchLeft.performed += OnPerformedLeft;
        _actionSwitchRight.performed += OnPerformedRight;

        _actionSwitchLeft?.Enable();
        _actionSwitchRight?.Enable();
        _actionSubmit?.Enable();
    }

    private void OnDisable()
    {
        _actionSwitchLeft.performed -= OnPerformedLeft;
        _actionSwitchRight.performed -= OnPerformedRight;

        _actionSwitchLeft?.Disable();
        _actionSwitchRight?.Disable();
        _actionSubmit?.Disable();
    }

    private float _thres = 0.7f;

    private void OnPerformedLeft(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;

        if (context.ReadValue<float>() > _thres)
        {
            changeCharacter(false);

            SE.instance.PlayClip(1);
        }
    }


    private void OnPerformedRight(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;

        if (context.ReadValue<float>() > _thres)
        {
            changeCharacter(true);

            SE.instance.PlayClip(1);
        }

    }
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

            if (_isSubmitted())
            {
                isStartingGame = true;
                SE.instance.PlayClip(0);
                Messerger.instance.TargetNumMessage = target;

                TransitionManager.instance.FadeOut();
            }
        }

        if (TransitionManager.instance.IsReadyToNextSceneNow() && isStartingGame)
        {
            SceneManager.LoadScene(mainSceneName);
        }
    }

    private bool _isSubmitted()
    {
        float _thres = 0.1f;
        return _actionSubmit.ReadValue<float>() > 1f - _thres;
    }


    void changeCharacter(bool isNext)
    {
        characterCursol[target].color = nonSelectionColor;

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
        characterCursol[target].color = selectedColor;
        if (isInit)
        {

            for(int i = isUnlockedNo0 ? 0 : 1; i < characterCursol.Count; ++i)
            {
                if (i != target) characterCursol[i].color = nonSelectionColor;
            }
        }
    }
}
