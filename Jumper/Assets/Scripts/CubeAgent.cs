using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace Assets.Scripts
{
    public class CubeAgent : Agent
    {
        [SerializeField] float jumpForce = 15f;
        private Rigidbody rb;
        [SerializeField] GameObject[] obstaclePrefabs;
        private GameObject obstacle;

        public override void OnEpisodeBegin()
        {
            obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
            rb = this.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero; // Reset velocity to prevent weird physics
            rb.angularVelocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.6f, 0); // Reset position
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward);

        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.localPosition);
            sensor.AddObservation(rb.linearVelocity.y);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float jumpAction = actions.ContinuousActions[0];

            RotateCorrectly();

            if (IsGrounded() && jumpAction > 0.0f)
            {
                rb.AddForce(Vector3.up * jumpAction, ForceMode.Impulse);

                // get distance via raycasting
                Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hitFront, 10f);

                if (hitFront.distance < 1.5f)
                {
                    SetReward(0.5f); // Reward for correctly jumping close to an obstacle
                }
                else
                {
                    SetReward(-0.4f); // Punish to much jumping
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
            // check if the agent is at the ground again, but only the ground/floor
            return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f) && hit.collider.CompareTag("floor");
        }

        private  void RotateCorrectly()
        {
            // Check for obstacles using raycasts
            bool seesObstacleFront = Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hitFront, 10f)
                                  && hitFront.collider.CompareTag("obstacle");

            bool seesObstacleRight = Physics.Raycast(this.transform.position, this.transform.right, out RaycastHit hitRight, 10f)
                                  && hitRight.collider.CompareTag("obstacle");

            bool seesObstacleLeft = Physics.Raycast(this.transform.position, -this.transform.right, out RaycastHit hitLeft, 10f)
                                  && hitLeft.collider.CompareTag("obstacle");


            // Rotate toward the correct direction to jump
            if (!seesObstacleFront && !seesObstacleLeft && seesObstacleRight)
            {
                // rotate right if an object is right
                this.transform.rotation = Quaternion.LookRotation(this.transform.right);
            }
            else if (!seesObstacleFront && seesObstacleLeft && !seesObstacleRight)
            {
                // rotate left if an object is left
                this.transform.rotation = Quaternion.LookRotation(-this.transform.right);
            }
            else if ((seesObstacleFront && !seesObstacleRight && !seesObstacleLeft) || (seesObstacleLeft && seesObstacleRight && !seesObstacleFront))
            {
                // don't rotate when an object is at front or of there are object left and right
                this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
            }
            else
            {
                // rotate 180 degrees if there is no object seen
                this.transform.rotation = Quaternion.LookRotation(-this.transform.forward);
            }
        }
    }
}
