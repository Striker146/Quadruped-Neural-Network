using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class LegData : MonoBehaviour
{
    public Rigidbody rBody;
    public ConfigurableJoint joint;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
    }

    public void addObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(rBody.velocity);
        sensor.AddObservation(rBody.angularVelocity);

        sensor.AddObservation(joint.targetRotation);
        sensor.AddObservation(joint.currentForce);
        sensor.AddObservation(joint.currentTorque);
    }
}
