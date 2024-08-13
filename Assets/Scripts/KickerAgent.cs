using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class KickerAgent : Agent
{
    [SerializeField] private Controller_Var1 controller;
    [SerializeField] private Controller_Var1 controllerEnemy;
    private Rigidbody ballRigidbody;
    [SerializeField] public char team = 'r';
    [SerializeField] private Transform ballTransform;

    public override void Initialize()
    {
        ballRigidbody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the environment for a new episode
 
        controller.setPosition(new float[4] { 0f, 0f, 0f, 0f });
        controller.setRotation(new float[4] { 0f, 0f, 0f, 0f });
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Add player's position and rotation to observations
        sensor.AddObservation(controller.getPosition());
        sensor.AddObservation(controller.getRotation());
        sensor.AddObservation(controllerEnemy.getPosition());

        //Add ball's position and velocity to observations
        Vector3 ballPose = ballTransform.localPosition;
        float ballPoseCorrect1 = Mathf.InverseLerp(-1320f, -160f, ballPose[0]);
        float ballPoseCorrect3 = Mathf.InverseLerp(-690f, -50f, ballPose[2]);
        Vector3 vel = ballRigidbody.velocity;

        //Team blue
        if(team == 'b'){
            sensor.AddObservation(ballPoseCorrect1);
            sensor.AddObservation(ballPoseCorrect3);
            sensor.AddObservation(vel[0]);
            sensor.AddObservation(vel[2]);
        }
        //team red
        else if(team == 'r'){
            sensor.AddObservation(1-ballPoseCorrect1);
            sensor.AddObservation(1-ballPoseCorrect3);
            sensor.AddObservation(-1*vel[0]);
            sensor.AddObservation(-1*vel[2]);
        }
    }

    //Continious Input
    public override void OnActionReceived(ActionBuffers actions)
    {
        float[] translational = new float[4];
        float[] rotational = new float[4];

        for (int i = 0; i < 4; i++)
        {
            translational[i] = actions.ContinuousActions[i];
            rotational[i] = actions.ContinuousActions[i + 4];
        }
        controller.setPosition(new float[4]{translational[0], translational[1], translational[2], translational[3]});
        controller.setRotation(new float[4]{rotational[0], rotational[1], rotational[2], rotational[3]});

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;

        // Beispiel für einfache kontinuierliche Steuerung
        // Translational input
        continuousActions[0] = Input.GetAxis("Vertical");   // Vorwärts/Rückwärts Bewegung für Stange 1
        continuousActions[1] = Input.GetAxis("Horizontal"); // Links/Rechts Bewegung für Stange 2
        continuousActions[2] = Input.GetAxis("Vertical");   // Vorwärts/Rückwärts Bewegung für Stange 3
        continuousActions[3] = Input.GetAxis("Horizontal"); // Links/Rechts Bewegung für Stange 4

        // Rotational input
        continuousActions[4] = Input.GetKey(KeyCode.Q) ? 1f : 0f; // Rotationsbewegung für Stange 1
        continuousActions[5] = Input.GetKey(KeyCode.W) ? 1f : 0f; // Rotationsbewegung für Stange 2
        continuousActions[6] = Input.GetKey(KeyCode.E) ? 1f : 0f; // Rotationsbewegung für Stange 3
        continuousActions[7] = Input.GetKey(KeyCode.R) ? 1f : 0f; // Rotationsbewegung für Stange 4
    }
}

