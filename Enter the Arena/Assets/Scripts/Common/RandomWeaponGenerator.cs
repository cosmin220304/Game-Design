using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponGenerator : MonoBehaviour
{
  public GameObject[] Positions;
  public GameObject GunPrefab, BladePrefab;
  public GameObject Room;

  private void Start()
  {
    foreach (var position in Positions)
    {
      GenerateWeapon(position.transform.position);
    }
  }

  public void GenerateWeapons()
  {
    RemoveLastGeneratedWeapons();
    foreach (var position in Positions)
    {
      GenerateWeapon(position.transform.position);
    }
  }

  private void RemoveLastGeneratedWeapons()
  {
    float minDist = Mathf.Infinity;
    float minDist2 = Mathf.Infinity;
    float minDist3 = Mathf.Infinity;
    var guns = GameObject.FindGameObjectsWithTag("Gun");
    var blades = GameObject.FindGameObjectsWithTag("Blade");
    var weapons = new List<GameObject>();
    weapons.AddRange(guns);
    weapons.AddRange(blades);

    GameObject w1 = null, w2=null, w3 = null;
    foreach (var w in weapons)
    {
      float dist = Vector2.Distance(w.transform.position, Room.transform.position);
      if (dist < minDist)
      {
        w1 = w;
        minDist = dist;
      }
      else if (dist < minDist2)
      {
        w2 = w;
        minDist2 = dist;
      }
      else if (dist < minDist3)
      {
        w3 = w;
        minDist3 = dist;
      }
    }

    Destroy(w1);
    Destroy(w2);
    Destroy(w3);
  }

  public void GenerateWeapon(Vector2 position)
  {
    if (Random.Range(0f, 1f) < 0.15f)
    {
      var blade = Instantiate(BladePrefab, position, Quaternion.identity);
      var bladeScript = blade.GetComponent<BladeControll>();
      if (Random.Range(0f, 1f) < 0.7)
      {
        bladeScript.DamageMultipliers = new float[] { Random.Range(1f, 2f) };
        bladeScript.BladeSpeed = Random.Range(1f, 5f);
        bladeScript.BladeSize = Random.Range(1f, 2f);
        bladeScript.AttackRange = Random.Range(0.5f, 2f);
      }
      else //rare weapon
      {
        bladeScript.BladeSpeed = Random.Range(5f, 10f);
        bladeScript.BladeSize = Random.Range(2f, 5f);
        bladeScript.AttackRange = Random.Range(2f, 5f);
      }
    }
    else
    {
      var gun = Instantiate(GunPrefab, position, Quaternion.identity);
      var gunScript = gun.GetComponent<GunControll>();
      gunScript.DamageMultipliers = new float[] { Random.Range(1f, 2f) };
      gunScript.BulletSpeed = Random.Range(20f, 60f);
      gunScript.BulletSlots = Random.Range(10f, 60f);
      gunScript.BulletSize = Random.Range(0.75f, 2f);
      gunScript.AttackSpeed = Random.Range(1f, 3f);
      gunScript.AttackRange = Random.Range(10f, 30f);
      gunScript.ReloadTime = Random.Range(1f, 2f);
      gunScript.Recoil = Random.Range(10f, 100f);
      gunScript.BulletNumber = Random.Range(1, 4);

      var randomEffect = Random.Range(0f, 1f); 
      if (randomEffect < 0.1f)
      {
        gunScript.BulletType = BulletTypes.BulletType.Diagonal;
      }
      else if (randomEffect < 0.2f)
      {
        gunScript.BulletType = BulletTypes.BulletType.Aimbot;
      }
      else if (randomEffect < 0.3f)
      {
        gunScript.BulletType = BulletTypes.BulletType.Boomerang;
      }
      else if (randomEffect < 0.4f)
      {
        gunScript.BulletType = BulletTypes.BulletType.Guided;
        gunScript.BulletSpeed *= 0.75f;
      } 
      else if (randomEffect < 0.5f)
      {
        gunScript.BulletType = BulletTypes.BulletType.ZigZag;
      }
      else
      {
        gunScript.BulletType = BulletTypes.BulletType.Normal;
      }


      randomEffect = Random.Range(0f, 1f);
      if (randomEffect < 0.1f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.bee;
      }
      else if (randomEffect < 0.2f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.bomb;
      }
      else if (randomEffect < 0.3f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.bouncy;
      }
      else if (randomEffect < 0.4f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.fire;
      }
      else if (randomEffect < 0.5f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.ghost;
      }
      else if (randomEffect < 0.6f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.healing;
      }
      else if (randomEffect < 0.6f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.ice;
      }
      else if (randomEffect < 0.7f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.toxic;
      }
      else if (randomEffect < 0.8f)
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.water;
      }
      else  
      {
        gunScript.BulletEffect = BulletEffects.BulletEffect.normal;
      }
    }
  }
}
