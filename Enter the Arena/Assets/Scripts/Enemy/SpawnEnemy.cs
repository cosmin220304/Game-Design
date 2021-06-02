using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
  public GameObject SpawnPrefab;
  public GameObject Enemy;
  public int waveNo;

  public IMovement Movement;
  public EnemyGunControll EnemyGunControll;
  public EnemyBladeControll EnemyBladeControll;

  public void Awake()
  {
    SpawnPrefab.SetActive(false);
    Enemy.SetActive(false);

    Movement = Enemy.GetComponent<IMovement>();
    EnemyGunControll = Enemy.GetComponentInChildren<EnemyGunControll>() ?? null;
    EnemyBladeControll = Enemy.GetComponentInChildren<EnemyBladeControll>() ?? null;

    Movement.enabled = false;
    if (EnemyGunControll != null) EnemyGunControll.enabled = false;
    if (EnemyBladeControll != null) EnemyBladeControll.enabled = false;
  }

  public void SpawnEnemies()
  {
    SpawnPrefab.SetActive(true);
    Enemy.SetActive(true);
    StartCoroutine("EnableEnemy");
  }
  
  IEnumerator EnableEnemy()
  {
    yield return new WaitForSeconds(0.15f);
    Movement.enabled = true;
    if (EnemyGunControll != null) EnemyGunControll.enabled = true;
    if (EnemyBladeControll != null) EnemyBladeControll.enabled = true;
  }  

  private void Update()
  {
    if (Enemy == null)
    {
      Destroy(this.gameObject);
    }
  }
}
