using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class AgenteDungeon : Agent
{
    //Agente
    Rigidbody rigidBody;
    public float velocidad = 50;
    
    //Jugador
    public GameObject jugador;
    public bool jugadorGolpeado;
    public bool jugadorDetectado;

    //Objetivos
    public Transform objetivoAgente;
    public Transform objetivoPlayer;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        objetivoAgente.GetComponent<MoverObjetivo>();
        objetivoPlayer.GetComponent<MoverObjetivo>();
    }

    private void Update()
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Know its position and the target's position
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(objetivoAgente.localPosition);
        sensor.AddObservation(jugador.transform.localPosition);
        sensor.AddObservation(jugador.GetComponent<Collision>().collider);

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

        rigidBody.AddForce(signalController * velocidad);

        // Policies
        float DistanciaObjetivo = Vector3.Distance(this.transform.localPosition, objetivoAgente.localPosition);
        float DistanciaPlayer = Vector3.Distance(this.transform.localPosition, objetivoAgente.localPosition);

        if (DistanciaObjetivo < 1.5f)
        {
            objetivoAgente.GetComponent<MoverObjetivo>().Mover();
            SetReward(1.0f);
        }
        else if (DistanciaPlayer < 1.5f )
        {
            Atacar();
        }
        else if (jugadorGolpeado)
        {
            SetReward(1f);
        }
        SetReward(-0.005f);
    }

    private void Atacar()
    {
        
    }
}
