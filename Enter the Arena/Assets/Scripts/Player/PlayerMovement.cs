using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float Speed;
  public SpriteRenderer Sr;
  public Animator anim;

  private Rigidbody2D rb2d;
  private float currentSpeed;
  private UiController uiController;

  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    currentSpeed = Speed;
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

  void AnimatePlayer()
  {
    if ((Sr.flipX == false && rb2d.velocity.x > 0.01f) || (Sr.flipX == true && rb2d.velocity.x < -0.01f))
    {
      anim.SetInteger("moving", 1);
      currentSpeed = Speed;
    }
    else if ((Sr.flipX == true && rb2d.velocity.x > 0.01f) || (Sr.flipX == false && rb2d.velocity.x < -0.01f))
    {
      anim.SetInteger("moving", -1);
      currentSpeed = 2 * Speed / 3;
    }
    else if (rb2d.velocity.y != 0)
    {
      anim.SetInteger("moving", 1);
      currentSpeed = Speed;
    }
    else
    {
      anim.SetInteger("moving", 0);
      currentSpeed = Speed;
    }
  }
}
