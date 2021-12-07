using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Camera
{
    public class AutoScrollingCameraController : MonoBehaviour
    {
        public Vector2[] targets;
        public float speed = 0.5f;

        public Edge[] edges;
        public Edge[] edgesDisabledOnReachedEnd;

        Vector3 _currentTarget;
        int _currentIndex = 0;

        Dictionary<Edge,EdgeCollider2D> _edgeColliders;
        
        bool _isEventStarted = false;
        
        LevelEventManager _levelEventManager;
        PlayerEventManager _playerEventManager;

        InGameEventType gameEvent = InGameEventType.AutoScrollingStart;

        private void Awake()
        {
            _levelEventManager = LevelEventManager.current;
            _levelEventManager.onStartGameEvent += HandleStartGameEvent;
            CalculateCurrentTarget();

            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerDie += HandlePlayerDie;
        }

        private void CalculateEdges()
        {
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            if (cam == null && edges.Length > 0)
                return;

            _edgeColliders = new Dictionary<Edge, EdgeCollider2D>();

            var bottomLeft = (Vector2)(cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)) - transform.position);
            var topLeft = (Vector2)(cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane)) - transform.position);
            var topRight = (Vector2)(cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane)) - transform.position);
            var bottomRight = (Vector2)(cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane)) - transform.position);

            foreach (Edge edge in edges)
            {
                Vector2[] points;
                switch (edge)
                {
                    case Edge.Right:
                        points = new Vector2[] { topRight, bottomRight };
                        break;
                    case Edge.Left:
                        points = new Vector2[] { topLeft, bottomLeft };
                        break;
                    case Edge.Bottom:
                        points = new Vector2[] { bottomLeft, bottomRight };
                        break;
                    case Edge.Top:
                        points = new Vector2[] { topLeft, topRight };
                        break;
                    default:
                        points = null;
                        break;
                }

                EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
                if (edgeCollider != null)
                {
                    edgeCollider.points = points;
                    _edgeColliders.Add(edge, edgeCollider);
                }

            }
        }

        private void OnDestroy()
        {
            _levelEventManager.onStartGameEvent -= HandleStartGameEvent;
            _playerEventManager.onPlayerDie -= HandlePlayerDie;
        }

        void HandlePlayerDie()
        {
            _isEventStarted = false;
        }

        void HandleStartGameEvent(InGameEventType gameEventType)
        {
            if (gameEventType == gameEvent)
            {
                _isEventStarted = true;
                CalculateEdges();
            }
        }

        void OnReachedEndScolling()
        {

            _isEventStarted = false;
            foreach(Edge edge in edgesDisabledOnReachedEnd)
            {
                EdgeCollider2D edgeCollider;
                if(_edgeColliders.TryGetValue(edge, out edgeCollider))
                {
                    edgeCollider.enabled = false;
                }
                
            }
        }

        void CalculateCurrentTarget()
        {
            Vector2 target = targets[_currentIndex];
            _currentTarget = new Vector3(target.x, target.y, transform.position.z);
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (!_isEventStarted)
                return;

            float step = speed * Time.deltaTime;
            GetComponent<Rigidbody2D>().position = Vector3.MoveTowards(transform.position, _currentTarget, step);
            //transform.position = Vector3.MoveTowards(transform.position, _currentTarget, step);

            if (transform.position == _currentTarget)
            {
                _currentIndex++;

                if (_currentIndex < targets.Length)
                {
                    CalculateCurrentTarget();

                }
                else
                {
                    OnReachedEndScolling();
                }
            }
        }
    }
}