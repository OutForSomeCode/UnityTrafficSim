using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerController : MonoBehaviour
{
    public GameObject pullingVehicle;

    private bool isReversing;

    void Update()
    {
        isReversing = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z < 0f;

        TrailerBrakes(pullingVehicle.GetComponent<VehicleController>().throttleInput);
    }

    //helping the trailer to get going and braking
    private void TrailerBrakes(float direction)
    {
        foreach(WheelCollider wheel in GetComponentsInChildren<WheelCollider>())
        {
            // is pulling vehicle going forward?
            if (direction > 0)
            {
                // is trailer reversing?
                if (isReversing)
                {
                    wheel.motorTorque = 0f;
                }
                else
                {
                    wheel.motorTorque = 10f;
                    wheel.brakeTorque = 0f;
                }
            }
            // is pulling vehicle going backwards
            else if (direction < 0)
            {
                // is trailer reversing?
                if (isReversing)
                {
                    wheel.motorTorque = -10;
                    wheel.brakeTorque = 0f;
                }
                else
                    wheel.motorTorque = 0;
            }
            else
                wheel.brakeTorque = 10;
        }
    }
}
