using System.Collections;
using UnityEngine;

public class IMovement : MonoBehaviour
{
  public float Speed = 5f;
  public SpriteRenderer Sr;
  public Animator Anim;
  public GameObject FrozenEffect;
  public GameObject FearEffect;
  public GameObject SlowEffect;
  public GameObject ReloadEffect;
  public GameObject SilenceEffect;

  protected Rigidbody2D rb2d;
  protected float currentSpeed, initialSpeed;
  protected bool isRetreating = false;

  private PlayerEffects.PlayerEffect currenPlayerEffect;
  private int freezeCounter = 0;

  protected void AnimatePlayer()
  {
    if ((Sr.flipX == false && rb2d.velocity.x > 0.01f) || (Sr.flipX == true && rb2d.velocity.x < -0.01f))
    {
      Anim.SetInteger("moving", 1);
      currentSpeed = Speed;
    }
    else if ((Sr.flipX == true && rb2d.velocity.x > 0.01f) || (Sr.flipX == false && rb2d.velocity.x < -0.01f))
    {
      Anim.SetInteger("moving", -1);
      currentSpeed = 2 * Speed / 3;
    }
    else if (rb2d.velocity.y != 0)
    {
      Anim.SetInteger("moving", 1);
      currentSpeed = Speed;
    }
    else
    {
      Anim.SetInteger("moving", 0);
      currentSpeed = Speed;
    }
  }

  public void AddEffect(PlayerEffects.PlayerEffect playerEffect, float duration)
  {
    switch (playerEffect)
    {
      case PlayerEffects.PlayerEffect.Slow: 
        StartCoroutine("Slow", duration);
        break;
      case PlayerEffects.PlayerEffect.Fear:
        StartCoroutine("Fear", duration);
        break;
      case PlayerEffects.PlayerEffect.Freeze:
        freezeCounter++;
        if (freezeCounter % 3 != 0) break;
        StartCoroutine("Freeze", duration);
        break;
      case PlayerEffects.PlayerEffect.Realoding:
        StartCoroutine("Reload", duration);
        break;
      default:
        break;
    }
  }

  private IEnumerator Reload(float duration)
  {
    if (currenPlayerEffect == PlayerEffects.PlayerEffect.Realoding) yield break;

    currenPlayerEffect = PlayerEffects.PlayerEffect.Realoding;
    ReloadEffect.SetActive(true);

    yield return new WaitForSeconds(duration);

    ReloadEffect.SetActive(false);
    currenPlayerEffect = PlayerEffects.PlayerEffect.None;
  }

  private IEnumerator Slow(float duration)
  {
    if (currenPlayerEffect == PlayerEffects.PlayerEffect.Slow) yield break;

    currenPlayerEffect = PlayerEffects.PlayerEffect.Slow;
    Speed /= 2;
    SlowEffect.SetActive(true);

    yield return new WaitForSeconds(duration);

    SlowEffect.SetActive(false);
    Speed = initialSpeed;
    currenPlayerEffect = PlayerEffects.PlayerEffect.None;
  }

  private IEnumerator Fear(float duration)
  {
    if(currenPlayerEffect == PlayerEffects.PlayerEffect.Fear) yield break;

    currenPlayerEffect = PlayerEffects.PlayerEffect.Fear;
    Speed *= -1;
    FearEffect.SetActive(true);

    yield return new WaitForSeconds(duration);

    FearEffect.SetActive(false);
    Speed = initialSpeed;
    currenPlayerEffect = PlayerEffects.PlayerEffect.None;
  }

  private IEnumerator Freeze(float duration)
  {
    if (currenPlayerEffect == PlayerEffects.PlayerEffect.Freeze) yield break;

    currenPlayerEffect = PlayerEffects.PlayerEffect.Freeze;
    Speed = 0;
    FrozenEffect.SetActive(true);

    yield return new WaitForSeconds(duration);

    FrozenEffect.SetActive(false);
    Speed = initialSpeed;
    currenPlayerEffect = PlayerEffects.PlayerEffect.None;
  } 
}
