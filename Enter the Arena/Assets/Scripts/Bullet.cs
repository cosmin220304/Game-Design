using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 0;
    public float Damage = 0;
    private float AttackRange = 0;
    private Vector2? Direction = null;
    private GameObject SpawnPoint = null; 

    public void Init(Vector3 direction, GameObject spawnPoint, float speed, float size, float[] damageMultiplier, float attackRange) 
    {
        Direction = direction;
        Speed = speed;
        transform.localScale *= size;
        foreach (var d in damageMultiplier)
        {
            Damage *= d;
        }
        SpawnPoint = spawnPoint;  
        AttackRange = attackRange;
    }

    void Update()
    {
        if (Direction == null)
        {
            Debug.Log("ASdasd");
            return;
        }

        if (Vector2.Distance(SpawnPoint.transform.position, transform.position) > AttackRange)
        {
            Destroy(this.gameObject);
        }
        transform.Translate((Direction.Value * Speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bulletScript = collision.GetComponent<Bullet>();
        if (bulletScript?.SpawnPoint == SpawnPoint)
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
