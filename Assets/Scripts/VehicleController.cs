using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrafficManager))]
public class VehicleController : MonoBehaviour
{
    public List<GameObject> frontWheels;
    public List<GameObject> rearWheels;
    public GameObject trailer;

    [Range(50f, 1000f)]
    public float horsePower = 100;
    [Range(0f, 50f)]
    public float steeringAngle = 35;
    public float currentSpeed = 0;

    public bool reachedDestination;

    private TrafficManager TrafficManager;
    private Vector3 destination;
    private float destinationRange, wheelBase, axleWidth, steeringInput, radius, mass, maxSpeed, upcommingTurn;
    private List<float> transmition, shiftSpeeds;
    public int currentGear = 0; //public for testing!!

    private void Start()
    {
        SetStartingValues();
        SetGearboxValues();
    }

    private void FixedUpdate()
    {
        UpdateSteering();
        UpdateGearbox();
        UpdateTorque();
        UpdateBreaking();
    }

    private void SetStartingValues()
    {
        Rigidbody vehicle = GetComponent<Rigidbody>();

        destinationRange = 5f;
        TrafficManager = GetComponent<TrafficManager>();
        mass = vehicle.mass;

        vehicle.drag = 0.05f + (mass - 1000) / 200000;
        vehicle.angularDrag = vehicle.drag;

        if (trailer != null)
            mass += trailer.GetComponent<Rigidbody>().mass;

        Vector3 frontAxel = new Vector3();
        Vector3 rearAxel = new Vector3();
        
        foreach (var wheel in frontWheels)
            frontAxel += wheel.GetComponent<WheelCollider>().transform.position;

        foreach (var wheel in rearWheels)
            rearAxel += wheel.GetComponent<WheelCollider>().transform.position;

        wheelBase = Vector3.Distance(frontAxel / frontWheels.Count, rearAxel / rearWheels.Count);
        radius = Mathf.Tan((180 - (steeringAngle + 90)) * Mathf.Deg2Rad) * wheelBase;
        axleWidth = Vector3.Distance(frontWheels[0].transform.position, frontWheels[1].transform.position);
    }

    private void SetGearboxValues()
    {
        shiftSpeeds = new List<float> { 10, 25, 45, 65, 85 };
        transmition = new List<float> { 6f, 3.5f, 2f, 1f, 1.2f, 1.3f };

        float percentage = (60f / 1000f * horsePower) - 30f;

        for (int i = 0; i < transmition.Count; i++)
            transmition[i] = transmition[i] / 100f * (i < 3 ? 100f - percentage : 100f + percentage);

    }

    private void UpdateSteering()
    {
        if (transform.position != destination)
        {
            // calc steering angle
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

        float left, right;

        if (steeringInput > 0)
        {
            left = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (radius + axleWidth / 2)) * steeringInput;
            right = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (radius - axleWidth / 2)) * steeringInput;
        }
        else if (steeringInput < 0)
        {
            left = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (radius - axleWidth / 2)) * steeringInput;
            right = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (radius + axleWidth / 2)) * steeringInput;
        }
        else
        {
            left = 0;
            right = 0;
        }

        frontWheels[0].GetComponent<WheelCollider>().steerAngle = left;
        frontWheels[1].GetComponent<WheelCollider>().steerAngle = right;
    }

    private void UpdateGearbox()
    {
        if (currentGear < 5)
            currentGear = currentSpeed < shiftSpeeds[currentGear] ? currentGear : currentGear + 1;

        if (currentGear > 0)
            currentGear = currentSpeed < shiftSpeeds[currentGear - 1] ? currentGear - 1 : currentGear;
    }

    private void UpdateTorque()
    {
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        float torque = currentGear < 3 ? mass / transmition[currentGear] : mass * transmition[currentGear];

        foreach (var wheel in frontWheels)
        {
            wheel.GetComponent<WheelCollider>().motorTorque = currentSpeed < maxSpeed ? torque : 0;
            wheel.GetComponent<WheelCollider>().brakeTorque = 0;
        }

        foreach (var wheel in rearWheels)
        {
            wheel.GetComponent<WheelCollider>().brakeTorque = 0;
        }
    }

    private void UpdateBreaking()
    {
        float turningSpeed = 20 + (90 - upcommingTurn);
        float breakingDistance = currentSpeed + (mass / horsePower * (currentSpeed / 100));

        if ((destination - transform.position).sqrMagnitude < Mathf.Pow(breakingDistance, 2))
        {
            if (currentSpeed > turningSpeed)
            {
                maxSpeed = turningSpeed;
                foreach (var wheel in frontWheels)
                    wheel.GetComponent<WheelCollider>().motorTorque = -1000;

                foreach (var wheel in rearWheels)
                    wheel.GetComponent<WheelCollider>().motorTorque = 0;
            }
        } 
    }

    public void SetDestination(Vector3 destination, float allowedSpeed, float turnAngle)
    {
        this.destination = destination;
        maxSpeed = allowedSpeed;
        upcommingTurn = turnAngle;
        reachedDestination = false;
    }
}
