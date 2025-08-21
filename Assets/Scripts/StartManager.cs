using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChanger : MonoBehaviour
{
    public void LoadCreationScene()
    {
        SceneManager.LoadScene("CreationScene");
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadMazeRushScene()
    {
        SceneManager.LoadScene("MazeRushScene");
    }

    public void LoadLocalMazeRushScene()
    {
        SceneManager.LoadScene("LocalRushScene");
    }

    public void LoadOnlineMazeRushScene()
    {
        SceneManager.LoadScene("OnlineRushScene");
    }
}
