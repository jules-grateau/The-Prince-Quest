using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Rigidbody2D))]
public class DestoryOnFall : MonoBehaviour
{
    public LayerMask groundLayer;
    public float SpeedBreak = 3;
    new Rigidbody2D rigidbody;
    new ParticleSystem particleSystem;
    new BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    bool exploded = false;
    private void Start()
    {
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
            if(overlapCollider.gameObject != gameObject)
            {
                Debug.Log(overlapCollider.gameObject.name);
                Debug.Log(Mathf.Abs(rigidbody.velocity.y));
            }
            if (overlapCollider.gameObject != gameObject && Mathf.Abs(rigidbody.velocity.y) > SpeedBreak)
            {
                Debug.Log(Mathf.Abs(rigidbody.velocity.y));
                spriteRenderer.enabled = false;
                rigidbody.isKinematic = true;
                collider.isTrigger = true;
                particleSystem.Play();
                exploded = true;
                Destroy(gameObject, 5);
            }
        });
    }
}
