using UnityEngine;

public class RightPart : MonoBehaviour
{
    public GunControll GunScript;
    public EnemyGunControll EnemyGunControll;

    public int NumberOfBullets;


    void Start()
    { 
    }

    void Update()
    {
        Debug.Log(GunScript.BulletSpeed);
    }
}
