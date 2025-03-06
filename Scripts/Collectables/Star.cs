using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    CollectableController collectableController;

    void Start()
    {
        collectableController = FindObjectOfType<CollectableController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            collectableController.CollectStar();  
        }
    }
}
