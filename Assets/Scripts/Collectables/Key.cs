using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Key: MonoBehaviour
{
    public KeyController keyController;

    void Start()
    {
        keyController = FindAnyObjectByType<KeyController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            keyController.AddKey();
            gameObject.SetActive(false);
        } 
    }
}
