using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
  public AudioSource[] songs;
  public AudioSource shop;
  public AudioSource chosenSong;


  void Start()
  {
    var idx = Random.Range(0, songs.Length - 1);
    chosenSong = songs[idx];
    chosenSong.Play();
  }

  public void EnterShop()
  {
    chosenSong.Pause();
    shop.Play();
  }

  public void LeaveShop()
  {
    shop.Pause();
    chosenSong.Play();
  }
}
