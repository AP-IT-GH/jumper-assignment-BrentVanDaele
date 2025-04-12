using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace Assets.Scripts
{
    public class CubeAgent : Agent
    {
        [SerializeField] float jumpForce = 7f;
        [SerializeField] GameObject obstaclePrefab;
        private Rigidbody rb;
        private GameObject obstacle = null;

        public override void OnEpisodeBegin()
        {
            obstacle = Instantiate(obstaclePrefab);
            rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero; // Reset velocity to prevent weird physics
            transform.localPosition = new Vector3(0, 0.6f, 0); // Reset position
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(rb.linearVelocity.y);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float jumpAction = actions.ContinuousActions[0];

            if (IsGrounded() && jumpAction > 0)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // Calculate the distance from the obstacle
                float distanceToObstacle = Mathf.Abs(obstacle.transform.position.x - transform.position.x);

                // Reward for jumping close to the obstacle, penalize for jumping far
                if (distanceToObstacle < 2f)
                {
                    SetReward(0.5f); // Positive reward for jumping close to the obstacle
                }
                else
                {
                    SetReward(-0.2f); // Smaller penalty for jumping too far
                }
            }

            // If the obstacle is destroyed, reward the agent for completing the task
            if (obstacle == null)
            {
                SetReward(2.0f);
                EndEpisode();
            }

            // If the agent falls down, penalize it
            if (transform.localPosition.y <= -0.5f)
            {
                SetReward(-1.0f);
                Destroy(obstacle);
                EndEpisode();
            }
        }



        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);

            if (collision.gameObject.CompareTag("obstacle"))
            {
                Debug.Log("Collided with an obstacle! Ending episode.");
                SetReward(-1.0f);
                Destroy(obstacle);
                EndEpisode();
            }
        }


        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                continuousActionsOut[0] = 1f;
            }
        }


        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 1.1f);
        }
    }
}
