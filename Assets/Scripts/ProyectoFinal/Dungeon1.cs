using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class Dungeon1 : Agent
{
    //Agente
    public int vida = 100;
    public int vidaActual;
    public float velocidad = 50;
    Rigidbody rigidBody;

    //ataque
    public float tiempoParaAtacar = 50;
    public float tiempoParaActacarActual;
    public bool puedoAtacar;
    public GameObject puntoAtaque;

    //Jugador
    public Collider espada;
    public GameObject jugador;
    public bool jugadorDetectado;
    public Transform posicionInicial;

    //ganar puntos
    public bool sumarPuntos;
    public bool restarPuntos;
    public bool muerto;

    //Objetivos
    public GameObject objetivoAgente;

    void Start()
    {
        vidaActual = vida;
        tiempoParaActacarActual = tiempoParaAtacar;
        rigidBody = GetComponent<Rigidbody>();
        jugadorDetectado = false;
    }

    public override void OnEpisodeBegin()
    {
        objetivoAgente.GetComponent<MoverObjetivo>().Mover();
        puedoAtacar = true;
        transform.position = posicionInicial.position;
        objetivoAgente.SetActive(true);
        jugadorDetectado = false;
        vidaActual = vida;
        tiempoParaActacarActual = tiempoParaAtacar;
        sumarPuntos = false;
        muerto = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Know its position and the target's position
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(objetivoAgente.transform.localPosition);
        sensor.AddObservation(jugador.transform.localPosition);
        sensor.AddObservation(espada);
        sensor.AddObservation(puntoAtaque);

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
        float DistanciaObjetivo = Vector3.Distance(this.transform.localPosition, objetivoAgente.transform.localPosition);
        float DistanciaPlayer = Vector3.Distance(this.transform.localPosition, jugador.transform.localPosition);

        if (DistanciaObjetivo < 1.5f && !jugadorDetectado)
        {
            objetivoAgente.GetComponent<MoverObjetivo>().Mover();
            SetReward(1.0f);
        }
        else if (DistanciaPlayer < .5f && puedoAtacar && jugadorDetectado)
        {
          
        }
        else if(sumarPuntos)
        {
            SetReward(1f);
            sumarPuntos = false;
            Debug.Log("Restando Puntos" + GetCumulativeReward());
        }
        if(restarPuntos)
        {
            SetReward(-1f);
            restarPuntos = false;
            Debug.Log("Sumando Puntos" + GetCumulativeReward());
        }
        if(DistanciaPlayer < 1 && !jugadorDetectado)
        {
            jugadorDetectado = true;
            objetivoAgente.SetActive(false);
            SetReward(1f);
            Debug.Log("Puntuacion actual" + GetCumulativeReward());
        }
        if(muerto)
        {
            SetReward(-1);
            EndEpisode();
        }
        if(transform.position.y <=-1)
        {
            SetReward(-1);
            EndEpisode();
        }
        SetReward(-0.005f);
    }

    private void Atacar()
    {
        Debug.Log("Ataque");
        //puntoAtaque.SetActive(true);
        puedoAtacar = false;
        tiempoParaActacarActual = tiempoParaAtacar;
    }

    public void RecibiendoDaño(int daño)
    {
        vidaActual -= daño;
        Debug.Log("Me atacaron vida restante" + vidaActual);
        RestarPuntos();
        if (vidaActual <= 0)
        {
            muerto = true;
        }
    }
    public void RestarPuntos()
    {
        restarPuntos = true;
    }
    public void SumarPuntos()
    {
        sumarPuntos = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Espada"))
        {
            RecibiendoDaño(1);
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("dañando al jugador");
        }
    }

    private void FixedUpdate()
    {
        if(!puedoAtacar)
        {
            //puntoAtaque.SetActive(false);
            tiempoParaActacarActual--;
            if(tiempoParaActacarActual <= 0)
            {
                puedoAtacar = true;
            }
        }
    }
}
