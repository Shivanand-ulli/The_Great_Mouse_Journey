using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI starText;
    [SerializeField] GameObject gameOver;
    int starPoints = 0;

    void Start()
    {
        starText.text = "0";
    }

    public void CollectStar()
    {
        starPoints++;
        starText.text = starPoints.ToString();
        AudioManager.instance.PlaySFX(1);

        if(starPoints == 60)
        {
            StartCoroutine(GameOver());
        }
    }


    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlaySFX(2);
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
}
