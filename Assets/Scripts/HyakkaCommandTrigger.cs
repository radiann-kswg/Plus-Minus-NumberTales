using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyakkaCommandTrigger : MonoBehaviour
{
    //[SerializeField]
    List<string> hyakkaCommandButtonName = new List<string> { "Right", "Down", "Left", "Right", "Up", "Left", "Down", "Down", "Up", "Up", "Right", "Left" };
    
    private int _hyakkaCommandProgress = 0;
    private bool _isSuccessInputHyakkaCommand = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!_isSuccessInputHyakkaCommand && _hyakkaCommandProgress < hyakkaCommandButtonName.Count)
            {
                if (Input.GetButtonDown(hyakkaCommandButtonName[_hyakkaCommandProgress]))
                {
                    _hyakkaCommandProgress++;
                    if (hyakkaCommandButtonName.Count <= _hyakkaCommandProgress) _isSuccessInputHyakkaCommand = true;
                }
                else
                {
                    _ResetHyakkaCommandProgress();
                }
            }
            else if (_isSuccessInputHyakkaCommand && !Input.GetButtonDown("Submit"))
            {
                _ResetHyakkaCommandProgress();
            }
        }
    }

    private void _ResetHyakkaCommandProgress()
    {
        _isSuccessInputHyakkaCommand = false;
        _hyakkaCommandProgress = Input.GetButtonDown(hyakkaCommandButtonName[0]) ? 1 : 0;
    }

    public bool IsSuccessInput()
    {
        return _isSuccessInputHyakkaCommand;
    }
}
