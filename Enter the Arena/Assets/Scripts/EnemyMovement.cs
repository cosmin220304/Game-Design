using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    public SpriteRenderer sr;

    private Seeker seeker;
    private Path path;
    private Rigidbody2D rb2d;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

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
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
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
        if (path == null)
        {
            return;
        }

        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } 
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb2d.AddForce(force);
        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }


        //Animation
        if (rb2d.velocity.x >= 0.01f)
        {
            sr.flipX = false;
        }
        else if (rb2d.velocity.x <= -0.01f)
        {
            sr.flipX = true;
        }
    }
}
