using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AccionesAgente : Agent
{
    Rigidbody rigidBody;
    public float velocityMultiplier = 50;
    public GameObject[] positions;
    bool stage1;
    bool stage2;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        stage1 = false;
        stage2 = false;
    }

    public Transform target;

    public override void OnEpisodeBegin()
    {
        // If the agent falls, its position restarts
        if (this.transform.localPosition.y < 0)
        {
            this.rigidBody.angularVelocity = Vector3.zero;
            this.rigidBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move randomly the target to somewhere else
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Know its position and the target's position
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(target.localPosition);

        // Observes its velocity in X and Y
        sensor.AddObservation(rigidBody.velocity.x);
        sensor.AddObservation(rigidBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 signalController = Vector3.zero;

        // 2 Actuators
        signalController.x = actionBuffers.ContinuousActions[0];
        signalController.z = actionBuffers.ContinuousActions[1];

        rigidBody.AddForce(signalController * velocityMultiplier);

        // Policies
        float targetDistance = Vector3.Distance(this.transform.localPosition, target.localPosition);

        if (targetDistance < 1.5f && !stage1)
        {
            stage1 = true;
            target.localPosition = positions[0].transform.localPosition;
            SetReward(1.0f);
        }
        else if (targetDistance < 1.5f && stage1 == true)
        {
            stage1 = false;
            stage2 = true;
            target.localPosition = positions[1].transform.localPosition;
            SetReward(50.0f);
        }
        else if (targetDistance < 1f && stage2 == true && stage1 == false)
        {
            target.localPosition = positions[2].transform.localPosition;
            SetReward(3.0f);
            EndEpisode();
        }

        //if (targetDistance < 3f)
        //{
        //    SetReward(10.0f);
        //}

        if (targetDistance > 3f)
        {
            SetReward(-2.0f);
        }

        if(transform.position.x <= -4.5 || transform.position.x <= 4.5 || transform.position.z <= -4.5)
        {
            SetReward(-10.0f);
        }
        
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-2.0f);
            EndEpisode();
        }

        SetReward(-0.01f);
    }
}
