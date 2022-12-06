using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class Player : Agent
{
    public float velocidad;
    public float random;
    private int objetivosAlcanzados;
    private Rigidbody rb;
    public GameObject objetivo;
    public GameObject objetivo2;
    public GameObject agente;

    public override void OnEpisodeBegin()
    {
        objetivosAlcanzados = 0;
        objetivo.GetComponent<MoverObjetivo>().Mover();
        objetivo2.GetComponent<MoverObjetivo>().Mover();
        rb = this.GetComponent<Rigidbody>();
        MoverSpawn();
    }

    public void MoverSpawn()
    {
        random = UnityEngine.Random.Range(-4, 4);
        transform.localPosition = new Vector3(random, 0.2f, random);
    }

    public void RestarPuntos()
    {
        SetReward(-0.5f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Distance(objetivo2.transform.localPosition, transform.localPosition));
        sensor.AddObservation(Vector3.Distance(objetivo.transform.localPosition, transform.localPosition));
        sensor.AddObservation((objetivo2.transform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation((objetivo.transform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(Vector3.Distance(agente.transform.localPosition, transform.localPosition));
        sensor.AddObservation((agente.transform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(transform.forward);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 signalController = Vector3.zero;

        signalController.x = actionBuffers.ContinuousActions[0];
        signalController.z = actionBuffers.ContinuousActions[1];

        rb.AddForce(signalController * velocidad);

        float distanciaAgente = Vector3.Distance(agente.transform.localPosition, transform.localPosition);

        if (transform.localPosition.y < -1)
        {
            EndEpisode();
        }
        if(distanciaAgente < .3f)
        {
            SetReward(-0.001f);
        }
        SetReward(-0.001f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Objetivo"))
        {
            SetReward(2);
            objetivosAlcanzados++;
            if(objetivosAlcanzados == 10)
            {
                EndEpisode();
            }
            other.gameObject.GetComponent<MoverObjetivo>().Mover();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Pared"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}
