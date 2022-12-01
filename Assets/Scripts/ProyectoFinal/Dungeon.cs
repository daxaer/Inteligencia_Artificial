using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class Dungeon : Agent
{
    public float velocidad;
    private Rigidbody rb;
    public bool jugadorDetectado;
    public GameObject player;
    public GameObject objetivo;
    public GameObject Espada;
    public Transform posicionInicial;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        jugadorDetectado = false;
        this.transform.localPosition = posicionInicial.transform.localPosition;
        player.GetComponent<Player>().MoverSpawn();
        objetivo.GetComponent<MoverObjetivo>().Mover();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(jugadorDetectado);
        sensor.AddObservation(Vector3.Distance(objetivo.transform.localPosition, transform.localPosition));
        sensor.AddObservation(Vector3.Distance(player.transform.localPosition, transform.localPosition));
        sensor.AddObservation(player);
        sensor.AddObservation(objetivo);
        //sensor.AddObservation(Vector3.Distance(Espada.transform.localPosition, transform.localPosition));
        sensor.AddObservation(transform.forward);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 signalController = Vector3.zero;

        signalController.x = actionBuffers.ContinuousActions[0];
        signalController.z = actionBuffers.ContinuousActions[1];

        rb.AddForce(signalController * velocidad);

        float distanciaPlayer = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

        if(distanciaPlayer < 2)
        {
            jugadorDetectado = true;
        }
        if(GetCumulativeReward() < -10)
        {
            EndEpisode();
        }
        if (GetCumulativeReward() > 3)
        {
            EndEpisode();
        }
        AddReward(-0.001f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Objetivo") && !jugadorDetectado)
        {
            Debug.Log("objetivo inpactado");
            AddReward(1f);
            collision.gameObject.GetComponent<MoverObjetivo>().Mover();
        }
        if (collision.transform.CompareTag("Player") && jugadorDetectado)
        {
            Debug.Log("jugador inpactado");
            AddReward(3f);
            collision.gameObject.GetComponent<Player>().MoverSpawn();
        }
        if(collision.transform.CompareTag("Espada"))
        {
            Debug.Log("agente inpactado");
            AddReward(-1);
            EndEpisode();
        }
    }
}
