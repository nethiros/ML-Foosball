using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BoundaryCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ScoreSystem _ScoreSystem;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

GameObject ball;

    void Reset()
    {
        _ScoreSystem.resetBall(ball);
    }


    public void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Ball"){
            Debug.Log("Ball left play-area");

            //_ScoreSystem.resetBall(other.gameObject);
            ball = other.gameObject;
            Invoke("Reset", 1.5f);
        }
    }
}
