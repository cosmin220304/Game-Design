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
    var guns = GameObject.FindGameObjectsWithTag("Gun");
    var blades = GameObject.FindGameObjectsWithTag("Blade");
    var water = GameObject.FindGameObjectsWithTag("Water");
    var stuffToDelete = new List<GameObject>();
    stuffToDelete.AddRange(guns);
    stuffToDelete.AddRange(blades);
    stuffToDelete.AddRange(water);
    
    foreach (var stuff in stuffToDelete)
    {
      if (stuff.transform.position.x < -50)
      {
        Destroy(stuff);
      }
    }
  }

  public void GenerateWeapon(Vector2 position)
  {
    if (Random.Range(0f, 1f) < 0.15f)
    {
      var blade = Instantiate(BladePrefab, position, Quaternion.identity);
      var bladeScript = blade.GetComponent<BladeControll>();
      bladeScript.DamageMultipliers = new float[] { Random.Range(1f, 2f) };
      bladeScript.BladeSpeed = Random.Range(1f, 5f);
      bladeScript.BladeSize = Random.Range(1f, 2f);
      bladeScript.AttackRange = Random.Range(0.5f, 2f);
    }
    else
    {
      var gun = Instantiate(GunPrefab, position, Quaternion.identity);
      var gunScript = gun.GetComponent<GunControll>();
      var damage = Random.Range(1f, 2f);
      gunScript.DamageMultipliers = new float[] { damage };
      gunScript.BulletSpeed = Random.Range(5f, 60f);
      gunScript.BulletSlots = Random.Range(1, 60);
      gunScript.BulletSize = Random.Range(0.5f, 2f);
      gunScript.AttackSpeed = Random.Range(0.25f, 3f);
      gunScript.AttackRange = Random.Range(5f, 30f);
      gunScript.ReloadTime = Random.Range(1f, 4f);
      gunScript.Recoil = Random.Range(5f, 100f);
      gunScript.BulletNumber = Random.Range(1, 4);

      gunScript.DamageMultipliers[0] /= gunScript.BulletNumber;

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
