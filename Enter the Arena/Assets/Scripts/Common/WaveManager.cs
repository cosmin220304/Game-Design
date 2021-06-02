using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
  public IDictionary<int, IList<SpawnEnemy>> waveSpawners;
  public int currentWave;

  private bool hasSpawnedPortal, hasSpawnedHP;
  public GameObject PortalPrefab, HPPrefab;

  void Start()
  {
    hasSpawnedPortal = true;
    hasSpawnedHP = true;
    currentWave = 1;
    var waves = GameObject.FindGameObjectsWithTag("Spawn");
    waveSpawners = new Dictionary<int, IList<SpawnEnemy>>();

    foreach (var wave in waves)
    {
      var waveScript = wave.GetComponent<SpawnEnemy>();
      var waveNo = waveScript.waveNo;
      if (waveSpawners.ContainsKey(waveNo))
      {
        waveSpawners[waveNo].Add(waveScript);
      }
      else
      {
        waveSpawners[waveNo] = new List<SpawnEnemy>() { waveScript };
      }
    }
    
    SpawnEnemies();
  }

  void Update()
  {
    if (!hasSpawnedPortal && currentWave % 3 == 0)
    {
      var position = new Vector2(Random.Range(-40f, 40f), Random.Range(-8f, 15f));
      Instantiate(PortalPrefab, position, Quaternion.identity);
      hasSpawnedPortal = true;
    }

    if (!hasSpawnedHP)
    {
      var position = new Vector2(Random.Range(-40f, 40f), Random.Range(-8f, 15f));
      Instantiate(HPPrefab, position, Quaternion.identity);
      hasSpawnedHP = true;
    }

    if (!waveSpawners.ContainsKey(currentWave))
    {
      Destroy(this.gameObject);
      return;
    }

    if (waveSpawners[currentWave].All(w => w == null))
    {
      currentWave++;
      hasSpawnedPortal = false;
      hasSpawnedHP = false;
      SpawnEnemies();
    }
  }

  void SpawnEnemies()
  {
    if (!waveSpawners.ContainsKey(currentWave))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      return;
    }

    foreach (var spawner in waveSpawners[currentWave])
    {
      spawner.SpawnEnemies();
    }
  }
}
