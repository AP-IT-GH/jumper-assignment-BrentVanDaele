using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObstacleZ : MonoBehaviour
    {
        [SerializeField] float obstacleSpeedMin = 0.1f;
        [SerializeField] float obstacleSpeedMax = 1.5f;
        private float obstacleSpeed;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            obstacleSpeed = Random.Range(obstacleSpeedMin, obstacleSpeedMax);
            this.transform.localPosition = new Vector3(0.0f, 0.5f, -3.0f);
            this.transform.localScale = new Vector3(8, 1, 1);
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.localPosition += Vector3.forward * obstacleSpeed;

            if (this.transform.localPosition.z >= 5.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}