using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 90f;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.fixedDeltaTime);
    }
}
