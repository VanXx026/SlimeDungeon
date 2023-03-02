using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("Game");
        GameManager.instance.isGameOver = false;
        ScreenCanvasManager.instance.OpenPlayerStatesPanel();
        ScreenCanvasManager.instance.OpenUIButtionPanel();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
