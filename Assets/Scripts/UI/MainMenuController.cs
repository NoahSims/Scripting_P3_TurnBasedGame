using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        //SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }

    public void StartButton()
    {
        Debug.Log("Starting game");
        SceneManager.LoadScene("GameScene");
    }

    public void ExitButton()
    {
        Debug.Log("quiting");
        Application.Quit();
    }
}
