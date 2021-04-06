using System.Collections;
using UnityEngine;

public class EnemyHp : MonoBehaviour, IEntityHp
{
    public float HP;
    public SpriteRenderer sr;
    public GameObject DeathParticlesPrefab;

    private bool isTakingDamage;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isTakingDamage = false;
    }


    public void DealDamage(float damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            var deathParticles = Instantiate(DeathParticlesPrefab, rb.position, Quaternion.identity);
            deathParticles.transform.parent = null;
            Destroy(this.gameObject);
        }

        if (!isTakingDamage)
        {
            StartCoroutine("FlashWithDamage");
        }
    }

    private IEnumerator FlashWithDamage()
    {
        isTakingDamage = true;
        var initialColor = sr.color;
        sr.color = new Color(initialColor.r * 0.2f, initialColor.g * 0.2f, initialColor.b * 0.2f);
        yield return new WaitForSeconds(0.5f);
        sr.color = initialColor;
        isTakingDamage = false;
    }

}
