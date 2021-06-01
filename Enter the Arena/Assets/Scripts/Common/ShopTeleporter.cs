using UnityEngine;

public class ShopTeleporter : MonoBehaviour
{
  public GameObject teleportPosition;
  public bool isTeleportedToShop;

  private void Update()
  {
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Main Player"))
    {
      var enemiesScripts = FindObjectsOfType<EnemyMovement>();
      foreach(var enemyScript in enemiesScripts)
      {
        enemyScript.isPlayerTeleported = isTeleportedToShop;
      }

      collision.transform.position = teleportPosition.transform.position;

      var musicPlayer = FindObjectOfType<MusicPlayer>();
      if (isTeleportedToShop)
      {
        musicPlayer.EnterShop();
        Destroy(this.gameObject);
      }
      else
      {
        musicPlayer.LeaveShop();
      }
    }
  }

}
