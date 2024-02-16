using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringRate : MonoBehaviour
{
    public float SpringValue = 50f;
    public float maxForce = 100;
    public float damperValue = 0f;
    ConfigurableJoint[] configurableJoints;
    
    // Start is called before the first frame update
    void Start()
    {
        configurableJoints = GetComponentsInChildren<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ConfigurableJoint configurableJoint in configurableJoints)
        {
            JointDrive jointDriver = new JointDrive();
            jointDriver.positionSpring = SpringValue;
            jointDriver.maximumForce = maxForce;
            jointDriver.positionDamper = damperValue;

            configurableJoint.angularXDrive = jointDriver;
            configurableJoint.angularYZDrive = jointDriver;

        }
    }
}
