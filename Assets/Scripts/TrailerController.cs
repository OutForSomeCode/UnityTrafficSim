using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerController : MonoBehaviour
{
    public GameObject pullingVehicle;

    public void Start()
    {
        Rigidbody trailer = GetComponent<Rigidbody>();

        float mass = trailer.mass;

        trailer.drag = 0.05f + (mass - 1000) / 200000;
        trailer.angularDrag = trailer.drag;
    }

    //helping the trailer to get going and braking
    private void FixedUpdate()
    {
        foreach(WheelCollider wheel in GetComponentsInChildren<WheelCollider>())
        {
            float direction = pullingVehicle.GetComponent<VehicleController>().currentSpeed;

            if (direction > 0 && direction < 1)
            {
                wheel.motorTorque = 10;
                wheel.brakeTorque = 0;
            }
            else if (direction > -1 && direction < 0)
            {
                wheel.motorTorque = -10;
                wheel.brakeTorque = 0;
            }
            else
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = 0;
            }
        }
    }
}
