using System.Collections;
using UnityEngine;

public class NewLevelStart : MonoBehaviour
{
  void Start()
  {
    StartCoroutine("HideAndStartLevel");
  }

  IEnumerator HideAndStartLevel()
  {
    Time.timeScale = 0;
    yield return new WaitForSeconds(3);
    Destroy(this.gameObject);
    Time.timeScale = 1;
  }
}
