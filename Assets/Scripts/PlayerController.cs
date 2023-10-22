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

    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/RefactoredAdvancedPlayerMovement.cs
    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private bool grounded = false;

    [Header("Gravity")]
    
    [SerializeField] private float gravityScale = 10f;

    [Tooltip("How fast the player falls, separate from their gravity when jumping")]
    [SerializeField] private float fallingGravityScale = 20f;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    // https://blog.yarsalabs.com/player-movement-with-new-input-system-in-unity

    private bool facingRight = true;
    

    private void Start() {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // First function to run
    private void Awake() {
        // Take player to spawn location
        transform.position = new Vector2(spawnX, spawnY);
        // Adjust groundCheckpoint
        
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

        // ColourTwo will be activated first. Thus, trigger its particles
        ParticleSpawner.instance.SpawnParticle(colourOne);
        ParticleSpawner.instance.SpawnParticle(colourTwo);
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
        grounded = IsGrounded();
        Movement();
    }

    void Update() {
        JumpHandle();
        GravityHandle();
        BlockSwapHandle();
        animator.SetBool("Rising", rb.velocity.y > 0);
    }

    private void Movement() {
        //  Action from new input system
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
        
        // Moving left when facing right or moving right while facing left
        if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight) {
            Flip();
        }

        // plays if the player is grounded
        animator.SetBool("Walk", horizontalInput !=0);
        // Sets the speed of the animation
        animator.SetFloat("Analogue", horizontalInput);

        animator.SetBool("Falling", !grounded);

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

    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/RefactoredAdvancedPlayerMovement.cs
    private void Flip() {
        // Flip the player's scale on the x axis
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/RefactoredAdvancedPlayerMovement.cs
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }

    //https://www.youtube.com/watch?v=P_6W-36QfLA
    //private void OnCollisionEnter2D(Collision2D other) {
    //    if (other.gameObject.CompareTag("Ground")) {
    //        grounded = true;
    //    }
    //}
    
    //private void OnCollisionExit2D(Collision2D other) {
    //    if (other.gameObject.CompareTag("Ground")) {
    //        grounded = false;
    //    }
    //}
}
