using UnityEngine;

namespace Assets.Scripts.Animation.Objects
{
    public class FloatingObject : MonoBehaviour
    {
        public Vector3 topOffsetBoundary;
        public Vector3 bottomOffsetBoundary;
        public float speed = 0.005f;

        private Vector3 _topBoundary;
        private Vector3 _bottomBoundary;
        private Vector3 _direction;

        private void Start()
        {
            _topBoundary = transform.position + topOffsetBoundary;
            _bottomBoundary = transform.position + bottomOffsetBoundary;
            _direction = _topBoundary;
        }
        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _direction, speed);

            if (transform.position == _direction)
            {
                if (_direction == _topBoundary)
                {
                    _direction = _bottomBoundary;
                }
                else
                {
                    _direction = _topBoundary;
                }
            }


        }
    }
}