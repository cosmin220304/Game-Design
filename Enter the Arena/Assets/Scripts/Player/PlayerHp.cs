using UnityEngine.UI;

public class PlayerHp : IEntityHp
{
  private UiController uiController;
  public Slider HpBar;

  void Start()
  {
    isTakingDamage = false;
    uiController = FindObjectOfType<UiController>();
    UpdateInterface();
    initialColor = sr.color;
    currentColor = initialColor;
  }

  public override void DealDamage(float damage)
  {
    HP -= damage;
    if (HP < 0)
    {
      uiController.GameOver();
      return;
    }
    else if (HP > 100)
    {
      HP = 100;
    }

    UpdateInterface();

    if (!isTakingDamage && damage > 0)
    {
      StartCoroutine("FlashWithDamage");
    }
  }

  private void UpdateInterface()
  {
    HpBar.value = HP / 100;
  }
}
