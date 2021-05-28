using UnityEngine;

public class Heart : MonoBehaviour
{
  public float HealAmount;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Main Player")
    {
      collision.GetComponent<IEntityHp>().DealDamage(-HealAmount);
      Destroy(this.gameObject);
    }
  }
}
