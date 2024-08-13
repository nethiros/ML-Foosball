using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorCollider : MonoBehaviour
{
    [SerializeField] private ScoreSystem _ScoreSystem;
    [SerializeField] private KickerAgent kickerAgentRed;
    [SerializeField] private KickerAgent kickerAgentBlue;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Ball"){
            //red team scored
            if(kickerAgentRed.team == 'r'){
                _ScoreSystem.updateRed(other.gameObject);
                kickerAgentRed.AddReward(2.0f);
                kickerAgentBlue.AddReward(-2.0f);
                //end the episode
                kickerAgentRed.EndEpisode();
                kickerAgentBlue.EndEpisode();
            }
            if(kickerAgentBlue.team == 'b'){
                //blue team scored
                _ScoreSystem.updateBlue(other.gameObject);
                kickerAgentRed.AddReward(-2.0f);
                kickerAgentBlue.AddReward(2.0f);
                //end the episode
                kickerAgentRed.EndEpisode();
                kickerAgentBlue.EndEpisode();
            }
        }
    }
}
