using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;
    public int medkitCount = 0;
    public UIManager uIManager;
    public Button restoreHealthButton; 
    [SerializeField] GameObject gameOver;

    private PlayerController playerController;
    private Coroutine healthDecreaseCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        playerController = GetComponent<PlayerController>();

        restoreHealthButton.onClick.AddListener(RestoreHealth);
        gameOver.SetActive(false);
    }

    void Update()
    {
        if (currentHealth > 0)
        {
            if (playerController.IsRunning())
            {
                StartHealthDecrease(2f);  
            }
            else if (playerController.IsWalking())
            {
                StartHealthDecrease(1f);  
            }
            else
            {
                StopHealthDecrease();
            }
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }

        // Disable the button if no medkits or full health
        restoreHealthButton.interactable = (medkitCount > 0 && currentHealth < maxHealth);
    }

    void StartHealthDecrease(float decreaseRate)
    {
        if (healthDecreaseCoroutine == null)
        {
            healthDecreaseCoroutine = StartCoroutine(DecreaseHealthRoutine(decreaseRate));
        }
    }

    void StopHealthDecrease()
    {
        if (healthDecreaseCoroutine != null)
        {
            StopCoroutine(healthDecreaseCoroutine);
            healthDecreaseCoroutine = null;
        }
    }

    IEnumerator DecreaseHealthRoutine(float decreaseRate)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / decreaseRate);
            currentHealth = Mathf.Max(0, currentHealth - 1f);
            StartCoroutine(SmoothHealthBarFill(currentHealth));

            if (currentHealth <= 0)
            {
                GameOver();
                yield break; // Stop the coroutine
            }
        }
    }

    void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void IncreaseHealth(float amount)
    {
        float targetHealth = Mathf.Min(currentHealth + amount, maxHealth);
        StartCoroutine(SmoothHealthBarFill(targetHealth));
    }

    public void IncreaseMedkitCount()
    {
        medkitCount++;
        uIManager.medKitCount();
    }

    public void RestoreHealth()
    {
        if (medkitCount > 0 && currentHealth < maxHealth) 
        {
            float restoreAmount = maxHealth * 0.5f; // Restore 50% of max health
            medkitCount--; // Consume one medkit
            uIManager.medKitCount(); // Update UI
            StartCoroutine(HealOverTime(restoreAmount, 1.5f)); // Heal gradually over 1.5 sec
        }
        else
        {
            Debug.Log("Cannot restore health! Either full health or no medkits left.");
        }
    }

    IEnumerator HealOverTime(float amount, float duration)
    {
        float startHealth = currentHealth;
        float targetHealth = Mathf.Min(currentHealth + amount, maxHealth);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentHealth = Mathf.Lerp(startHealth, targetHealth, elapsedTime / duration);
            healthBar.value = currentHealth;
            yield return null;
        }

        currentHealth = targetHealth; // Ensure exact final value
        healthBar.value = currentHealth;
    }

    // Smoothly fill health bar
    IEnumerator SmoothHealthBarFill(float targetHealth)
    {
        float startHealth = healthBar.value;
        float elapsedTime = 0f;
        float duration = 0.5f; // Half a second for a smooth transition

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthBar.value = Mathf.Lerp(startHealth, targetHealth, elapsedTime / duration);
            yield return null;
        }

        healthBar.value = targetHealth;
        currentHealth = targetHealth;
    }
}
