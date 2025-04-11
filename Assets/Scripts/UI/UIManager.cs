using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pausePannel,conformationPannel;
    public enum pannel{none,pause}
    public pannel currentPannel = pannel.none;
    public TextMeshProUGUI medKitText;
    public PlayerHealth playerHealth;

    void Start()
    {
        pausePannel.SetActive(false);
        conformationPannel.SetActive(false);
    }
    public void OnPause()
    {
        PlayButtonSound();
        Time.timeScale = 0.0f;
        pausePannel.SetActive(true); 
        currentPannel = pannel.pause;
    }
    public void OnContinue()
    {
        PlayButtonSound();
        Time.timeScale = 1.0f;
        pausePannel.SetActive(false);
        currentPannel = pannel.none;
    }

    public void OnExit()
    {
        PlayButtonSound();
        pausePannel.SetActive(false);
        ShowConformationPannel();
    }

    public void ShowConformationPannel()
    {
        conformationPannel.SetActive(true);
    }

    public void YesButton()
    {
        conformationPannel.SetActive(false);
        SceneManager.LoadScene("Home");
        PlayButtonSound();
    }

    public void NoButton()
    {
        conformationPannel.SetActive(false);
        PlayButtonSound();
        if(currentPannel == pannel.none)
        {
            return;
        }
        else if(currentPannel == pannel.pause)
        {
            pausePannel.SetActive(true);
        }
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(0);
    }

    public void medKitCount()
    {
        medKitText.text = playerHealth.medkitCount.ToString();
    }
}
