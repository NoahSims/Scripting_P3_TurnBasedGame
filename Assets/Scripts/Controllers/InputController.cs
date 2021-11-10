using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public static InputController Current;

    public event Action<int> PressedMouse = delegate { };
    private Vector3 _mouseWorldPosition;

    private void Awake()
    {
        Current = this;
    }

    private void Update()
    {
        DetectMouse();
    }

    public Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, 0);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            _mouseWorldPosition = ray.GetPoint(distance);
        }

        return _mouseWorldPosition;
    }

    private void DetectMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PressedMouse?.Invoke(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            PressedMouse?.Invoke(1);
        }
    }
}
