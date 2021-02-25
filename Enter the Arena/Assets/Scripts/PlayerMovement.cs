using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;  
    public SpriteRenderer Sr;
    public Animator anim;

    private Rigidbody2D rb2d;
    private float currentSpeed;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentSpeed = Speed;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * currentSpeed;
        float moveVertical = Input.GetAxisRaw("Vertical") * currentSpeed;

        rb2d.velocity = new Vector2(moveHorizontal, moveVertical);

        if ( (Sr.flipX == false && moveHorizontal > 0) || (Sr.flipX == true && moveHorizontal < 0))
        {
            anim.SetInteger("moving", 1);
            currentSpeed = Speed;
        }
        else if ((Sr.flipX == true && moveHorizontal > 0) || (Sr.flipX == false && moveHorizontal < 0))
        {
            anim.SetInteger("moving", -1);
            currentSpeed = Speed / 2;
        }
        else if (moveVertical != 0)
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
