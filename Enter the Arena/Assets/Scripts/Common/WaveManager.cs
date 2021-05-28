using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
  public IDictionary<int, IList<SpawnEnemy>> waveSpawners;
  public int currentWave;

  void Start()
  {
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
    if (!waveSpawners.ContainsKey(currentWave))
    {
      Destroy(this.gameObject);
      return;
    }

    if (waveSpawners[currentWave].All(w => w == null))
    {
      currentWave++;
      SpawnEnemies();
    }
  }

  void SpawnEnemies()
  {
    foreach (var spawner in waveSpawners[currentWave])
    {
      spawner.SpawnEnemies();
    }
  }
}
