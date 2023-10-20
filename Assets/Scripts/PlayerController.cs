using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Import new input system
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private float spawnX = -5;
    [SerializeField] private float spawnY = -3;

    [Header("Blocks")]
    [Tooltip("First map colour player may control")]
    [SerializeField] private Colour colourOne = Colour.Red;
    [Tooltip("Second map colour player may control")]
    [SerializeField] private Colour colourTwo = Colour.Blue;

    [Header("Speed")]
    [SerializeField] private float speedGround = 10f;
    [Tooltip("Fraction that ground speed will be reduced by when in air")]
    [SerializeField] private float speedFractionAir = 0.75f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 10f;

    [Header("Gravity")]
    
    [SerializeField] private float gravityScale = 10f;

    [Tooltip("How fast the player falls, separate from their gravity when jumping")]
    [SerializeField] private float fallingGravityScale = 20f;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    // https://blog.yarsalabs.com/player-movement-with-new-input-system-in-unity/
    // Take direction from auto-generated script
    private float direction = 0;

    private bool grounded = false;

    private void Start() {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // First function to run
    private void Awake() {
        // Take player to spawn location
        transform.position = new Vector2(spawnX, spawnY);
    }

    private void JumpHandle() {
        bool jump = playerInput.actions["Jump"].WasPressedThisFrame();

        if(jump && grounded) Jump();
    }

    private void BlockSwapHandle() {
        bool blockSwap = playerInput.actions["BlockSwap"].WasPressedThisFrame();

        if(blockSwap) BlockSwap(colourOne, colourTwo);
    }

    private void BlockSwap(Colour colourOne, Colour colourTwo) {
        MapManager.instance.SwitchTileState(colourOne);
        MapManager.instance.SwitchTileState(colourTwo);
    }

    private void GravityHandle() {
        // Player is rising, gravity scale is set lower.
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

    void FixedUpdate() {
        Movement();
    }

    void Update() {
        JumpHandle();
        GravityHandle();
        BlockSwapHandle();
    }

    private void Movement() {
        //  Action from new input system
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
        //Debug.Log("Current input: "+ playerInput.actions["Move"].ReadValue<Vector2>().x);

        // plays if the player is grounded
        animator.SetBool("Walk", horizontalInput !=0);
        // Sets the speed of the animation
        animator.SetFloat("Analogue", horizontalInput);

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
        //rb.AddForce(Vector2.right * speedGround, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
    }

    //https://www.youtube.com/watch?v=P_6W-36QfLA
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            grounded = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            grounded = false;
        }
    }
}
