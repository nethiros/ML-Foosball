    using System.Collections;
    using UnityEngine;

    namespace TableFootball
    {
        public class ShotRewardHandler : MonoBehaviour
        {
            [SerializeField] private KickerAgent agent;  // Referenz zum Agenten

            [SerializeField] private GameObject ball;

            [SerializeField] private float rewardMultiplier = 0.02f;  // Multiplicator for the reward

            [SerializeField] private float penaltyMultiplier = 0.015f;  // Multiplicator for the penalty

            private bool hasCollided;

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject == ball.gameObject && !hasCollided)
                {
                    hasCollided = true;
                    StartCoroutine(HandleShotReward());
                }
            }

            private IEnumerator HandleShotReward()
            {
                // Initial Ball velocity
                float initialVelocityX = ball.GetComponent<Rigidbody>().velocity.x;

                // Wait for 100ms
                yield return new WaitForSeconds(0.1f);

                // Velocity of the ball after 100ms
                float finalVelocityX = ball.GetComponent<Rigidbody>().velocity.x;

                // Reward or penalty
                if (agent.team == 'r')  // red team
                {
                    if (finalVelocityX > initialVelocityX)
                    {
                        agent.AddReward(-penaltyMultiplier * (finalVelocityX - initialVelocityX));  // Penalty
                    }
                    else
                    {
                        agent.AddReward(rewardMultiplier * (initialVelocityX - finalVelocityX));  // Reward
                    }
                }
                else if (agent.team == 'b')  // blue team
                {
                    if (finalVelocityX < initialVelocityX)
                    {
                        agent.AddReward(-penaltyMultiplier * (initialVelocityX - finalVelocityX));  // Penalty
                    }
                    else
                    {
                        agent.AddReward(rewardMultiplier * (finalVelocityX - initialVelocityX));  // Reward
                    }
                }

                // Reset for next collision
                hasCollided = false;
            }
        }
    }
