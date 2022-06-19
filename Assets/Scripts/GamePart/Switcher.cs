using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    [SerializeField]
    GameObject leftSwitch, rightSwitch;

    public bool IsRight = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        leftSwitch.SetActive(!IsRight);
        rightSwitch.SetActive(IsRight);
    }

    public void Switching(bool isRight_new)
    {
        IsRight = isRight_new;
    }
}
