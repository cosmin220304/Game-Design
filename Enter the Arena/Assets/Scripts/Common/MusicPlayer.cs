using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
  public AudioSource[] songs;
  void Start()
  {
    var idx = Random.Range(0, songs.Length - 1);
    var randomSong = songs[idx];
    randomSong.Play();
  }
}
