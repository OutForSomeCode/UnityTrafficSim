using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrafficManager))]
public class VehicleController : MonoBehaviour
{
    public List<WheelCollider> frontWheels;
    public List<WheelCollider> rearWheels;

    public bool reachedDestination;

    private TrafficManager TrafficManager;
    private Vector3 destination;
    private float destinationRange = 5f;
    private float strengthCoefficient = 20000f;
    private float maxSteeringAngle = 30f;
    private float throttleInput = 5f;
    private float steeringInput;

    // Start is called before the first frame update
    private void Start()
    {
        TrafficManager = GetComponent<TrafficManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var wheel in frontWheels)
        {
            wheel.steerAngle = maxSteeringAngle * steeringInput;
        }

        foreach (var wheel in rearWheels)
        {
            wheel.motorTorque = strengthCoefficient * Time.deltaTime * throttleInput;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position != destination)
        {
            // calc steering angle and throttle input
            Vector3 relativeVector = transform.InverseTransformPoint(destination);
            steeringInput = relativeVector.x / relativeVector.magnitude;

            if ((destination - transform.position).sqrMagnitude < Mathf.Pow(destinationRange, 2))
            {
                reachedDestination = true;
            }
        }
        else
        {
            reachedDestination = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }
}
