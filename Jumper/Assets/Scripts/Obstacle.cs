using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] float obstacleSpeed = 1.0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
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