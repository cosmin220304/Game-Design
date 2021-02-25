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
    //Upgradables
    public float[] DamageMultipliers;
    public float BulletSpeed = 0;
    public float BulletSlots = 0;
    public float BulletSize = 0;
    public float AttackSpeed = 0;
    public float AttackRange = 0;
    public float MaxBullets = 0;
    public float ReloadTime = 0;
    public float Recoil = 0;

    public TMP_Text bulletSlotsText;
    public float WeaponRadius = 2.5f;
    public GameObject Player;
    public GameObject BulletSpawn;
    public GameObject BulletPrefab;
    public SpriteRenderer PlayerSriteRenderer, GunSpriteRenderer;

    private bool isRealoding = false;
    private float currentBullets = 0;
    private float shootAgainTime = 0;
    private float currentRecoil = 0;
    private float resetRecoilTime = 0;

    private void Start()
    {
        DamageMultipliers = new float[] { 1 };
        currentBullets = MaxBullets; 
        UpdateBulletSlotsInterface();
    }
    public bool isLeftA(Vector2 point)
    {
        var a = new Vector2(-1, 1);
        var b = new Vector3(1, -1);
        return ((b.x - a.x) * (point.y - a.y) - (b.y - a.y) * (point.x - a.x)) > 0;
    }

    public bool isLeftB(Vector2 point)
    {
        var a = new Vector2(-1, -1);
        var b = new Vector3(1, 1);
        return ((b.x - a.x) * (point.y - a.y) - (b.y - a.y) * (point.x - a.x)) > 0;
    }

    private void Update()
    {  
        //MOVE AROUND PLAYER
        //https://stackoverflow.com/questions/57593968/restricting-cursor-to-a-radius-around-my-player
        var _center = new Vector2(Player.transform.position.x, Player.transform.position.y);
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.y += currentRecoil / 10;
        Vector2 playerToCursor = cursorPos - _center;
        Vector2 dir = playerToCursor.normalized; 
        Vector2 cursorVector = dir * WeaponRadius;
        transform.position = _center + cursorVector;

        //Make gun look away
        Vector3 lookAwayFromPlayer = (transform.position - Player.transform.position).normalized;
        float rot_z = Mathf.Atan2(lookAwayFromPlayer.y, lookAwayFromPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + currentRecoil);

        //Flip player && weapon
        if (-90 <= rot_z && rot_z <= 90)
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

            GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.transform.position, transform.rotation);
            bullet.transform.parent = null;
            bullet.GetComponent<Bullet>()
                .Init(lookAwayFromPlayer, BulletSpawn, BulletSpeed, BulletSize, DamageMultipliers, AttackRange);

            currentRecoil += 100 / Recoil;
            resetRecoilTime = Time.time + 2 / AttackSpeed;
        }
        
        //Recoil logic
        if (Time.time >= resetRecoilTime )
        {
            currentRecoil -= 10 / Recoil;
            if (currentRecoil < 0)
            {
                currentRecoil = 0f;
            }
        }

        //Reload
        if (Input.GetKey(KeyCode.R) && !isRealoding)
        {
            isRealoding = true;
            StartCoroutine("Reload"); 
        }  
    }

    public void UpdateBulletSlotsInterface()
    {
        bulletSlotsText.text = $"{currentBullets} / {MaxBullets}";
    }

    private IEnumerator Reload()
    { 
        yield return new WaitForSeconds(ReloadTime);
        currentBullets = MaxBullets;
        isRealoding = false;
        UpdateBulletSlotsInterface();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, WeaponRadius);
    }
}
