using System.Collections;
using TMPro;
using UnityEngine;

public class ShopTeleporter : MonoBehaviour
{
  public GameObject teleportPosition;
  public bool isTeleportedToShop;
  public bool firstTimeInShop;
  public AudioSource PickUpWeaponAudio;
  public TMP_Text NoobText;

  private void Update()
  {
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Main Player"))
    {
      EnableOrDisableEnemies();
      collision.transform.position = teleportPosition.transform.position;
      PlayMusic();
      StartCoroutine("NoobHelper");
      RegenerateWeapons();
      CleanUp();
    }
  }

  private void RegenerateWeapons()
  {
    if (!isTeleportedToShop)
    {
      FindObjectOfType<RandomWeaponGenerator>().GenerateWeapons();
    }
  }

  private void EnableOrDisableEnemies()
  {
    var enemiesScripts = FindObjectsOfType<EnemyMovement>();
    foreach (var enemyScript in enemiesScripts)
    {
      enemyScript.isPlayerTeleported = isTeleportedToShop;
    }
  }

  private void PlayMusic()
  {
    var musicPlayer = FindObjectOfType<MusicPlayer>();
    if (isTeleportedToShop)
    {
      musicPlayer.EnterShop();
    }
    else
    {
      musicPlayer.LeaveShop();
    }
  }

  private IEnumerator NoobHelper()
  {
    yield return new WaitForSeconds(2);
    if (isTeleportedToShop && firstTimeInShop)
    {
      firstTimeInShop = false;
      PickUpWeaponAudio.Play();
      NoobText.text = "Press [E] to pick up a weapon.";
    }
    else
    {
      NoobText.text = "";
    }
  }
  private void CleanUp()
  {
    if (isTeleportedToShop)
    {
      Destroy(this.GetComponent<CircleCollider2D>());
      transform.localScale = new Vector3(0, 0, 0);
      Destroy(this.gameObject, 5);
    }
  }
}
