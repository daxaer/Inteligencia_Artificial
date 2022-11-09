using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Escalador : Agent
{
    Rigidbody rigidBody;
    public GameObject[] targets;
    public Transform cube;
    public Objetivo0 obj0;
    public Objetivo1 obj1;
    public Objetivo2 obj2;
    public Objetivo3 obj3;
    public Vector3 actualPos;
    public int reward = 1;
    public float velocityMultiplier = 50.0f;
    public float jumpForce = 10.0f;
    public bool canJump;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGround();
        //Debug.Log("Recompensa: " + GetCumulativeReward());
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rigidBody.angularVelocity = Vector3.zero;
            this.rigidBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, -6);
        }

        cube.transform.position = new Vector3(Random.Range(-2, 2), 1, Random.Range(-5, -1));
        actualPos = cube.transform.position;

        reward = 1;
        Debug.Log("BuscandoObjetivo");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(cube.localPosition);
        sensor.AddObservation(targets[0].transform.position);
        sensor.AddObservation(targets[1].transform.position);
        sensor.AddObservation(targets[2].transform.position);
        sensor.AddObservation(targets[3].transform.position);
        sensor.AddObservation(obj0.transform.position);
        sensor.AddObservation(obj1.transform.position);
        sensor.AddObservation(obj2.transform.position);
        sensor.AddObservation(obj3.transform.position);



        sensor.AddObservation(rigidBody.velocity.x);
        sensor.AddObservation(rigidBody.velocity.y);
        sensor.AddObservation(rigidBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 signalController = Vector3.zero;
        Vector3 signalController2 = Vector3.zero;

        // 2 Actuators
        signalController.x = actionBuffers.ContinuousActions[0];
        signalController.z = actionBuffers.ContinuousActions[1];
        signalController2.y = actionBuffers.ContinuousActions[0];

        rigidBody.AddForce(signalController * velocityMultiplier);
        

        // Policies

        if (obj0.touchingAgent0 && reward == 1)
        {
            Debug.Log("Objetivo Alcanzado: " + reward);
            SetReward(1.0f);
            reward += 1;
        }
        if (obj1.touchingCube && reward == 2)
        {
            Debug.Log("Objetivo Alcanzado: " + reward);
            SetReward(1.0f);
            reward += 1;
        }

        if (obj2.touchingAgent && reward == 3)
        {
            Debug.Log("Objetivo Alcanzado: " + reward);
            SetReward(2.0f);
            reward += 1;
        }

        if (obj3.touchingAgent2 && reward == 4)
        {
            Debug.Log("Objetivo Alcanzado:" + reward);
            SetReward(3.0f);
        }

        if (canJump)
        {
            canJump = false;
            rigidBody.AddForce(signalController2 * jumpForce, ForceMode.Impulse);
        }

        if (this.transform.localPosition.y < 0 || cube.transform.localPosition.y < 0)
        {
            SetReward(-2.0f);
            EndEpisode();
        }

        if (actualPos.z < cube.transform.position.z )
        {
            SetReward(0.1f);
            actualPos = cube.transform.position;
            Debug.Log("Gane Recompensa:");
        }

        SetReward(-0.01f);
    }

    public void CheckGround()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * 0.8f);

        if (Physics.Raycast(landingRay, out hit, 0.8f))
        {
            if (hit.collider == null)
            {
                canJump = false;
            }
            else
            {
                canJump = true;
            }
        }
    }
}
