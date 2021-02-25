using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Upgradables
    public float Speed = 0;
    public float Size = 0;
    public float Damage = 1; 

    public Vector2 Direction = Vector2.zero;
    public GameObject SpawnPoint;

    private float destroyTimer = Mathf.Infinity;

    public void Init(Vector3 direction, GameObject spawnPoint, float speed, float size, float[] damageMultiplier, float attackRange) 
    {
        Direction = direction;
        Speed = speed;
        Size = size;
        SpawnPoint = spawnPoint;  
        foreach (var d in damageMultiplier)
        {
            Damage *= d;
        }
        destroyTimer = Time.time + attackRange;
        transform.localScale *= Size;
    }

    void Update()
    {
        if (Time.time >= destroyTimer)
        {
            Destroy(this.gameObject);
        }

        transform.Translate((Direction * Speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bulletScript = collision.GetComponent<Bullet>();
        if (bulletScript && bulletScript.SpawnPoint == SpawnPoint)
        {
            return;
        }
        Destroy(this.gameObject);
    }

}
