using Assets.Scripts.Manager;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controllers.Environment
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class DestoryOnFall : MonoBehaviour
    {
        public LayerMask groundLayer;
        public float SpeedBreak = 3;
        public int particulPlayTime = 1;
        public int destoryTime = 2;

        new Rigidbody2D rigidbody;
        new ParticleSystem particleSystem;
        new BoxCollider2D collider;
        SpriteRenderer spriteRenderer;
        bool exploded = false;
        EventManager eventManager;
        private void Start()
        {
            eventManager = EventManager.current;
            rigidbody = GetComponent<Rigidbody2D>();
            particleSystem = GetComponent<ParticleSystem>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (exploded)
                return;

            Vector2 boxPoint = rigidbody.position + new Vector2(collider.offset.x, -collider.size.y);
            Vector2 boxSize = new Vector2((collider.size.x * 0.95f), 0.50f);
            // Collider to detected collision with ground, to check if grounded
            Collider2D[] groundOverlapBox = Physics2D.OverlapBoxAll(boxPoint
                , boxSize, transform.rotation.y, groundLayer);
            bool groundHit = groundOverlapBox != null;
            Debug.DrawRay(boxPoint, boxSize, groundHit ? Color.green : Color.red);

            groundOverlapBox.ToList().ForEach((overlapCollider) =>
            {
                if (overlapCollider.gameObject != gameObject && Mathf.Abs(rigidbody.velocity.y) > SpeedBreak)
                {
                    spriteRenderer.enabled = false;
                    rigidbody.isKinematic = true;
                    collider.isTrigger = true;
                    StartCoroutine(PlayParticule());
                    exploded = true;
                    StartCoroutine(DestroyCoroutine());
                }
            });
        }

        IEnumerator PlayParticule()
        {
            particleSystem.Play();
            yield return new WaitForSeconds(particulPlayTime);
            particleSystem.Stop();
        }

        IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(destoryTime);
            spriteRenderer.enabled = true;
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector2.zero;
            collider.isTrigger = false;
            exploded = false;
            eventManager.DestroyGameObject(gameObject.GetInstanceID());
        }
    }
}
