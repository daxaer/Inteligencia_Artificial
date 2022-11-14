using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Escalador : Agent
{
    Rigidbody rigidBody;
    Rigidbody rCube;
    public GameObject cube;
    public GameObject wall;
    public float velocityMultiplier = 50.0f;
    public float jumpForce = 10.0f;
    public bool canJump;
    public Objetivo1 obj1;
    bool puntocubo;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rCube = cube.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGround();
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
        rCube.constraints = RigidbodyConstraints.None;
        rCube.constraints = RigidbodyConstraints.FreezeRotation;

        this.transform.localPosition = new Vector3(0, 0.5f, -6);
        puntocubo = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Know its position and the target's position
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(cube.transform.localPosition) ;
        sensor.AddObservation(obj1);

        // Observes its velocity in X and Y
        sensor.AddObservation(rigidBody.velocity.x);
        sensor.AddObservation(rigidBody.velocity.z);

        //jump
        sensor.AddObservation(rigidBody.velocity.y);
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

        if (canJump)
        {
            canJump = false;
            rigidBody.AddForce(signalController2 * jumpForce, ForceMode.Impulse);
        }


        if (obj1.touchingAgent && puntocubo == true)
        {
            SetReward(1f);
            obj1.touchingAgent = false;
            Debug.Log("punto 1");
            EndEpisode();
        }
        else if(obj1.touchingAgent && puntocubo == false)
        {
            obj1.touchingAgent = false;
            SetReward(-1f);
            EndEpisode();
        }

        if (cube.transform.position.z > 0 && puntocubo == false)
        {
            puntocubo = true;
            SetReward(1f);
            rCube.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("punto cubo");
        }
        SetReward(-0.005f);
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-2.0f);
            EndEpisode();
        }
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