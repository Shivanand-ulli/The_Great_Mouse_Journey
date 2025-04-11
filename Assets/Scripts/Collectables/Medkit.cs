using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medkit : MonoBehaviour
{
    public static Medkit activeMedkit = null;  // Only one active chest at a time

    public PlayerHealth playerHealth;
    public GameObject chestPanel;
    public Button pickupButton;
    public GameObject[] medKits;

    private int medkitsRemaining;

    void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found!");
        }

        pickupButton.onClick.AddListener(PickUpMedkits);
        chestPanel.SetActive(false);
        medkitsRemaining = medKits.Length;
    }

    public bool HasMedkits()
    {
        return medkitsRemaining > 0;
    }

    public void ShowMedkitPanel()
    {
        if (HasMedkits())
        {
            // Hide UI for any previously active chest
            if (activeMedkit != null && activeMedkit != this)
            {
                activeMedkit.HideMedkitPanel();
            }

            activeMedkit = this; // Set this chest as active
            chestPanel.SetActive(true);
        }
    }

    public void HideMedkitPanel()
    {
        chestPanel.SetActive(false);
        if (activeMedkit == this)
        {
            activeMedkit = null; // Reset active chest when UI is hidden
        }
    }

    public void PickUpMedkits()
    {
        if (activeMedkit != this) return; // Prevent multiple chests from triggering at once

        if (playerHealth.medkitCount < 3)
        {
            foreach (GameObject medKit in medKits)
            {
                if (medKit.activeSelf)
                {
                    playerHealth.IncreaseMedkitCount();
                    Debug.Log("Medkit picked up! Player's current medkit count: " + playerHealth.medkitCount);

                    medKit.SetActive(false);
                    medkitsRemaining--;
                    break; // Ensure only one medkit is picked per button press
                }
            }

            if (medkitsRemaining == 0)
            {
                HideMedkitPanel();
            }
        }
        else
        {
            Debug.Log("You already have 3 medkits!");
        }
    }
}

