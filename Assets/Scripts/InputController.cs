using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public event Action PressedMouse = delegate { };

    private void Update()
    {
        DetectMouse();
    }

    private void DetectMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PressedMouse?.Invoke();
        }
    }
}
