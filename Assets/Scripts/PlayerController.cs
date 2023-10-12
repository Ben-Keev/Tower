using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Import new input system
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]

    public float speedGround = 10f;
    [Tooltip("Fraction that ground speed will be reduced by when in air")]
    public float speedFractionAir = 0.75f;

    [Header("Jump")]
    public float jumpHeight = 10f;

    [Header("Gravity")]
    
    public float gravityScale = 10f;

    [Tooltip("How fast the player falls, separate from their gravity when jumping")]
    public float fallingGravityScale = 20f;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private bool grounded = false;

    private void Awake() {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update() { 
        Movement();
        bool jump = playerInput.actions["Jump"].WasPressedThisFrame();

        if(jump) Jump();

        // Player is rising, gravity scale is set to its typical value.
        if(rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }

        // Player is falling, gravity scale is set higher.
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
    }

    private void Movement() {
        //  Action from new input system
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;

        // This anim plays if the player is grounded
        animator.SetBool("Walk", horizontalInput !=0);

        if (!grounded) {
            // Move the player Character slower in the air
            rb.velocity = new Vector2((horizontalInput*speedGround)*speedFractionAir, rb.velocity.y);
        } else { // On ground, get full movement speedGround
            rb.velocity = new Vector2(horizontalInput*speedGround, rb.velocity.y);
        }
    }

    private void Jump() {
        grounded = true;
        //https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/#:~:text=The%20basic%20method%20of%20jumping,using%20the%20Add%20Force%20method.
        // Impulse applies force immediately, creating a jumping motion
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * speedGround, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
    }
}
