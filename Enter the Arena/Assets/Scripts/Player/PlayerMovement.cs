using UnityEngine;

public class PlayerMovement : IMovement
{
  private UiController uiController;

  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    currentSpeed = Speed;
    initialSpeed = Speed;
    uiController = FindObjectOfType<UiController>();
  }

  void FixedUpdate()
  {
    float moveHorizontal = Input.GetAxis("Horizontal") * currentSpeed;
    float moveVertical = Input.GetAxis("Vertical") * currentSpeed;
    rb2d.velocity = new Vector2(moveHorizontal, moveVertical);
    AnimatePlayer();

    if (Input.GetKey(KeyCode.Escape))
    {
      uiController.TogglePause();
    }
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
    {
      uiController.ToggleWeapon();
    }
  } 
}
