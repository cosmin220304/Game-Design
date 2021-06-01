using UnityEngine;

public class Heart : MonoBehaviour
{
  public float HealAmount;

  private void Start()
  {
    Destroy(this.gameObject, 5);
  }

  private void Update()
  {
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 10, transform.eulerAngles.z);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Main Player")
    {
      collision.GetComponent<IEntityHp>().DealDamage(-HealAmount);
      Destroy(this.gameObject);
    }
  }
}
