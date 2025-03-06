using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField]public  WheelCollider FR;
    [SerializeField]public WheelCollider FL;
    [SerializeField]public  WheelCollider BR;
    [SerializeField]public  WheelCollider BL;

    [SerializeField] Transform FRTransform;
    [SerializeField] Transform FLTransform;
    [SerializeField] Transform BRTransform;
    [SerializeField] Transform BLTransform;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput != 0)
        {
            currentAcceleration = acceleration * verticalInput;
        }
        else
        {
            currentAcceleration = 0f; 
        }

        
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakingForce = Mathf.Lerp(currentBreakingForce, breakingForce, Time.deltaTime * 5f);
            currentAcceleration = 0f; 
        }
        else
        {
            currentBreakingForce = 0f;
        }

        BR.motorTorque = currentAcceleration;
        BL.motorTorque = currentAcceleration;

        // Apply brake torque to ALL wheels
        FR.brakeTorque = currentBreakingForce;
        FL.brakeTorque = currentBreakingForce;
        BR.brakeTorque = currentBreakingForce;
        BL.brakeTorque = currentBreakingForce;

        // Steering (apply only to front wheels)
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        FR.steerAngle = currentTurnAngle;
        FL.steerAngle = currentTurnAngle;

        UpdateWheel(FR, FRTransform);
        UpdateWheel(FL, FLTransform);
        UpdateWheel(BR, BRTransform);
        UpdateWheel(BL, BLTransform);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }
}
