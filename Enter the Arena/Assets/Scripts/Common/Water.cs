using UnityEngine;

public class Water : MonoBehaviour
{
  public GameObject WaterParticles;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.tag.Contains("Player"))
    {
      SpawnParticles(collision);
      var enemyMov = collision.GetComponent<EnemyMovement>();
      var playerMov = collision.GetComponent<PlayerMovement>();
      if (enemyMov)
      {
        enemyMov.Speed /= 2; 
      }
      else if (playerMov)
      {
        playerMov.Speed /= 2;
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.transform.tag.Contains("Player"))
    {
      SpawnParticles(collision);
      var enemyMov = collision.GetComponent<EnemyMovement>();
      var playerMov = collision.GetComponent<PlayerMovement>();
      if (enemyMov)
      {
        enemyMov.Speed *= 2;
      }
      else if (playerMov)
      {
        playerMov.Speed *= 2;
      }
    }
  }

  void SpawnParticles(Collider2D collision)
  {
    var waterParticles = Instantiate(WaterParticles, collision.transform.position, Quaternion.identity);
    Destroy(waterParticles, 0.5f);
  }
}
