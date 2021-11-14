using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 250f;
    public float xMinDistance;
    public float xMaxDistance;
    public GameObject lookDirectionObject;
    public float deathSpeed = 3f;

    private float xFirstDestination;
    private float xSecondDestination;
    private float xDestination;
    private Vector2 destinationDirection;
    private Rigidbody2D enemyRb;
    private Animator enemyAnimator;
    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        xFirstDestination = transform.position.x + xMinDistance;
        xSecondDestination = transform.position.x + xMaxDistance;
        xDestination = xFirstDestination;
        CalculateDestinationDirection();
        CalculateLookDirection();
    }

    void CalculateDestinationDirection()
    {
        var destinationDistance = xDestination - transform.position.x;

        if (destinationDistance > 0)
            destinationDirection = Vector2.right;
        if (destinationDistance < 0)
            destinationDirection = Vector2.left;
    }

    void CalculateLookDirection()
    {
        Vector2 lookDirection = lookDirectionObject.transform.position - transform.position;

        if (destinationDirection == Vector2.right && lookDirection.x < 0)
        {
            enemyRb.transform.eulerAngles = enemyRb.transform.eulerAngles + new Vector3(0, 180, 0);
        }
        if (destinationDirection == Vector2.left && lookDirection.x > 0)
        {
            enemyRb.transform.eulerAngles = enemyRb.transform.eulerAngles + new Vector3(0, 180, 0);
        }
    }

    void SetNextDestination()
    {
        if(xDestination == xFirstDestination)
        {
            xDestination = xSecondDestination;
        } else
        {
            xDestination = xFirstDestination;
        }
        CalculateDestinationDirection();
        CalculateLookDirection();
    }

    public void Kill()
    {
        enemyAnimator.SetBool("isDead", true);
        enemyRb.isKinematic = true;
        enemyRb.velocity = Vector2.zero;
        isAlive = false;
        GetComponent<Collider2D>().isTrigger = true;
        Destroy(gameObject, 2);
    }
    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            enemyRb.velocity = new Vector2(speed * destinationDirection.x * Time.deltaTime, enemyRb.velocity.y);

            if (destinationDirection == Vector2.right && xDestination <= transform.position.x)
                SetNextDestination();

            if (destinationDirection == Vector2.left && xDestination >= transform.position.x)
                SetNextDestination();

            return;
        }

        if(!isAlive)
        {
            transform.Translate(Vector2.down * deathSpeed * Time.deltaTime);
        }

    }
}