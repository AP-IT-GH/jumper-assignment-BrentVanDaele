using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObstacleX : MonoBehaviour
    {
        [SerializeField] float obstacleSpeedMin = 0.001f;
        [SerializeField] float obstacleSpeedMax = 0.5f;
        private float obstacleSpeed;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            obstacleSpeed = Random.Range(obstacleSpeedMin, obstacleSpeedMax);
            this.transform.localPosition = new Vector3(-3.0f, 0.5f, 0f);
            this.transform.localScale = new Vector3(1, 1, 8);
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.localPosition += Vector3.right * obstacleSpeed;

            if (this.transform.localPosition.x >= 5.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}