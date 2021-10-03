using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrafficManager))]
public class VehicleController : MonoBehaviour
{
    public List<GameObject> frontWheels;
    public List<GameObject> rearWheels;

    [Range(-1, 1)]
    public float throttleInput = 1f;
    [Range(50, 1000)]
    public float horsePower = 100f;
    [Range(0, 50)]
    public float steeringAngle = 35f;

    public bool reachedDestination;

    private TrafficManager TrafficManager;
    private Vector3 destination;
    private float destinationRange = 5f;
    private float steeringInput;

    // Start is called before the first frame update
    private void Start()
    {
        TrafficManager = GetComponent<TrafficManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        
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
                throttleInput = 0f;
            }
        }
        else
        {
            reachedDestination = true;
            throttleInput = 0f;
        }

        foreach (var wheel in frontWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = steeringAngle * steeringInput;
        }

        foreach (var wheel in rearWheels)
        {
            wheel.GetComponent<WheelCollider>().motorTorque = (horsePower * 1000) * Time.deltaTime * throttleInput;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
        throttleInput = 1f;
    }
}
