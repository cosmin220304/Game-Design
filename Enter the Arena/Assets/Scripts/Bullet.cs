using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 0;
    public float Damage = 0;
    private float AttackRange = 0;
    private Vector2? Direction = null;
    private GameObject SpawnPoint = null;
    private Vector2 spawnPosition;

    public void Init(Vector3 direction, GameObject spawnPoint, float speed, float size, float[] damageMultiplier, float attackRange) 
    {
        Direction = direction;
        Speed = speed;
        transform.localScale *= size;
        Damage = 10;
        foreach (var d in damageMultiplier)
        {
            Damage *= d;
        }
        SpawnPoint = spawnPoint;  
        AttackRange = attackRange;
        spawnPosition = SpawnPoint.transform.position;
    }

    void Update()
    {
        if (Direction == null)
        {
            return;
        }

        if (Vector2.Distance(spawnPosition, transform.position) > AttackRange)
        {
            Destroy(this.gameObject);
        }
        transform.Translate((Direction.Value * Speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Blade")
        {
            return;
        }

        var bulletScript = collision.GetComponent<Bullet>();
        if (bulletScript?.SpawnPoint == SpawnPoint)
        {
            return;
        }

        if (collision.tag == "Player")
        {
            collision.GetComponent<IEntityHp>().DealDamage(Damage);
        }

        Destroy(this.gameObject);
    }
}
