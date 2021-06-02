using UnityEngine;

public class BladeControll : MonoBehaviour
{
  [Header("Upgradables")]
  public float[] DamageMultipliers;
  public float BladeSpeed = 0;
  public float BladeSize = 0;
  public float AttackRange = 0;

  [Header("Components")]
  public float WeaponRadius = 2.5f;
  public GameObject Player;
  public SpriteRenderer PlayerSriteRenderer, GunSpriteRenderer;

  [SerializeField] private float goForwardTime = 0;
  [SerializeField] private float currentWeaponRadius;
  [SerializeField] private bool hasComeBack = true;
  [SerializeField] private Vector3 initialSize;
  [SerializeField] private float damage;

  public bool pickedUp = false;

  private void Start()
  {
    Player = GameObject.FindGameObjectWithTag("Main Player");
    PlayerSriteRenderer = GameObject.FindGameObjectWithTag("Skin").GetComponent<SpriteRenderer>();
    GunSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    if (transform.parent && transform.parent.tag.Contains("Player"))
    {
      init();
    }
  }

  public void init()
  {
    pickedUp = true;
    DamageMultipliers = new float[] { 1 };
    damage = 10;
    foreach (var d in DamageMultipliers)
    {
      damage *= d;
    }
    currentWeaponRadius = WeaponRadius;
    initialSize = transform.localScale;
    transform.localPosition = new Vector2(WeaponRadius, 0);
  }

  private void Update()
  {
    if (Time.deltaTime == 0 || !pickedUp) return;

    //Change size
    transform.localScale = BladeSize * initialSize;

    //Check our mouse position
    Vector3 lookAwayFromPlayer = (transform.position - Player.transform.position).normalized;
    float rot_z = Mathf.Atan2(lookAwayFromPlayer.y, lookAwayFromPlayer.x) * Mathf.Rad2Deg;
    var isMouseRightSide = -90 <= rot_z && rot_z <= 90;

    //Make gun lookaway from player 
    transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

    //MOVE AROUND PLAYER
    //https://stackoverflow.com/questions/57593968/restricting-cursor-to-a-radius-around-my-player
    var _center = new Vector2(Player.transform.position.x, Player.transform.position.y);
    Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 playerToCursor = cursorPos - _center;
    Vector2 dir = playerToCursor.normalized;
    Vector2 cursorVector = dir * currentWeaponRadius;
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
    if (Time.time >= goForwardTime && hasComeBack)
    {
      if (Input.GetMouseButton(0))
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
    if (collision.tag == "Player")
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
  }
}