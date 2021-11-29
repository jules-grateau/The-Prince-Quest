﻿using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DraggableController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        
        Rigidbody2D _rb;
        Rigidbody2D _rbToFollow;
        float _initMass;
        float xOffset;

        public float forceMultiplier = 1f;

        // Use this for initialization
        void Awake()
        {
            _playerEventManager = PlayerEventManager.current;
            _rb = GetComponent<Rigidbody2D>();
            _initMass = _rb.mass;
            _playerEventManager.onStartInteractWith += HandleStartInteractWith;
            _playerEventManager.onStopInteractWith += HandleStopInteractWith;
        }

        private void OnDestroy()
        {
            _playerEventManager.onStartInteractWith -= HandleStartInteractWith;
            _playerEventManager.onStopInteractWith -= HandleStopInteractWith;
        }

        void HandleStartInteractWith(GameObject interactionFrom, int gameobjectId)
        {
            if(gameObject.GetInstanceID() == gameobjectId)
            {
                StartDragging(interactionFrom);
                _playerEventManager.StartDragging(gameobjectId);
            }
        }

        void HandleStopInteractWith(int gameobjectId)
        {
            if (gameObject.GetInstanceID() == gameobjectId)
            {
                StopDragging();
                _playerEventManager.StopDragging(gameobjectId);
            }
        }

        void StartDragging(GameObject interactionFrom)
        {
            _rbToFollow = interactionFrom.GetComponent<Rigidbody2D>();
            if(_rbToFollow != null)
            {
                _rb.mass = 0;
                xOffset = _rb.position.x - _rbToFollow.position.x;
            }
        }

        void StopDragging()
        {
            _rbToFollow = null;
            _rb.mass = _initMass;
        }

        private void FixedUpdate()
        {
            if(_rbToFollow != null)
            {
                _rb.MovePosition(new Vector2(_rbToFollow.position.x + xOffset, _rb.position.y));
            }
        }
    }
}