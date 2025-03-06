using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CarEnter : MonoBehaviour
{
    public GameObject player;
    public GameObject car;
    public CinemachineFreeLook playerCamera;
    public CinemachineFreeLook carCamera;
    public Button enterCarButton;
    public Button exitCarButton;
    public BoxCollider carBoxCollider;

    private bool isNearCar = false;
    private bool isDriving = false;
    WheelController wheelController;

    void Start()
    {
        enterCarButton.gameObject.SetActive(false);
        exitCarButton.gameObject.SetActive(false);
        wheelController = GetComponent<WheelController>();
        wheelController.enabled = false;

        playerCamera.gameObject.SetActive(true);
        carCamera.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNearCar = true;
            enterCarButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNearCar = false;
            enterCarButton.gameObject.SetActive(false);
        }
    }

    public void EnterCar()
    {
        if (!isDriving)
        {
            isDriving = true;
            player.SetActive(false);

            wheelController.enabled = true;

            playerCamera.gameObject.SetActive(false);
            carCamera.gameObject.SetActive(true);

            enterCarButton.gameObject.SetActive(false);
            exitCarButton.gameObject.SetActive(true);

            player.transform.SetParent(car.transform);

            wheelController.acceleration = 5000;
        }
    }

    public void ExitCar()
    {
        if (isDriving)
        {
            isDriving = false;

            player.transform.SetParent(null);
            player.SetActive(true);


            playerCamera.gameObject.SetActive(true);
            carCamera.gameObject.SetActive(false);

            
            player.transform.position = car.transform.position + Vector3.right * 2f;

            exitCarButton.gameObject.SetActive(false);

            if (isNearCar)
            {
                enterCarButton.gameObject.SetActive(true);
            }

            wheelController.breakingForce = 20000f;
            wheelController.acceleration = 0f;

            wheelController.FR.brakeTorque = 10000f;
            wheelController.FL.brakeTorque = 10000f;
            wheelController.BR.brakeTorque = 10000f;
            wheelController.BL.brakeTorque = 10000f;
            
        }
    }
}
