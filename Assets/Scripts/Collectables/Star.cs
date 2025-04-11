using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    CollectableController collectableController;
    [SerializeField] ParticleSystem starEffect;
    public float rotationSpeed = 10f;

    void Start()
    {
        collectableController = FindObjectOfType<CollectableController>();

        if (collectableController == null)
        {
            Debug.LogError("CollectableController not found in the scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (starEffect != null)
            {
                ParticleSystem effectInstance = Instantiate(starEffect, transform.position, Quaternion.identity);
                effectInstance.Play();
                Destroy(effectInstance.gameObject, effectInstance.main.duration);
            }

            if (collectableController != null)
            {
                collectableController.CollectStar();
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
