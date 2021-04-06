using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour, IEntityHp
{
    private UiController uiController;
    public float HP;
    public Slider HpBar;
    public SpriteRenderer sr;

    private bool isTakingDamage;

    void Start()
    {
        isTakingDamage = false;
        uiController = FindObjectOfType<UiController>();
        UpdateInterface();
    }

    public void DealDamage(float damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            uiController.GameOver();
            return;
        }

        UpdateInterface();

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

    private void UpdateInterface()
    {
        HpBar.value = HP / 100;
    }
}
