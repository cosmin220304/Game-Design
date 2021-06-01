using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTeleporter : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Main Player"))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
  }
}
