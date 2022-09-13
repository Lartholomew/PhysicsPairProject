using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int targetLevel = 0;

    public void ChangeScene()
    {
        SceneManager.LoadScene(targetLevel);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
