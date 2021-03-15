//using UnityEngine;

//public class GunControll : MonoBehaviour
//{
//    public int Min, Max;
//    void Update()
//    {
//        //The distance from your player to the camera
//        float camToPlayerDist = Vector3.Distance(transform.position, Camera.main.transform.position);

//        //This is the world position of your mouse
//        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camToPlayerDist));

//        //The direction your mouse is pointing in with relation to your player
//        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;

//        //the angle of your direction
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//        if (Min < angle && angle < Max)
//        {
//            //Setting the rotation to the transform.
//            transform.rotation = Quaternion.Euler(0, 0, angle);
//        }
//        Debug.Log(angle);
//    }
//}

//public class Target : MonoBehaviour
//{

//    private float RotateSpeed = 5f;
//    private float Radius = 0.1f;

//    private Vector2 _centre;
//    private float _angle;

//    private void Start()
//    {
//        _centre = transform.position;
//    }

//    private void Update()
//    {

//        _angle += RotateSpeed * Time.deltaTime;

//        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
//        transform.position = _centre + offset;
//    }
//}

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

    [Header("Components")]
    public TMP_Text bulletSlotsText;
    public float WeaponRadius = 2.5f;
    public GameObject Player;
    public GameObject BulletSpawn;
    public GameObject BulletPrefab;
    public SpriteRenderer PlayerSriteRenderer, GunSpriteRenderer;

    private bool isRealoding = false;
    private float currentBullets = 0;
    private float shootAgainTime = 0;
    private float currentGunRecoil = 0;
    private float resetRecoilTime = 0;
    private float circleRecoil = 0;
    private bool hasRecoil;

    private void Start()
    {
        DamageMultipliers = new float[] { 1 };
        currentBullets = BulletSlots;
        UpdateBulletSlotsInterface();
    }

    private void Update()
    {
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
            currentBullets -= 1;
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
            GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.transform.position, Quaternion.identity);
            bullet.transform.parent = null;
            bullet.GetComponent<Bullet>()
                .Init(direction, BulletSpawn, BulletSpeed, BulletSize, DamageMultipliers, AttackRange);
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
        }
    }

    public void UpdateBulletSlotsInterface()
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
