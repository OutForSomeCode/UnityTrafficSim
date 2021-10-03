using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways()]
public class Suspension : MonoBehaviour
{
    public bool setSuspensionDistance = true;

    [Range(0, 20)]
    public float naturalFrequency = 10;
    [Range(0, 3)]
    public float dampingRatio = 0.8f;
    [Range(-1, 1)]
    public float forceShift = 0.03f;

    void Update()
    {
        foreach (WheelCollider wheel in GetComponentsInChildren<WheelCollider>())
        {
            JointSpring spring = wheel.suspensionSpring;

            spring.spring = Mathf.Pow(Mathf.Sqrt(wheel.sprungMass) * naturalFrequency, 2);
            spring.damper = 2 * dampingRatio * Mathf.Sqrt(spring.spring * wheel.sprungMass);

            wheel.suspensionSpring = spring;

            Vector3 wheelRelativeBody = transform.InverseTransformPoint(wheel.transform.position);
            float distance = GetComponent<Rigidbody>().centerOfMass.y - wheelRelativeBody.y + wheel.radius;

            wheel.forceAppPointDistance = distance - forceShift;

            if (spring.targetPosition > 0 && setSuspensionDistance)
                wheel.suspensionDistance = wheel.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);

            UpdateVisualWheel(wheel.gameObject);
        }
    }

    private void UpdateVisualWheel(GameObject wheel)
    {
        if (wheel.transform.childCount == 0)
            return;

        Transform visualWheel = wheel.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        wheel.GetComponent<WheelCollider>().GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
