using UnityEngine;

public class EnemyBladeControll : MonoBehaviour
{
  [Header("Upgradables")]
  public float[] DamageMultipliers;
  public float BladeSpeed = 0;
  public float BladeSize = 0;
  public float AttackRange = 0;
  public float DistanceAway = 0;

  [Header("Components")]
  public float WeaponRadius = 2.5f;
  public GameObject Enemy, Target;
  public SpriteRenderer PlayerSriteRenderer, GunSpriteRenderer;

  private float goForwardTime = 0;
  private float currentWeaponRadius;
  private bool hasComeBack = true;
  private Vector3 initialSize;
  private float damage;

  private void Start()
  {
    Target = GameObject.FindGameObjectWithTag("Main Player");
    damage = 10;
    foreach (var d in DamageMultipliers)
    {
      damage *= 10;
    }
    currentWeaponRadius = WeaponRadius;
    initialSize = transform.localScale;
  }

  private void FixedUpdate()
  {
    //Change size
    transform.localScale = BladeSize * initialSize;

    //Check our mouse position
    Vector3 lookAwayFromPlayer = (transform.position - Enemy.transform.position).normalized;
    float rot_z = Mathf.Atan2(lookAwayFromPlayer.y, lookAwayFromPlayer.x) * Mathf.Rad2Deg;
    var isMouseRightSide = -90 <= rot_z && rot_z <= 90;

    //Make gun lookaway from player 
    transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

    //MOVE AROUND PLAYER
    //https://stackoverflow.com/questions/57593968/restricting-cursor-to-a-radius-around-my-player
    var _center = new Vector2(Enemy.transform.position.x, Enemy.transform.position.y);
    Vector2 cursorVector = ((Vector2)Target.transform.position - _center).normalized * currentWeaponRadius;
    transform.position = _center + cursorVector;

    //Flip player && weapon
    if (isMouseRightSide)
    {
      GunSpriteRenderer.flipY = false;
      PlayerSriteRenderer.flipX = false;
    }
    else
    {
      GunSpriteRenderer.flipY = true;
      PlayerSriteRenderer.flipX = true;
    }

    //Knife attack 
    if (Time.time >= Time.time && hasComeBack)
    {
      if (Vector2.Distance(Enemy.transform.position, Target.transform.position) <= DistanceAway)
      {
        hasComeBack = false;
        goForwardTime = Time.time + AttackRange / 10;
      }
    }
    else if (Time.time < goForwardTime)
    {
      currentWeaponRadius += Time.deltaTime * BladeSpeed * 10;
    }
    else if (!hasComeBack)
    {
      currentWeaponRadius -= Time.deltaTime * BladeSpeed * 10;
      if (currentWeaponRadius < WeaponRadius)
      {
        currentWeaponRadius = WeaponRadius;
        hasComeBack = true;
      }
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Main Player")
    {
      collision.GetComponent<IEntityHp>()?.DealDamage(damage);
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, WeaponRadius);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, AttackRange);

    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, DistanceAway);
  }
}
