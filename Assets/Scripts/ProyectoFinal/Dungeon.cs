using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class Dungeon : Agent
{
    public float velocidad;
    public float random;
    private int objetivosAlcanzados;
    private Rigidbody rb;
    public GameObject objetivo;
    public GameObject player;
    public GameObject espada;

    public override void OnEpisodeBegin()
    {
        objetivosAlcanzados = 0;
        objetivo.GetComponent<MoverObjetivo>().Mover();
        rb = this.GetComponent<Rigidbody>();
        MoverSpawn();
    }

    public void MoverSpawn()
    {
        random = UnityEngine.Random.Range(-4, 4);
        transform.localPosition = new Vector3(random, 0.2f, random);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Distance(player.transform.localPosition, transform.localPosition));
        sensor.AddObservation((player.transform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(Vector3.Distance(objetivo.transform.localPosition, transform.localPosition));
        sensor.AddObservation((objetivo.transform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(Vector3.Distance(espada.transform.localPosition, transform.localPosition));
        sensor.AddObservation((espada.transform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(transform.forward);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 signalController = Vector3.zero;

        signalController.x = actionBuffers.ContinuousActions[0];
        signalController.z = actionBuffers.ContinuousActions[1];

        rb.AddForce(signalController * velocidad);
        if (transform.localPosition.y < -1)
        {
            EndEpisode();
        }
        SetReward(-0.001f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objetivo"))
        {
            SetReward(0.2f);
           
            other.gameObject.GetComponent<MoverObjetivo>().Mover();
        }
        if (other.CompareTag("Player"))
        {
            SetReward(2);
            objetivosAlcanzados++;
            if (objetivosAlcanzados == 10)
            {
                EndEpisode();
            }
            other.GetComponent<Player>().MoverSpawn();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pared"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public void restarPuntos()
    {
        SetReward(-0.1f);
    }
}
