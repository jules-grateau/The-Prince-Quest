using Assets.Scripts.Manager.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyTargetTrackingController : MonoBehaviour
    {
        public float detectionRadius = 1f;
        public float backDetectionRadiusDivider = 2f;
        public LayerMask targetLayer;
        public LayerMask blockSightLayer;

        GameObject _eyes;
        const string EyesGameObjectName = "Eyes";

        // Use this for initialization
        void Start()
        {
            _eyes = transform.Find(EyesGameObjectName)?.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            TargetDetection();
        }

        void TargetDetection()
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer);
            bool seePlayer = collider != null;

            Color rayCastColor = seePlayer ? Color.green : Color.red;
            Debug.DrawRay(transform.position, Vector2.right * detectionRadius, rayCastColor);
            Debug.DrawRay(transform.position, Vector2.left * detectionRadius, rayCastColor);
            Debug.DrawRay(transform.position, Vector2.up * detectionRadius, rayCastColor);
            Debug.DrawRay(transform.position, Vector2.down * detectionRadius, rayCastColor);

            if (collider != null)
            {
                Vector2 targetPosition = collider.gameObject.transform.position;

                Vector2 currLookingDirection = new Vector2(_eyes.transform.position.x - transform.position.x, 0).normalized;
                Vector2 targetDirection = new Vector2(targetPosition.x - transform.position.x, 0).normalized;

                if(backDetectionRadiusDivider != 0 && currLookingDirection != targetDirection && Vector2.Distance(transform.position, targetPosition) > detectionRadius/ backDetectionRadiusDivider)
                {
                    return;
                }

                RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, blockSightLayer);
                if (hit.collider == null)
                {
                    Debug.DrawLine(transform.position, targetPosition, Color.yellow);
                    EnemyEventManager.current.EnemyFindTarget(gameObject.GetInstanceID(),
                    collider.transform.position);
                }
            }
        }
    }
}