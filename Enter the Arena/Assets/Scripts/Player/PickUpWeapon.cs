using System.Collections;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
  public bool isInRange;
  public bool pickedUp;
  public GameObject player;
  public Transform oldWeapon;

  private void Start()
  {
    isInRange = false;
    pickedUp = false;
    player = GameObject.FindGameObjectWithTag("Main Player");
  }

  private void Update()
  {
    if (isInRange && !pickedUp && Input.GetKey(KeyCode.E))
    {
      pickedUp = true;
      PickWeapon();
    }
  }

  private void PickWeapon()
  {
    foreach (Transform child in player.transform)
    {
      if ((child.CompareTag("Gun") || child.CompareTag("Blade")) && child != this.transform)
      {
        oldWeapon = child;
        StartCoroutine("MakeOldWeaponDropable");
        MakeThisNewWeapon();
        break;
      }
    }
  }

  private void MakeThisNewWeapon()
  {
    transform.parent = player.transform;

    if (this.CompareTag("Gun"))
    {
      this.GetComponent<GunControll>().init();
    }
    else
    {
      this.GetComponent<BladeControll>().init();
    }
  }

  private IEnumerator MakeOldWeaponDropable()
  {
    if (oldWeapon.CompareTag("Gun"))
    {
      oldWeapon.GetComponent<GunControll>().pickedUp = false;
    }
    else
    {
      oldWeapon.GetComponent<BladeControll>().pickedUp = false;
    }

    oldWeapon.parent = null;

    yield return new WaitForSeconds(1);
    
    oldWeapon.GetComponent<BoxCollider2D>().enabled = true;
    oldWeapon.GetComponent<PickUpWeapon>().pickedUp = false;
    GetComponent<BoxCollider2D>().enabled = false;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Main Player"))
    {
      isInRange = true;
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.CompareTag("Main Player"))
    {
      isInRange = false;
    }
  }
}
