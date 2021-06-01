using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class GunControll : MonoBehaviour
{
  [Header("Upgradables")]
  public float[] DamageMultipliers;
  public float BulletSpeed = 0;
  public float BulletSlots = 0;
  public float BulletSize = 0;
  public float AttackSpeed = 0;
  public float AttackRange = 0;
  public float ReloadTime = 0;
  public float Recoil = 0;
  public int BulletNumber = 1;
  public BulletTypes.BulletType BulletType;
  public BulletEffects.BulletEffect BulletEffect;

  [Header("Components")]
  public TMP_Text bulletSlotsText;
  public float WeaponRadius;
  public GameObject Player;
  public GameObject BulletSpawn;
  public GameObject BulletPrefab;
  public SpriteRenderer PlayerSriteRenderer, GunSpriteRenderer;

  [SerializeField] private bool isRealoding = false;
  [SerializeField] private float currentBullets = 0;
  [SerializeField] private float shootAgainTime = 0;
  [SerializeField] private float currentGunRecoil = 0;
  [SerializeField] private float resetRecoilTime = 0;
  [SerializeField] private float circleRecoil = 0;
  [SerializeField] private bool hasRecoil;
  [SerializeField] private IMovement movementScript;

  private bool pickedUp = false;

  private void Start()
  {
    if (transform.parent.tag.Contains("Player"))
    {
      init();
      pickedUp = true;
    }
  }

  private void init()
  {
    DamageMultipliers = new float[] { 1 };
    currentBullets = BulletSlots;
    UpdateBulletSlotsInterface();
    movementScript = this.transform.parent.GetComponent<IMovement>();
    transform.position = new Vector2(WeaponRadius, 0);
  }

  private void Update()
  {
    if (Time.deltaTime == 0 || !pickedUp) return;

    //Check if you have recoil
    hasRecoil = Recoil != 0;

    //Check our mouse position
    Vector3 lookAwayFromPlayer = (transform.position - Player.transform.position).normalized;
    float rot_z = Mathf.Atan2(lookAwayFromPlayer.y, lookAwayFromPlayer.x) * Mathf.Rad2Deg;
    var isMouseRightSide = -90 <= rot_z && rot_z <= 90;

    //Make gun lookaway from player 
    var addedRotation = isMouseRightSide ? currentGunRecoil : -currentGunRecoil;
    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + addedRotation);
    if (Math.Abs(transform.rotation.z - 90) < 5)
    {
      transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    //MOVE AROUND PLAYER
    //https://stackoverflow.com/questions/57593968/restricting-cursor-to-a-radius-around-my-player
    var _center = new Vector2(Player.transform.position.x, Player.transform.position.y);
    Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    cursorPos.y += circleRecoil / 10;
    Vector2 playerToCursor = cursorPos - _center;
    Vector2 dir = playerToCursor.normalized;
    Vector2 cursorVector = dir * WeaponRadius;
    transform.position = _center + cursorVector;

    //Flip player && weapon
    if (isMouseRightSide)
    {
      GunSpriteRenderer.flipY = false;
      PlayerSriteRenderer.flipX = false;
    }
    else
    {
      GunSpriteRenderer.flipY = true;
      PlayerSriteRenderer.flipX = true;
    }

    //Shoot
    if (Input.GetMouseButton(0) && Time.time >= shootAgainTime && currentBullets > 0 && !isRealoding)
    {
      shootAgainTime = Time.time + 1 / AttackSpeed;
      currentBullets -= BulletNumber;
      if (currentBullets < 0) currentBullets = 0;
      UpdateBulletSlotsInterface();

      if (hasRecoil)
      {
        currentGunRecoil += 100 / Recoil;
        if (currentGunRecoil > Recoil && Math.Abs(transform.rotation.z - 90) < 5)
        {
          currentGunRecoil = resetRecoilTime;
        }
        circleRecoil += 1000 / Recoil;
        resetRecoilTime = Time.time + 1 / AttackSpeed;
      }

      Vector2 direction = (BulletSpawn.transform.position - transform.position).normalized;
      switch (BulletNumber)
      {
        case 4:
          var offset4 = (Vector2)BulletSpawn.transform.position + new Vector2(direction.y, direction.x) * 2;
          if (BulletType == BulletTypes.BulletType.ZigZag) offset4 = BulletSpawn.transform.position;
          ShootBullet(offset4, 4);
          goto case 3;
        case 3:
          var offset3 = (Vector2)BulletSpawn.transform.position + new Vector2(direction.y, -direction.x);
          if (BulletType == BulletTypes.BulletType.ZigZag) offset3 = BulletSpawn.transform.position;
          ShootBullet(offset3, 3);
          goto case 2;
        case 2:
          var offset2 = (Vector2)BulletSpawn.transform.position + new Vector2(direction.y, direction.x);
          if (BulletType == BulletTypes.BulletType.ZigZag) offset2 = BulletSpawn.transform.position;
          ShootBullet(offset2, 2);
          goto default;
        default:
          ShootBullet(BulletSpawn.transform.position, 1);
          break;
      }
    }

    if (hasRecoil)
    {
      if (Time.time >= resetRecoilTime)
      {
        currentGunRecoil -= AttackSpeed * 10 / Recoil;
        if (currentGunRecoil < 0)
        {
          currentGunRecoil = 0f;
        }
      }
      circleRecoil -= 100 / Recoil;
      if (circleRecoil < 0)
      {
        circleRecoil = 0f;
      }
    }

    //Reload
    if (Input.GetKey(KeyCode.R) && !isRealoding || currentBullets < 1)
    {
      StartCoroutine("Reload");
      movementScript.AddEffect(PlayerEffects.PlayerEffect.Realoding, ReloadTime);
    }
  }

  private void ShootBullet(Vector2 bulletSpawn, int bulletNumber)
  {
    Vector2 direction = (BulletSpawn.transform.position - transform.position).normalized;
    GameObject bullet = Instantiate(BulletPrefab, bulletSpawn, Quaternion.identity);
    bullet.transform.parent = null;
    bullet.GetComponent<Bullet>()
        .Init(transform.parent.gameObject, direction, BulletSpawn, BulletSpawn.transform.position, BulletType, BulletEffect,
        BulletSpeed, BulletSize, DamageMultipliers, AttackRange, bulletNumber);
  }

  private void UpdateBulletSlotsInterface()
  {
    bulletSlotsText.text = $"{currentBullets} / {BulletSlots}";
  }

  private IEnumerator Reload()
  {
    isRealoding = true;
    yield return new WaitForSeconds(ReloadTime);
    currentBullets = BulletSlots;
    isRealoding = false;
    UpdateBulletSlotsInterface();
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, WeaponRadius);

    Gizmos.color = Color.red;
    Vector2 direction = (BulletSpawn.transform.position - transform.position).normalized * AttackRange;
    Gizmos.DrawRay(BulletSpawn.transform.position, direction);

    Gizmos.DrawWireSphere((Vector2)BulletSpawn.transform.position + direction, BulletSize * 0.5f);
  }
}
