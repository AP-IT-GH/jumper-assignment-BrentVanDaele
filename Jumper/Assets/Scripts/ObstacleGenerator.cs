using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField] GameObject obstaclePrefab;
        private GameObject obstacle;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            obstacle = Instantiate(obstaclePrefab);
        }

        // Update is called once per frame
        void Update()
        {
            if (obstacle == null)
            {
                obstacle = Instantiate(obstaclePrefab);
            }
        }
    }
}