using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LegInfo : MonoBehaviour
{
    [SerializeField] private LegData thigh;
    [SerializeField] private LegData leg;
    [SerializeField] private FootData foot;
    [SerializeField] private bool FootCollison;



    public void ApplySignal(List<float> signals){
        Quaternion hipAngleQ = Quaternion.Euler(signals[0] * 360f, signals[1] * 360f,0);
        thigh.joint.targetRotation = hipAngleQ;

        Quaternion kneeAngleQ = Quaternion.Euler(signals[2] * 360f, signals[3] * 360f,0);
        leg.joint.targetRotation = kneeAngleQ;

        //foot.stick = signals[4] > 0;
    }

    public void addObservations(VectorSensor sensor)
    {
        thigh.addObservations(sensor);
        leg.addObservations(sensor);
        foot.addObservations(sensor);
    }

    public void AllowStickyFoot(bool sticky)
    {
        foot.allowStick = sticky;
        if (!sticky && foot.stickyJoint != null)
        {
            Debug.Log("Destroying!");
            Destroy(foot.stickyJoint);
        }
    }
}
