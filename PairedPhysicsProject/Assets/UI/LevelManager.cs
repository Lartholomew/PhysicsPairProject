using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int startingLevel = 0;

    public void StartGame()
    {
        SceneManager.LoadScene(startingLevel);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
