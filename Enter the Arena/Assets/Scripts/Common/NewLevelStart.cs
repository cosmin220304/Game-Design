using System.Collections;
using UnityEngine;

public class NewLevelStart : MonoBehaviour
{
  PlayerHp PlayerHp;
  void Start()
  {
    StartCoroutine("HideAndStartLevel");
    PlayerHp = FindObjectOfType<PlayerHp>();
  }

  private void Update()
  {
    PlayerHp.DealDamage(-100);
  }

  IEnumerator HideAndStartLevel()
  {
    Time.timeScale = 0;
    yield return new WaitForSeconds(3);
    Destroy(this.gameObject);
    Time.timeScale = 1;
  }
}
