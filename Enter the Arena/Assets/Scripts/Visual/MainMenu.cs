using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject Story;
  private AssetBundle myLoadedAssetBundle;
  private string[] scenePaths;

  void Start()
  {
    Story.SetActive(false);
  }

  public void EnableStory()
  {
    Story.SetActive(true);
  }
  
  public void DisableStory()
  {
    Story.SetActive(false);
  }

  public void StartGame()
  {
    SceneManager.LoadScene("Level1", LoadSceneMode.Single);
  }

  public void DebuggingLevel()
  {
    SceneManager.LoadScene("Debug", LoadSceneMode.Single);
  }
}

