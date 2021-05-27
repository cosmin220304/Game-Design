using UnityEngine;

public class Water : MonoBehaviour
{
  public GameObject WaterParticles;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.tag.Contains("Player"))
    {
      SpawnParticles(collision);
      collision.GetComponent<IMovement>()?.AddEffect(PlayerEffects.PlayerEffect.Slow, 1);
    }
  } 

  void SpawnParticles(Collider2D collision)
  {
    var waterParticles = Instantiate(WaterParticles, collision.transform.position, Quaternion.identity);
    Destroy(waterParticles, 0.5f);
  }
}
