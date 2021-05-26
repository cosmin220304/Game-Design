using UnityEngine;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
  public GameObject GameOverScreen;
  public GameObject PauseScreen;
  public GameObject Weapon;

  private void Start()
  {
    GameOverScreen.SetActive(false);
    PauseScreen.SetActive(false);
    Weapon.SetActive(false);
    Time.timeScale = 1;
  }

  public void GameOver()
  {
    GameOverScreen.SetActive(true);
    Time.timeScale = 0;
  }

  public void TogglePause()
  {
    if (!PauseScreen.activeSelf)
    {
      PauseScreen.SetActive(true);
      Time.timeScale = 0;
    }
    else
    {
      PauseScreen.SetActive(false);
      Time.timeScale = 1;
    }
  }

  public void Restart()
  {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void ToggleWeapon()
  {
    if (!Weapon.activeSelf)
    {
      Weapon.SetActive(true);
    }
    else
    {
      Weapon.SetActive(false);
    }
  }

  public void Exit()
  {
    Application.Quit();
  }
}
