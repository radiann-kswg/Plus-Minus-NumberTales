using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switching : MonoBehaviour
{
    [SerializeField]
    GameObject leftSwitch, rightSwitch;

    public bool isRight = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        leftSwitch.SetActive(!isRight);
        rightSwitch.SetActive(isRight);
    }

    public void Switching(bool isRight0)
    {
        isRight = isRight0;
    }
}
