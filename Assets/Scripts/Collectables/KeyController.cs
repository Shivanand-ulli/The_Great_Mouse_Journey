using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyController: MonoBehaviour
{
    private float keyCount = 0;
    public Button keyButton;
    public TextMeshProUGUI keyCountTxt;
    
    void Start()
    {
        UpdateKeyUI();
        // keyButton.onClick.AddListener(UseKey);
    }

    public void AddKey()
    {
        keyCount++;
        UpdateKeyUI();
    }

    public bool HasKey()
    {
        return keyCount > 0;
    }

    public void UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            UpdateKeyUI();
        }
        else
        {
            Debug.Log("No keys left!"); // Optional: Show UI message
        }
    }

    public void UpdateKeyUI()
    {
        if(keyCountTxt != null)
        {
            keyCountTxt.text = keyCount.ToString();
        }
    }
}
