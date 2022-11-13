using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Escalador : Agent
{
    Rigidbody rigidBody;
    public GameObject target;
    public GameObject cube;
    public GameObject wall;
    public float velocityMultiplier = 50.0f;
    public float jumpForce = 10.0f;
    public bool canJump;
    public float maximaAltura = 0.01f;
    public int contador;
    public Objetivo1 objetivo1;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
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

        wall.transform.localScale = new Vector3(10, maximaAltura, 2);
        this.transform.localPosition = new Vector3(0, 0.5f, -6);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Know its position and the target's position
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(target.transform.position);

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
        if (objetivo1.touchingCube)
        {
            SetReward(1f);
            contador++;
            Debug.Log("punto Ganado" + contador);
            if (contador >= 30)
            {
                if(maximaAltura < 10)
                {
                    maximaAltura = maximaAltura + 0.05f;
                    contador = 0;
                }
            }
            objetivo1.touchingCube = false;
            EndEpisode();
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