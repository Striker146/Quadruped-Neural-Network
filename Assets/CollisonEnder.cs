using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonEnder : MonoBehaviour
{
    // Start is called before the first frame update
    QuadrupedAgent agent;
    void Start()
    {
        agent = GetComponentInParent<QuadrupedAgent>();
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "floor")
        {
            //agent.AddReward(-1f);
            agent.floorMesh.material.color = Color.red;
            agent.Finish(false);
        }
            
    }
}
