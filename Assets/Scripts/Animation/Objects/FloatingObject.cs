using UnityEngine;

namespace Assets.Scripts.Animation.Objects
{
    public class FloatingObject : MonoBehaviour
    {
        public Vector3 topOffsetBoundary;
        public Vector3 bottomOffsetBoundary;
        public float speed = 0.005f;

        private Vector3 topBoundary;
        private Vector3 bottomBoundary;
        private Vector3 direction;
        private void Start()
        {
            topBoundary = transform.position + topOffsetBoundary;
            bottomBoundary = transform.position + bottomOffsetBoundary;
            direction = topBoundary;
        }
        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, direction, speed);

            if (transform.position == direction)
            {
                if (direction == topBoundary)
                {
                    direction = bottomBoundary;
                }
                else
                {
                    direction = topBoundary;
                }
            }


        }
    }
}