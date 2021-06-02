using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossEnd : MonoBehaviour
{
  public GameObject Boss;

  private void Update()
  {
    if (Boss == null)
    {
      SceneManager.LoadScene("End");
    }
  }
}
