using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HyakkaCommandTrigger : MonoBehaviour
{
    [SerializeField]
    private InputAction _action;

    //[SerializeField]
    List<Vector2> hyakkaCommandButtonName = new List<Vector2> { Vector2.right, Vector2.down, Vector2.left, Vector2.right, Vector2.up, Vector2.left, Vector2.down, Vector2.down, Vector2.up, Vector2.up, Vector2.right, Vector2.left };
    
    private int _hyakkaCommandProgress = 0;
    private bool _isSuccessInputHyakkaCommand = false;

    private void OnEnable()
    {
        _action.performed += OnPerformedCommand;

        _action?.Enable();
    }

    private void OnDisable()
    {
        _action.performed -= OnPerformedCommand;

        _action?.Disable();
    }


    private void OnPerformedCommand(InputAction.CallbackContext context)
    {
        if (TransitionManager.instance.IsTransitionAnimating())
            return;


        // キーを離した時も performed が値 (0,0) で来て進捗がリセットされるため、離しは判定しない
        if (context.ReadValue<Vector2>().sqrMagnitude < 0.01f)
            return;

        if (!_isSuccessInputHyakkaCommand && _hyakkaCommandProgress < hyakkaCommandButtonName.Count)
        {
            if (_GudgeHyakkaCommand(_hyakkaCommandProgress))
            {
                _hyakkaCommandProgress++;
                if (hyakkaCommandButtonName.Count <= _hyakkaCommandProgress) _isSuccessInputHyakkaCommand = true;
            }
            else
            {
                _ResetHyakkaCommandProgress();
            }
            return;
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

    private bool _GudgeHyakkaCommand(int index)
    {
        float _thres = 0.1f;
        return Vector2.Distance(_action.ReadValue<Vector2>(), hyakkaCommandButtonName[index]) < _thres;
    }


    private void _ResetHyakkaCommandProgress()
    {
        _isSuccessInputHyakkaCommand = false;
        _hyakkaCommandProgress = _GudgeHyakkaCommand(0) ? 1 : 0;
    }

    public bool IsSuccessInput()
    {
        return _isSuccessInputHyakkaCommand;
    }
}
