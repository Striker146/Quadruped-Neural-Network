using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Unity.VisualScripting;
using Unity.MLAgents.Integrations.Match3;
using TMPro;



public class QuadrupedAgent : Agent
{
     Rigidbody rBody;
     EnviromentReset enviromentReset;
    public Transform target;
    public int MaxSteps = 3000;
    public int currentStep;
    public float bearing;
    public float distance;
    public GameObject floor;
    public MeshRenderer floorMesh;
    public float EpisodeDistance;
    public TextMeshPro scoreDisplay;
    public TextMeshPro lastScoreDisplay;
    float oldTargetDistance;
    float startingTargetDistance;
    public float maxLivingPunishment;
    public LegInfo[] legs;

    void Start()
    {
        enviromentReset = GetComponentInParent<EnviromentReset>();
        scoreDisplay = enviromentReset.scoreDisplay;
        lastScoreDisplay = enviromentReset.lastScoreDisplay;
        rBody = GetComponent<Rigidbody>();
        floorMesh = floor.GetComponent<MeshRenderer>();
    }

    public void Reset() {
        Destroy(transform.GetComponent<TrailRenderer>());
        enviromentReset.Reset();
    }


    public void OnDrawGizmosSelected() {
        Gizmos.DrawSphere(floor.transform.position + new Vector3(0,1f,0),0.5f);
    }

    public override void OnEpisodeBegin()
    {
        float tarDistance = 10f;
        float angle = Random.Range(0f, 360f);
        Vector3 position = new Vector3(tarDistance * Unity.Mathematics.math.cos(angle), 0.5f, tarDistance * Unity.Mathematics.math.sin(angle));
        target.localPosition = position;

        EpisodeDistance = Vector3.Distance(target.localPosition, this.transform.localPosition);
        oldTargetDistance = Vector3.Distance(transform.localPosition, target.localPosition);
    }

    private IEnumerator DestroyHoldJointAfterDelay(FixedJoint holdJoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (holdJoint != null)
        {
            Destroy(holdJoint);
            SetReward(0f);
            floorMesh.material.color = Color.grey;
            foreach (LegInfo leg in legs)
            {
                leg.AllowStickyFoot(true);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 directionToTarget = target.position - this.transform.position;
        bearing = Vector3.SignedAngle(this.transform.forward, directionToTarget, Vector3.up) / 180;
        distance = Vector3.Distance(transform.position, target.position);
        sensor.AddObservation(bearing);
        sensor.AddObservation(distance);
        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(transform.localPosition);

        foreach (LegInfo leg in legs){
            leg.addObservations(sensor); 
        }

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);
        sensor.AddObservation(rBody.velocity.z);
    }

    public void Finish(bool taskCompleted){
        if (!taskCompleted)
        {
            float remainingPunishment = -(maxLivingPunishment * (MaxSteps - StepCount + 1))/MaxSteps;
            AddReward(remainingPunishment);
        }
        float score = GetCumulativeReward();
        lastScoreDisplay.text = $"Last score: {Mathf.Round(score * 100f) / 100f}";
        EndEpisode();
        Reset();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int inputsPerLeg = 5;
        for (int i = 0; i < 4; i++){
            List<float> legSignalList = new List<float>();
            for (int j = 0; j < inputsPerLeg; j++)
            {
                legSignalList.Add(actions.ContinuousActions[i*inputsPerLeg + j]);
            }
            legs[i].ApplySignal(legSignalList);
        }



        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log(distanceToTarget);
        }
        if (distanceToTarget < 1.6f)
        {
            Debug.Log("Task Achieved");
            AddReward(10f);
            Finish(true);
            floorMesh.material.color = Color.green;
        }

        if (transform.localPosition.y < -2)
        {
            Finish(false);
        }

        if (StepCount > MaxSteps && MaxSteps != 0){
            floorMesh.material.color = Color.blue;
            Finish(false);
        }
        else {
            currentStep = StepCount;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    public void FixedUpdate()
    {

        float punishScore = -maxLivingPunishment / MaxSteps;
        AddReward(punishScore);

        float targetDistance = Vector3.Distance(transform.localPosition, target.localPosition);
        float reward = (-targetDistance + oldTargetDistance) / 2;
        AddReward(reward);
        oldTargetDistance = targetDistance;
        float score = GetCumulativeReward();
        scoreDisplay.text = $"Current score: {Mathf.Round(score * 100f) / 100f}";
    }
}