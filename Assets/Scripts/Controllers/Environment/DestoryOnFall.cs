using Assets.Scripts.Manager.Events;
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
        public float speedBreak = 3;
        public int particulPlayTime = 1;
        public int destoryTime = 2;

        Rigidbody2D _rigidbody;
        ParticleSystem _particleSystem;
        BoxCollider2D _collider;
        SpriteRenderer _spriteRenderer;
        LevelEventManager _levelEventManager;

        bool _exploded = false;

        private void Start()
        {
            _levelEventManager = LevelEventManager.current;
            _rigidbody = GetComponent<Rigidbody2D>();
            _particleSystem = GetComponent<ParticleSystem>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (_exploded)
                return;

            Vector2 boxPoint = _rigidbody.position + new Vector2(_collider.offset.x, -_collider.size.y);
            Vector2 boxSize = new Vector2((_collider.size.x * 0.95f), 0.50f);
            // Collider to detected collision with ground, to check if grounded
            Collider2D[] groundOverlapBox = Physics2D.OverlapBoxAll(boxPoint
                , boxSize, transform.rotation.y, groundLayer);
            bool groundHit = groundOverlapBox != null;
            Debug.DrawRay(boxPoint, boxSize, groundHit ? Color.green : Color.red);

            groundOverlapBox.ToList().ForEach((overlapCollider) =>
            {
                if (overlapCollider.gameObject != gameObject && Mathf.Abs(_rigidbody.velocity.y) > speedBreak)
                {
                    _spriteRenderer.enabled = false;
                    _rigidbody.isKinematic = true;
                    _collider.isTrigger = true;
                    StartCoroutine(PlayParticule());
                    _exploded = true;
                    StartCoroutine(DestroyCoroutine());
                }
            });
        }

        IEnumerator PlayParticule()
        {
            _particleSystem.Play();
            yield return new WaitForSeconds(particulPlayTime);
            _particleSystem.Stop();
        }

        IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(destoryTime);
            _spriteRenderer.enabled = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector2.zero;
            _collider.isTrigger = false;
            _exploded = false;
            _levelEventManager.DestroyGameObject(gameObject.GetInstanceID());
        }
    }
}
