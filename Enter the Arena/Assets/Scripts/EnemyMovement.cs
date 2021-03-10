using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    public float distanceAway;
    public Transform target;
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    public SpriteRenderer Sr;
    public Animator anim;

    private Seeker seeker;
    private Path path;
    private Rigidbody2D rb2d;
    private int currentWayPoint = 0;
    private float currentSpeed;
    private float currentDistanceAway;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            var x_off = Random.Range(-10.0f, 10.0f);
            var y_off = Random.Range(-10.0f, 10.0f);
            if (distanceAway == 0)
            {
                x_off = Random.Range(-1.0f, 1.0f);
                y_off = Random.Range(-1.0f, 1.0f);
            }
            var offsetTarget = new Vector3(target.position.x + x_off, target.position.y + y_off, target.position.z);
            seeker.StartPath(rb2d.position, offsetTarget, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        AnimatePlayer();
        
        RaycastHit2D hit = Physics2D.Linecast(rb2d.position, target.position);
        if (hit.collider != null && hit.transform.tag != "Player")
        {
            currentDistanceAway = 0;
        }
        else
        {
            currentDistanceAway = distanceAway;
        }
        
        if (currentWayPoint >= path.vectorPath.Count || Vector2.Distance(rb2d.position, target.position) <= currentDistanceAway || path == null)
        {
            rb2d.velocity = new Vector2(0, 0);
            return;
        } 

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb2d.position).normalized;
        var x_off = Random.Range(-1.0f, 1.0f);
        var y_off = Random.Range(-1.0f, 1.0f);
        direction = new Vector2(direction.x + x_off, direction.y + y_off);
        Vector2 force = direction * currentSpeed * Time.deltaTime;
        rb2d.AddForce(force);
        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    void AnimatePlayer()
    {
        if ((Sr.flipX == false && rb2d.velocity.x > 0.01f) || (Sr.flipX == true && rb2d.velocity.x < -0.01f))
        {
            anim.SetInteger("moving", 1);
            currentSpeed = speed;
        }
        else if ((Sr.flipX == true && rb2d.velocity.x > 0.01f) || (Sr.flipX == false && rb2d.velocity.x < -0.01f))
        {
            anim.SetInteger("moving", -1);
            currentSpeed = 2 * speed / 3;
        }
        else if (rb2d.velocity.y != 0)
        {
            anim.SetInteger("moving", 1);
            currentSpeed = speed;
        }
        else
        {
            anim.SetInteger("moving", 0);
            currentSpeed = speed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentDistanceAway); 
    }
}
