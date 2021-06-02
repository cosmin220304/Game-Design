using System.Collections;
using TMPro;
using UnityEngine;

public class NoobHelper : MonoBehaviour
{
  public AudioSource[] Tutorial;
  public TMP_Text NoobText;

  void Start()
  {
    StartCoroutine("TutorialHelper");
  }

  IEnumerator TutorialHelper()
  {
    yield return new WaitForSeconds(2);
    NoobText.text = "You can use [W] [A] [S] [D] to move.	";
    Tutorial[0].Play();

    yield return new WaitForSeconds(4);
    NoobText.text = "And left click to shoot.	";
    Tutorial[1].Play();

    yield return new WaitForSeconds(8);
    NoobText.text = "Press [R] to reload.	";
    Tutorial[2].Play();

    yield return new WaitForSeconds(8);
    NoobText.text = "";
  }
}
