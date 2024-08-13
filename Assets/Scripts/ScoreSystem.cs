using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private TextMeshPro textObject_red;
    [SerializeField] private TextMeshPro textObject_blue;
    [SerializeField] private Transform einwurfRot;
    [SerializeField] private Transform einwurfBlau;
    [SerializeField] private KickerAgent kickerAgentRed;
    [SerializeField] private KickerAgent kickerAgentBlue;
    private Rigidbody ballRigidbody;    //Debug
    [SerializeField] private Transform ballTransform;   //Debug
    private float scale = 0.001f;
    public int score_blue = 0;
    public int score_red = 0;
    private int m_ResetTimer;
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    //Reset the Ball
    public void resetBall(GameObject toReset){
        Rigidbody rb = toReset.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        float velX = Random.Range(10f,20f)*scale;
        float velY = Random.Range(-2f,2f)*scale;
        float velZ = Random.Range(50f,600f)*scale;
        //Ball coming from left
        if(Random.Range(0f,1f)>0.5f){
            toReset.transform.position = einwurfBlau.position;
            rb.velocity = new Vector3(velX, velY, velZ);
        }
        //Ball coming from right
        else{
            toReset.transform.position = einwurfRot.position;
            rb.velocity = new Vector3(-velX, velY, -velZ);
        }
    }

    //Update amount of goals (red team scored)
    public void updateRed(GameObject coll){
        score_red++;
        textObject_red.text = "Tore: " + score_red;
        textObject_blue.text = "Tore: " + score_blue;
        resetBall(coll);
    }
    //Update amount of goals (blue team scored)
    public void updateBlue(GameObject coll){
        score_blue++;
        textObject_red.text = "Tore: " + score_red;
        textObject_blue.text = "Tore: " + score_blue;
        resetBall(coll);
    }
    void Start()
    {
        textObject_red.text = "Tore: " + score_red;
        textObject_blue.text = "Tore: " + score_blue;
        resetBall(ball);
        ballRigidbody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();     //Debug
    }

    void FixedUpdate(){

        m_ResetTimer += 1;
        //Ball is standing still for too long, reset the episode
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_ResetTimer = 0;
            kickerAgentRed.EndEpisode();
            kickerAgentBlue.EndEpisode();
            resetBall(ball);
        }
    }

    //Update is called once per frame
    void Update()
    {
        //Reset the ball
        if(Input.GetKeyDown(KeyCode.R)){
            resetBall(ball);

        }
        
        if(Input.GetKeyDown(KeyCode.B)){    //Debug
            Vector3 ballPose = ballTransform.localPosition;
            float ballPoseCorrect1 = Mathf.InverseLerp(-1320f, -160f, ballPose[0]);
            float ballPoseCorrect3 = Mathf.InverseLerp(-690f, -50f, ballPose[2]);
            Debug.Log(ballPoseCorrect1);
            Debug.Log(ballPose[1]);
            Debug.Log(ballPoseCorrect3);

        }

        if(Input.GetKeyDown(KeyCode.V)){    //Debug
            ballRigidbody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
            Vector3 vel = ballRigidbody.velocity;
            Debug.Log(vel[0]);
            Debug.Log(vel[1]);
            Debug.Log(vel[2]);

        }

        if(Input.GetKeyDown(KeyCode.T)){ //BallPosTest
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            float velX = Random.Range(0f,6000f)*scale;
            float velY = Random.Range(0f,6000f)*scale;
            float velZ = Random.Range(0f,6000f)*scale;
            ball.transform.position = Vector3.Lerp(einwurfBlau.position,einwurfRot.position,0.5f);
            rb.velocity = new Vector3(velX, velY, velZ);
        }
    }

}
