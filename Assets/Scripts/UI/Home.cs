using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void GameSceen()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1.0f;
        PlaySFX();
    }

    public void PlaySFX()
    {
        AudioManager.instance.PlaySFX(0);
    }
}
