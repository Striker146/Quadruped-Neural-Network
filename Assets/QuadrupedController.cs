using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupedController : MonoBehaviour
{
    LegController[] legControllers;
    public float hipAngle;
    [ReadOnlyInspector]
    public float kneeAngle;
    // Start is called before the first frame update
    void Start()
    {
        legControllers = GetComponentsInChildren<LegController>();
    }

    // Update is called once per frame
    void Update()
    {
        kneeAngle = - 2 * hipAngle;
        foreach (LegController legController in legControllers){
            legController.hipAngle = hipAngle;
            legController.kneeAngle = kneeAngle;
        }
    }
}
