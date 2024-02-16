using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegController : MonoBehaviour
{
    public ConfigurableJoint hipJoint;
    public ConfigurableJoint kneeJoint;
    public float hipAngle = 45;
    public float kneeAngle = 45;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion hipAngleQ = Quaternion.Euler(hipAngle, 0,0);
        hipJoint.targetRotation = hipAngleQ;

        Quaternion kneeAngleQ = Quaternion.Euler(kneeAngle, 0,0);
        kneeJoint.targetRotation = kneeAngleQ;
    }
}
