using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _buttonSound = null;

    public void StartButton()
    {
        PlayButtonSound();
        Debug.Log("Starting game");
        SceneManager.LoadScene("GameScene");
    }

    public void ExitButton()
    {
        PlayButtonSound();
        Debug.Log("quiting");
        Application.Quit();
    }

    private void PlayButtonSound()
    {
        if (_buttonSound != null)
        {
            AudioHelper.PlayClip2D(_buttonSound, 1f);
        }
    }
}
