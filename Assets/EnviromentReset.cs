using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class EnviromentReset : MonoBehaviour
{

    public GameObject quadruped;

    public void Reset(){
        Instantiate(quadruped, new Vector3(0,2,0), quaternion.Euler(new Vector3(0,0,0)), this.transform);
    }
}
