using UnityEngine;
using System;

public class BladeControll : MonoBehaviour
{
    //Upgradables
    public float[] DamageMultipliers;
    public float BladeSpeed = 0;
    public float BladeSize = 0; 
    public float AttackRange = 0;

    public float WeaponRadius = 2.5f;
    public GameObject Player;
    public SpriteRenderer PlayerSriteRenderer, GunSpriteRenderer;

    private float goForwardTime = 0;
    private float currentWeaponRadius;
    private bool hasComeBack = true;
    private Vector3 initialSize;

    private void Start()
    {
        DamageMultipliers = new float[] { 1 };
        currentWeaponRadius = WeaponRadius;
        initialSize = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = BladeSize * initialSize;

        //Check our mouse position
        Vector3 lookAwayFromPlayer = (transform.position - Player.transform.position).normalized;
        float rot_z = Mathf.Atan2(lookAwayFromPlayer.y, lookAwayFromPlayer.x) * Mathf.Rad2Deg;
        var isMouseRightSide = -90 <= rot_z && rot_z <= 90;

        //Make gun lookaway from player 
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z); 

        //MOVE AROUND PLAYER
        //https://stackoverflow.com/questions/57593968/restricting-cursor-to-a-radius-around-my-player
        var _center = new Vector2(Player.transform.position.x, Player.transform.position.y);
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToCursor = cursorPos - _center;
        Vector2 dir = playerToCursor.normalized;
        Vector2 cursorVector = dir * currentWeaponRadius;
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

        if (Time.time >= goForwardTime && hasComeBack)
        {
            //Shoot
            if (Input.GetMouseButton(0))
            {
                hasComeBack = false;
                goForwardTime = Time.time + AttackRange / 10;
            }
        }
        else if (Time.time < goForwardTime)
        {
            currentWeaponRadius += Time.deltaTime * BladeSpeed * 10; 
        }
        else if (!hasComeBack)
        {
            currentWeaponRadius -= Time.deltaTime * BladeSpeed * 10;
            if (currentWeaponRadius < WeaponRadius)
            {
                currentWeaponRadius = WeaponRadius;
                hasComeBack = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, WeaponRadius);
    }
}