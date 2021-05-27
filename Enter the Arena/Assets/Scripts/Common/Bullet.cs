using UnityEngine;

public class Bullet : MonoBehaviour
{
  public GameObject ImpactEffectPrefab, ExplosionEffectPrefab, WaterPrefab;
  public TrailRenderer TrailRenderer;
  public SpriteRenderer BulletSpriteRenderer;

  private float Speed = 0;
  private float Damage = 0;
  private float AttackRange = 0;
  private Vector2 Direction;
  private GameObject SpawnPoint = null;
  private Vector2 SpawnPosition;
  private GameObject Origin;
  private BulletTypes.BulletType BulletType;
  private BulletEffects.BulletEffect BulletEffect;
  private GameObject ClosestEnemy;
  private int BulletNumber;

  private bool comeBack = false;
  private bool dropDown = false;
  private float tick = 0;
  private bool followMouse = true;
  private bool throwEffect = false;

  private bool noDamage = false;

  public void Init(GameObject origin, Vector3 direction, GameObject spawnPoint, Vector2 spawnPosition,
    BulletTypes.BulletType bulletType, BulletEffects.BulletEffect bulletEffect,
    float speed, float size, float[] damageMultiplier, float attackRange, int bulletNumber)
  {
    Origin = origin;
    transform.tag = origin.tag;
    Direction = direction;
    Speed = speed;
    transform.localScale *= size;
    Damage = 10;
    foreach (var d in damageMultiplier)
    {
      Damage *= d;
    }
    SpawnPoint = spawnPoint;
    AttackRange = attackRange;
    SpawnPosition = spawnPosition;
    BulletType = bulletType;
    BulletEffect = bulletEffect;
    BulletNumber = bulletNumber;

    InitBulletType();
    InitBulletEffect();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (ShouldIgnore(collision)) return;

    if (!noDamage)
    {
      DealDamage(collision);
    }

    if (!throwEffect && BulletEffect != BulletEffects.BulletEffect.ghost)
    {
      DestroyBullet(collision);
    }
  }

  void Update()
  {
    if (Direction == null)
    {
      return;
    }

    switch (BulletType)
    {
      case BulletTypes.BulletType.Aimbot:
        AimBotTravel();
        break;
      case BulletTypes.BulletType.Guided:
        GuidedTravel();
        break;
      case BulletTypes.BulletType.Boomerang:
        BoomerangTravel();
        break;
      case BulletTypes.BulletType.ZigZag:
        ZigZagTravel();
        break;
      case BulletTypes.BulletType.Launcher:
        LauncherTravel();
        break;
      case BulletTypes.BulletType.Normal:
      case BulletTypes.BulletType.Diagonal:
      default:
        NormalBulletTravel();
        break;
    }
  }
  void LauncherTravel()
  {
    if (!dropDown && Vector2.Distance(SpawnPosition, transform.position) > AttackRange)
    {
      dropDown = true;
    }

    Speed -= 0.2f;
    if (!dropDown)
    {
      transform.Translate(Direction * Speed * Time.deltaTime);
    }
    else
    {
      if (SpawnPoint.transform.position.y > Origin.transform.position.y)
      {
        Direction.y -= 0.1f;
        if (transform.position.y < SpawnPoint.transform.position.y)
        {
          DestroyBullet();
        }
      }
      else
      {
        Direction.y += 0.1f;
        if (transform.position.y > SpawnPoint.transform.position.y)
        {
          DestroyBullet();
        }
      }

      if (Mathf.Abs(transform.position.y - SpawnPoint.transform.position.y) < 3)
      {
        noDamage = false;
      }
      transform.Translate(Direction * Speed * Time.deltaTime);
    }
  }

  void ZigZagTravel()
  {
    if (Vector2.Distance(SpawnPosition, transform.position) > AttackRange)
    {
      DestroyBullet();
    }

    tick += 0.5f;
    var direction = new Vector2();
    direction.x = Direction.x;

    switch (BulletNumber)
    {
      case 4:
        direction.y = Direction.y - Mathf.Cos(tick);
        break;
      case 3:
        direction.y = Direction.y + Mathf.Cos(tick);
        break;
      case 2:
        direction.y = Direction.y - Mathf.Sin(tick);
        break;
      default:
        direction.y = Direction.y + Mathf.Sin(tick);
        break;
    }
    transform.Translate(direction * Speed * Time.deltaTime);
  }

  void AimBotTravel()
  {
    if (Vector2.Distance(SpawnPosition, transform.position) > AttackRange)
    {
      DestroyBullet();
    }

    if (ClosestEnemy == null)
    {
      transform.Translate(Direction * Speed * Time.deltaTime);
      return;
    }

    var direction = Direction;
    if (transform.position.x < ClosestEnemy.transform.position.x)
      direction.x += 1;
    else
      direction.x -= 1;
    if (transform.position.y < ClosestEnemy.transform.position.y)
      direction.y += 1;
    else
      direction.y -= 1;
    Direction = direction.normalized;
    transform.Translate(Direction * Speed * Time.deltaTime);
  }

  void GuidedTravel()
  {
    if (Vector2.Distance(SpawnPosition, transform.position) > AttackRange)
    {
      DestroyBullet();
    }

    var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if (Vector2.Distance(mousePosition, transform.position) < 1.5)
    {
      followMouse = false;
    }

    if (followMouse)
    {
      var direction = Direction;
      if (transform.position.x < mousePosition.x)
        direction.x += 1;
      else
        direction.x -= 1;
      if (transform.position.y < mousePosition.y)
        direction.y += 1;
      else
        direction.y -= 1;
      Direction = direction.normalized;
      transform.Translate(Direction * Speed * Time.deltaTime);
    }
    else
    {
      transform.Translate(Direction * Speed * Time.deltaTime);
    }
  }

  void BoomerangTravel()
  {
    if (!comeBack && Vector2.Distance(SpawnPosition, transform.position) > AttackRange)
    {
      comeBack = true;
    }

    if (!comeBack)
    {
      transform.Translate(Direction * Speed * Time.deltaTime);
    }
    else
    {
      transform.Translate(Direction * -Speed * Time.deltaTime);
    }
  }

  void NormalBulletTravel()
  {
    if (Vector2.Distance(SpawnPosition, transform.position) > AttackRange)
    {
      DestroyBullet();
    }

    transform.Translate(Direction * Speed * Time.deltaTime);
  }

  private void GetClosesEnemy()
  {
    float minDist = Mathf.Infinity;
    var enemies = GameObject.FindGameObjectsWithTag("Player");

    foreach (var e in enemies)
    {
      if (e.name == "Player") continue;

      float dist = Vector2.Distance(e.transform.position, transform.position);
      if (dist < minDist)
      {
        ClosestEnemy = e;
        minDist = dist;
      }
    }
  }

  private void DealDamage(Collider2D collision)
  {
    if (collision.tag.Contains("Player"))
    {
      collision.GetComponent<IEntityHp>()?.DealDamage(Damage);
    }
  }

  private void DestroyBullet(Collider2D collision = null)
  {
    var effect = Instantiate(ImpactEffectPrefab, transform.position, Quaternion.identity) as GameObject;
    effect.transform.parent = collision ? collision.transform : null;
    effect.transform.localScale *= transform.localScale.x;
    effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

    var destroyEffectTimer = 0.5f;
    var destroyBulletTimer = 0f;
    var byPassLaucher = false;
    IEntityHp hpScript = null;
    if (collision != null)
    {
      hpScript = collision.GetComponent<IEntityHp>() ?? null;
    }

    switch (BulletEffect)
    {
      case BulletEffects.BulletEffect.toxic:
        if (!hpScript) break;
        destroyEffectTimer = 3f;
        hpScript.ApplyPoision(Damage);
        break;
      case BulletEffects.BulletEffect.fire:
        if (!hpScript) break;
        hpScript.ApplyFire();
        break;
      case BulletEffects.BulletEffect.bomb:
        SpawnExplosion();
        break;
      case BulletEffects.BulletEffect.healing:
        if (!hpScript) break;
        Origin.GetComponent<IEntityHp>()?.DealDamage(-Damage);
        break;
      case BulletEffects.BulletEffect.water:
        var water = Instantiate(WaterPrefab, transform.position, Quaternion.identity);
        water.transform.parent = null;
        break;
      case BulletEffects.BulletEffect.bouncy:
        if (!collision) break;
        destroyBulletTimer = 3f;
        var rotation = Random.Range(150, 210);
        Direction = Direction.Rotate(rotation);
        break;
      case BulletEffects.BulletEffect.ice:
      case BulletEffects.BulletEffect.bee:
        destroyBulletTimer = 3f;
        var rrotation = Random.Range(-90, 90);
        Direction = Direction.Rotate(rrotation);
        byPassLaucher = true;
        break;
      default:
        break;
    }

    if (BulletType == BulletTypes.BulletType.Launcher && !byPassLaucher)
    {
      SpawnExplosion(true);
    }

    Destroy(effect, destroyEffectTimer);
    Destroy(this.gameObject, destroyBulletTimer);
  }

  private void SpawnExplosion(bool isBigExplosion = false)
  {
    var explosion = Instantiate(ExplosionEffectPrefab, transform.position, Quaternion.identity);
    explosion.transform.parent = null;
    explosion.GetComponent<Explode>().Init(Damage, BulletEffect);
    if (isBigExplosion)
    {
      explosion.transform.localScale *= 2;
    }
  }

  private bool ShouldIgnore(Collider2D collision)
  {
    if (collision.tag == "Blade")
    {
      return true;
    }

    if (collision.GetComponent<Bullet>())
    {
      return true;
    }

    if (Origin.CompareTag(collision.tag))
    {
      return true;
    }

    if (collision.tag == "Water")
    {
      return true;
    }

    return false;
  }

  private void InitBulletType()
  {
    if (BulletType == BulletTypes.BulletType.Diagonal)
    {
      switch (BulletNumber)
      {
        case 1:
          Direction = Direction.Rotate(45);
          break;
        case 2:
          Direction = Direction.Rotate(15);
          break;
        case 3:
          Direction = Direction.Rotate(-45);
          break;
        case 4:
          Direction = Direction.Rotate(-15);
          break;
        default:
          Debug.LogError($"INVALID BULLET NUMBER {BulletNumber}");
          break;
      };
    }
    else if (BulletType == BulletTypes.BulletType.Aimbot)
    {
      GetClosesEnemy();
    }
    else if (BulletType == BulletTypes.BulletType.Launcher)
    {
      throwEffect = true;
      noDamage = true;
    }
  }

  private void InitBulletEffect()
  {
    var bulletColor = BulletSpriteRenderer.color;

    switch (BulletEffect)
    {
      case BulletEffects.BulletEffect.toxic:
        bulletColor = Color.green;
        break;
      case BulletEffects.BulletEffect.ghost:
        bulletColor.a = 0.5f;
        TrailRenderer.enabled = false;
        break;
      case BulletEffects.BulletEffect.fire:
        bulletColor = Color.red;
        break;
      case BulletEffects.BulletEffect.healing:
        bulletColor = Color.magenta;
        break;
      case BulletEffects.BulletEffect.bee:
        bulletColor = Color.yellow;
        break;
      case BulletEffects.BulletEffect.water:
        bulletColor = Color.blue;
        TrailRenderer.enabled = false;
        break;
      case BulletEffects.BulletEffect.ice:
        bulletColor = Color.blue;
        break;
      case BulletEffects.BulletEffect.normal:
      default:
        break;
    }

    BulletSpriteRenderer.color = bulletColor;
    TrailRenderer.material.SetColor("_Color", bulletColor);
  }
}
