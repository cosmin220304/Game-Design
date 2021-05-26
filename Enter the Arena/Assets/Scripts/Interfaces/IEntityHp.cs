using System.Collections;
using UnityEngine;

public abstract class IEntityHp : MonoBehaviour
{
  public SpriteRenderer sr;
  public float HP;
  public GameObject DeathParticlesPrefab;
  public Rigidbody2D rb;
  protected bool isTakingDamage;
  protected Color initialColor;
  protected Color currentColor;

  private bool isOnFire = false;
  private float fireDamage = 5;

  public virtual void DealDamage(float damage)
  {
    HP -= damage;
    if (HP < 0)
    {
      var deathParticles = Instantiate(DeathParticlesPrefab, rb.position, Quaternion.identity);
      deathParticles.transform.parent = null;
      Destroy(this.gameObject);
      Destroy(deathParticles, 5);
    }

    if (!isTakingDamage)
    {
      StartCoroutine("FlashWithDamage");
    }
  }

  public void ApplyPoision(float damage)
  {
    StartCoroutine("PoisionDamage", damage / 2);
  }

  private IEnumerator PoisionDamage(float damage)
  {
    currentColor = Color.green;

    DealDamage(damage);
    yield return new WaitForSeconds(1);
    DealDamage(damage);
    yield return new WaitForSeconds(1);
    DealDamage(damage);

    currentColor = initialColor;
  }

  public void ApplyFire()
  {
    StartCoroutine("FireDamage");
  }

  private IEnumerator FireDamage()
  {
    currentColor = Color.red;
    isOnFire = true;

    DealDamage(fireDamage);
    yield return new WaitForSeconds(1);
    DealDamage(fireDamage);
    yield return new WaitForSeconds(1);
    DealDamage(fireDamage);
    yield return new WaitForSeconds(1);
    DealDamage(fireDamage);
    yield return new WaitForSeconds(1);
    DealDamage(fireDamage);

    currentColor = initialColor;
    isOnFire = false;
  }

  private IEnumerator FlashWithDamage()
  {
    isTakingDamage = true;
    sr.color = new Color(currentColor.r * 0.2f, currentColor.g * 0.2f, currentColor.b * 0.2f);

    yield return new WaitForSeconds(0.5f);

    sr.color = currentColor;
    isTakingDamage = false;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (isOnFire && collision.transform.tag.Contains("Player"))
    {
      collision.transform.GetComponent<IEntityHp>().ApplyFire();
    }
  }
}
