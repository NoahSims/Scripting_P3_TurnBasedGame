using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameController : MonoBehaviour
{
    [SerializeField] private GameObject _uiController = null;
    

    private void OnEnable()
    {
        _uiController?.SetActive(true);
    }

    private void OnDisable()
    {
        if(_uiController != null)
            _uiController.SetActive(false);
    }
}
