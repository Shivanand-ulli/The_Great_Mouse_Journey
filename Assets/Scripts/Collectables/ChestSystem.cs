using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestSystem : MonoBehaviour
{
    public static ChestSystem currentChest = null;  // Track only one active chest

    public KeyController keyController;
    public Animator animator;
    public Medkit medkit; 
    public Button openChestButton;

    private bool isPlayerNearBy = false;
    private bool isChestOpened = false;

    void Start()
    {
        keyController = FindAnyObjectByType<KeyController>(); 
        animator = GetComponent<Animator>();  
        medkit = GetComponent<Medkit>(); 

        // Ensure only one button press triggers chest opening
        openChestButton.onClick.AddListener(() => OpenNearestChest());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearBy = true;

            // Ensure only the closest chest is active
            if (currentChest == null || Vector3.Distance(other.transform.position, transform.position) < 
                Vector3.Distance(other.transform.position, currentChest.transform.position))
            {
                currentChest = this;
            }
        }        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearBy = false;
            animator.SetBool("isOpen", false);
            isChestOpened = false;
            medkit.HideMedkitPanel();

            // Reset if the player moves away
            if (currentChest == this)
            {
                currentChest = null;
            }
        }  
    }

    public void TryOpenChest()
    {
        if (isPlayerNearBy && keyController.HasKey() && !isChestOpened)
        {
            keyController.UseKey();
            animator.SetBool("isOpen", true);
            isChestOpened = true;

            if (medkit.HasMedkits())
            {
                medkit.ShowMedkitPanel();
            }
        }
    }

    // Open only the nearest chest
    public static void OpenNearestChest()
    {
        if (currentChest != null)
        {
            currentChest.TryOpenChest();
        }
    }
}

