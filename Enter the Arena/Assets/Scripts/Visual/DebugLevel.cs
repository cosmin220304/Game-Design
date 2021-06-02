using System;
using System.Collections;
using UnityEngine;

public class DebugLevel : MonoBehaviour
{
  public GameObject GunPrefab, Position, EnemyPrefab;

  float[] DamageMultipliers;
  float BulletSpeed;
  int BulletSlots;
  float BulletSize;
  float AttackSpeed;
  float AttackRange;
  float ReloadTime;
  float Recoil;
  int BulletNumber;
  BulletTypes.BulletType BulletType;
  BulletEffects.BulletEffect BulletEffect;

  private void Start()
  {
    DamageMultipliers = new float[] { 1 };
    BulletSpeed = 15;
    BulletSlots = 55;
    BulletSize = 1;
    AttackSpeed = 1;
    AttackRange = 15;
    ReloadTime = 2;
    Recoil = 15;
    BulletNumber = 1;
    BulletType = BulletTypes.BulletType.Normal;
    BulletEffect = BulletEffects.BulletEffect.normal;
    StartCoroutine("CheckIfEnemyIsDead");
  }

  IEnumerator CheckIfEnemyIsDead()
  {
    while(true)
    {
      var enemy = GameObject.FindGameObjectWithTag("Player");
      if (enemy == null)
      {
        var new_enemy = Instantiate(EnemyPrefab, Position.transform.position, Quaternion.identity);
        new_enemy.transform.position = new Vector2(-25, 5);
        FindObjectOfType<EnemyGunControll>().enabled = false;
      }
      yield return new WaitForSeconds(2);
    }
  }

  public void RandomWeapon()
  {
    DamageMultipliers = new float[] { UnityEngine.Random.Range(1f, 2f) };
    BulletSpeed = UnityEngine.Random.Range(20f, 60f);
    BulletSlots = UnityEngine.Random.Range(10, 60);
    BulletSize = UnityEngine.Random.Range(0.75f, 2f);
    AttackSpeed = UnityEngine.Random.Range(1f, 3f);
    AttackRange = UnityEngine.Random.Range(10f, 30f);
    ReloadTime = UnityEngine.Random.Range(1f, 2f);
    Recoil = UnityEngine.Random.Range(10f, 100f);
    BulletNumber = UnityEngine.Random.Range(1, 4);
    BulletType = (BulletTypes.BulletType)UnityEngine.Random.Range(0, 6);
    BulletEffect = (BulletEffects.BulletEffect)UnityEngine.Random.Range(0, 9);
    Generate();
  }

  public void UpdateDamage(string e)
  {
    try
    {
      DamageMultipliers = new float[] { float.Parse(e) };
    }
    catch { }
  }

  public void UpdateBulletSpeed(string e)
  {
    try
    {
      BulletSpeed = float.Parse(e);
    }
    catch { }
  }

  public void UpdateBulletSlots(string e)
  {
    try
    {
      BulletSlots = int.Parse(e);
    }
    catch { }
  }

  public void UpdateBulletSize(string e)
  {
    try
    {
      BulletSize = float.Parse(e);
    }
    catch { }
  }

  public void UpdateAttackSpeed(string e)
  {
    try
    {
      AttackSpeed = float.Parse(e);
    }
    catch { }
  }

  public void UpdateAttackRange(string e)
  {
    try
    {
      AttackRange = float.Parse(e);
    }
    catch { }
  }

  public void UpdateReloadTime(string e)
  {
    try
    {
      ReloadTime = float.Parse(e);
    }
    catch { }
  }

  public void UpdateRecoil(string e)
  {
    try
    {
      Recoil = float.Parse(e);
    }
    catch { }
  }

  public void UpdateBulletNumber(string e)
  {
    try
    {
      BulletNumber = int.Parse(e);
    }
    catch { }
  }

  public void UpdateBulletType(string e)
  {
    try
    {
      BulletType = (BulletTypes.BulletType)Enum.Parse(typeof(BulletTypes.BulletType), e);
    }
    catch { }
  }

  public void UpdateBulletEffect(string e)
  {
    try
    {
      BulletEffect = (BulletEffects.BulletEffect)Enum.Parse(typeof(BulletEffects.BulletEffect), e);
    }
    catch { }
  }

  public void Generate()
  {
    var existingGuns = GameObject.FindGameObjectsWithTag("Gun");
    foreach (var existingGun in existingGuns)
    {
      if (existingGun.transform.parent == null) 
      {
        Destroy(existingGun);
      }
    }

    var gun = Instantiate(GunPrefab, Position.transform.position, Quaternion.identity);
    var gunScript = gun.GetComponent<GunControll>();
    gunScript.DamageMultipliers = DamageMultipliers;
    gunScript.BulletSpeed = BulletSpeed;
    gunScript.BulletSlots = BulletSlots;
    gunScript.BulletSize = BulletSize;
    gunScript.AttackSpeed = AttackSpeed;
    gunScript.AttackRange = AttackRange;
    gunScript.ReloadTime = ReloadTime;
    gunScript.Recoil = Recoil;
    gunScript.BulletNumber = BulletNumber;
    gunScript.BulletType = BulletType;
    gunScript.BulletEffect = BulletEffect;
  }
}
