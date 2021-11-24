using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyPlayerTrackingController : MonoBehaviour
    {

        public float lookDistance = 10f;
        public float lookHeight = 0.5f;

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
            LookForPlayer();
        }

        void LookForPlayer()
        {
            float xOffsett = _eyes.transform.position.x - transform.position.x;

            Vector2[] lookDirections = new Vector2[]
            {
                new Vector2(xOffsett, 0).normalized,
                new Vector2(xOffsett, lookHeight).normalized,
                new Vector2(xOffsett, -lookHeight).normalized,

            };

            foreach(Vector2 lookDirection in lookDirections)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(_eyes.transform.position, lookDirection, lookDistance);
                bool seePlayer = raycastHit.collider != null && raycastHit.collider.gameObject.CompareTag("Player");
                Debug.DrawRay(_eyes.transform.position, lookDirection * lookDistance, seePlayer ? Color.green : Color.red);
            }
        }

    }
}