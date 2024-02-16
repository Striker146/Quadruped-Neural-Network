using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class FootData : MonoBehaviour
{
    LegInfo legInfo;
    Rigidbody rBody;
    bool footCollison;
    public FixedJoint stickyJoint;
    public bool stick;
    public bool allowStick = true;
    void Start()
    {
        legInfo = GetComponentInParent<LegInfo>();
        rBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "floor")
        {
            footCollison = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "floor")
        {
            footCollison = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (stickyJoint == null && stick && allowStick)
        {
            stickyJoint = this.AddComponent<FixedJoint>();
            if (collision.rigidbody != null)
            {
                stickyJoint.connectedBody = collision.rigidbody;
            }
            
        }
        if (stickyJoint != null && (!stick || !allowStick))
        {
            Destroy(stickyJoint);
        }
    }

    public void addObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(rBody.velocity);
        sensor.AddObservation(rBody.angularVelocity);

        sensor.AddObservation(footCollison);
        sensor.AddObservation(stick);
    }
}
