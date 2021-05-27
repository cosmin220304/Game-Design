using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
  public float DistanceAway;
  public float RetreatDistance;
  public Transform Target;
  public float Speed = 5f;
  public float NextWaypointDistance = 3f;
  public SpriteRenderer Sr;
  public Animator Anim;
  public GameObject weapon;

  private Seeker seeker;
  private Path path;
  private Rigidbody2D rb2d;
  private int currentWayPoint = 0;
  private float currentSpeed;
  private float currentDistanceAway;
  private bool isRetreating;

  void Start()
  {
    seeker = GetComponent<Seeker>();
    rb2d = GetComponent<Rigidbody2D>();
    isRetreating = false;
    InvokeRepeating("UpdatePath", 0f, .5f);
  }

  void UpdatePath()
  {
    if (seeker.IsDone())
    {
      var x_off = Random.Range(-2.0f, 2.0f);
      var y_off = Random.Range(-2.0f, 2.0f);
      //If is melee
      if (DistanceAway == 0)
      {
        x_off = Random.Range(-1.0f, 1.0f);
        y_off = Random.Range(-1.0f, 1.0f);
      }
      var offsetTarget = new Vector3(Target.position.x + x_off, Target.position.y + y_off, Target.position.z);
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

    if (isRetreating)
    {
      rb2d.velocity = (rb2d.position - (Vector2)Target.position).normalized * currentSpeed;
      return;
    }

    RaycastHit2D hit = Physics2D.Linecast(rb2d.position, Target.position);
    if (hit.collider != null && hit.transform.gameObject != Target.gameObject)
    {
      currentDistanceAway = 0;
    }
    else
    {
      currentDistanceAway = DistanceAway;
    }

    var distanceFromPlayer = Vector2.Distance(rb2d.position, Target.position);
    if (distanceFromPlayer <= RetreatDistance)
    {
      StartCoroutine(Retreat(Target));
    }
    if (path == null || currentWayPoint >= path.vectorPath.Count || distanceFromPlayer <= currentDistanceAway)
    {
      rb2d.velocity = new Vector2(0, 0);
      return;
    }

    Vector2 direction = (Vector2)path.vectorPath[currentWayPoint] - rb2d.position;
    var x_off = Random.Range(-1.0f, 1.0f);
    var y_off = Random.Range(-1.0f, 1.0f);
    direction = new Vector2(direction.x + x_off, direction.y + y_off);
    Vector2 force = direction.normalized * Speed;
    rb2d.velocity = force;

    float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWayPoint]);
    if (distance < NextWaypointDistance)
    {
      currentWayPoint++;
    }
  }
  private IEnumerator Retreat(Transform target)
  {
    var tempTarget = Target;

    isRetreating = true;
    Target = target;
    yield return new WaitForSeconds(Random.Range(1, 3));
    Target = tempTarget;
    isRetreating = false;
  }

  void AnimatePlayer()
  {
    if ((Sr.flipX == false && rb2d.velocity.x > 0.01f) || (Sr.flipX == true && rb2d.velocity.x < -0.01f))
    {
      Anim.SetInteger("moving", 1);
      currentSpeed = Speed;
    }
    else if ((Sr.flipX == true && rb2d.velocity.x > 0.01f) || (Sr.flipX == false && rb2d.velocity.x < -0.01f))
    {
      Anim.SetInteger("moving", -1);
      currentSpeed = 2 * Speed / 3;
    }
    else if (rb2d.velocity.y != 0)
    {
      Anim.SetInteger("moving", 1);
      currentSpeed = Speed;
    }
    else
    {
      Anim.SetInteger("moving", 0);
      currentSpeed = Speed;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.tag == "Main Player" && !isRetreating)
    {
      StartCoroutine(Retreat(collision.collider.transform));
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, currentDistanceAway);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, RetreatDistance);
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, DistanceAway);
  }
}
