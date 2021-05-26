using System.Collections;
using UnityEngine;

public class EnemyHp : IEntityHp
{
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    isTakingDamage = false;
    initialColor = sr.color;
    currentColor = initialColor;
  }
}
