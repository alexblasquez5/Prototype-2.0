using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadSceneAsync(5);
    }

}
