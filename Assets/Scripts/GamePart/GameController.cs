using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameDirector director;
    [SerializeField]
    Switcher sw;
    [SerializeField]
    Velt vl;

    [SerializeField]
    private InputAction _actionSwitchLeft, _actionSwitchRight, _actionHDrop, _actionHold;

    private void OnEnable()
    {
        _actionSwitchLeft.performed += OnPerformedLeft;
        _actionSwitchRight.performed += OnPerformedRight;
        _actionHDrop.performed += OnPerformedHDrop;
        _actionHold.performed += OnPerformedHold;

        _actionSwitchLeft?.Enable();
        _actionSwitchRight?.Enable();
        _actionHDrop?.Enable();
        _actionHold?.Enable();
    }

    private void OnDisable()
    {
        _actionSwitchLeft.performed -= OnPerformedLeft;
        _actionSwitchRight.performed -= OnPerformedRight;
        _actionHDrop.performed -= OnPerformedHDrop;
        _actionHold.performed -= OnPerformedHold;

        _actionSwitchLeft?.Disable();
        _actionSwitchRight?.Disable();
        _actionHDrop?.Disable();
        _actionHold?.Disable();
    }

    private float _thres = 0.7f;

    private void OnPerformedLeft(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;

       if(context.ReadValue<float>() > _thres)
        {
            sw.Switching(false);

            SE.instance.PlayClip(1);
        }
    }


    private void OnPerformedRight(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;

        if (context.ReadValue<float>() > _thres)
        {
            sw.Switching(true);

            SE.instance.PlayClip(1);
        }

    }
    private void OnPerformedHDrop(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;

        if (context.ReadValue<float>() > _thres)
        {
            vl.HardDrop();

            SE.instance.PlayClip(1);
        }

    }
    private void OnPerformedHold(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;

        if (context.ReadValue<float>() > _thres)
        {
            director.HoldNum();

            SE.instance.PlayClip(1);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
