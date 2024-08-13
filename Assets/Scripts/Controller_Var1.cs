using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;


public class Controller_Var1 : MonoBehaviour
{   
    [SerializeField] private GameObject[] players = new GameObject[4];
    [SerializeField] private Transform[] startPositions = new Transform[4];
    [SerializeField] private Transform[] endPositions = new Transform[4];
    [SerializeField] private float[] maxRotations = new float[4]{60f, 90f, 90f, 90f};
    [SerializeField] private float directionOfRotation = 1f;
    [SerializeField] private float moveSpeed = 2f; // Speed of movement
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

    //Debugging
    private float[] targetPositions;
    private float[] targetRotations;

    public void setPosition(float[] translational){
        for (int p = 0; p < players.Length; p++)
        {
            targetPositions[p] = (translational[p] + 1f) / 2f;  // Map -1 to 1 range to 0 to 1 range
        }
    }
    public void setRotation(float[] rotational){
        for (int p = 0; p < players.Length; p++)
        {
            targetRotations[p] = (rotational[p] + 1f) / 2f;  // Map -1 to 1 range to 0 to 1 range
        }
    }
    public float[] getPosition(){
        float[] positions = new float[players.Length];
        for(int p = 0; p < players.Length; p++){
            positions[p] = Mathf.InverseLerp(startPositions[p].position.z, endPositions[p].position.z, players[p].transform.position.z);
        }
        return positions;
    }
    public float[] getRotation(){
        float[] rotations = new float[players.Length];
        for(int p = 0; p < players.Length; p++){
            float currentRotation = players[p].transform.rotation.eulerAngles.z;
            if(currentRotation > maxRotations[p]){
                currentRotation = currentRotation-360f;
            }
            rotations[p] = Mathf.InverseLerp(directionOfRotation*-1f*maxRotations[p], directionOfRotation*maxRotations[p], currentRotation);
        }
        return rotations;
    }

    
    void Start()
    {
        targetPositions = new float[players.Length];
        targetRotations = new float[players.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for (int p = 0; p < players.Length; p++)
        {
            // Move towards target position
            Vector3 startPosition = startPositions[p].position;
            Vector3 endPosition = endPositions[p].position;
            float targetPos = targetPositions[p];
            Vector3 targetPosition = Vector3.Lerp(startPosition, endPosition, targetPos);
            players[p].transform.position = Vector3.MoveTowards(players[p].transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Rotate towards target rotation
            float targetRot = targetRotations[p];
            float currentRotation = players[p].transform.rotation.eulerAngles.z;
            if (currentRotation > 180f) currentRotation -= 360f; // Handle wrap-around
            float targetRotation = Mathf.Lerp(-1f * maxRotations[p], maxRotations[p], targetRot);
            float newRotation = Mathf.MoveTowardsAngle(currentRotation, directionOfRotation * targetRotation, rotationSpeed * Time.deltaTime);

            players[p].transform.rotation = Quaternion.Euler(0f, 0f, newRotation);    //players[p].transform.rotation = Quaternion.Euler(0f, 0f, directionOfRotation * newRotation);

        }
        
    }

    void FixedUpdate()
    {

    }
}
