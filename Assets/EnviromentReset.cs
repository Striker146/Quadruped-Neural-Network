using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;

public class EnviromentReset : MonoBehaviour
{

    public GameObject enviromentPrefab;
    public GameObject currentEnviroment;
    public TextMeshPro scoreDisplay;
    public TextMeshPro lastScoreDisplay;

    public void Reset(){
        if (currentEnviroment != null)
        {
            Destroy(currentEnviroment);
        }
        currentEnviroment = Instantiate(enviromentPrefab, this.transform.localPosition, quaternion.Euler(new Vector3(0,0,0)), this.transform);
    }
}
