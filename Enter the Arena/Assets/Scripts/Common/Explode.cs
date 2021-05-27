using UnityEngine;

public class Explode : MonoBehaviour
{
  private float Damage;
  private Rigidbody2D col;
  private BulletEffects.BulletEffect BulletEffect;

  private void Start()
  {
    col = GetComponent<Rigidbody2D>();
  }

  public void Init(float damage, BulletEffects.BulletEffect bulletEffect)
  {
    Damage = damage;
    BulletEffect = bulletEffect;
    Destroy(this.gameObject, 1f);
    Destroy(col, 0.5f);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag.Contains("Player"))
    {
      collision.GetComponent<IEntityHp>()?.DealDamage(Damage);

      IEntityHp hpScript = null;
      if (collision != null)
      {
        hpScript = collision.GetComponent<IEntityHp>() ?? null;
      }
      switch (BulletEffect)
      {
        case BulletEffects.BulletEffect.toxic:
          hpScript?.ApplyPoision(Damage);
          break;
        case BulletEffects.BulletEffect.fire:
          hpScript?.ApplyFire();
          break; 
        default:
          break;
      }
    }
  }
}
