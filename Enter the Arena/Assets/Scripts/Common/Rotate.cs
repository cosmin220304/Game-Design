using UnityEngine;

public class Rotate : MonoBehaviour
{
  private void Update()
  {
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 5);
  }

}
