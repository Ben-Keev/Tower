using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Import new input system
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Blocks")]
    [Tooltip("First map colour player may control")]
    [SerializeField] private Colour colourOne = Colour.Red;
    [Tooltip("Second map colour player may control")]
    [SerializeField] private Colour colourTwo = Colour.Blue;

    private PlayerCollision playerCollision;
    private PlayerAnimator playerAnimator;
    private PlayerPhysics pp;
    private Rigidbody2D rb;
    
    // Used by other scripts, but not inspector.
    [HideInInspector] public bool grounded;
    private bool facingRight = true;
    
    private void Start() 
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
        pp = GetComponent<PlayerPhysics>();
        playerCollision = GetComponent<PlayerCollision>();
    }

    void FixedUpdate() 
    {
        grounded = playerCollision.IsGrounded();
        playerAnimator.RisingFallingAnim(grounded, rb.velocity.y);
    }

    void Update()
    {
        pp.GravityHandle(rb);
    }

    public void BlockSwap()
    {
        AudioManager.instance.PlayPlayerSound("Switch");
        MapManager.instance.SwitchTileState(colourOne);
        MapManager.instance.SwitchTileState(colourTwo);

        // ColourTwo will be activated first. Thus, trigger its particles
        ParticleSpawner.instance.SpawnParticlesAtBlocks(colourOne);
        ParticleSpawner.instance.SpawnParticlesAtBlocks(colourTwo);
    }

    public void Movement(float horizontalInput) {
        playerAnimator.WalkAnim(horizontalInput);
        AudioManager.instance.PlayPlayerSound("Footstep");

        // Moving left when facing right or moving right while facing left
        if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight) {
            facingRight = playerAnimator.Flip(facingRight);
        }

        if (!grounded) {
            // Move the player Character slower in the air
            rb.velocity = new Vector2((horizontalInput*pp.speedGround)*pp.speedFractionAir, rb.velocity.y);
        } else { // On ground, get full movement speedGround
            rb.velocity = new Vector2(horizontalInput*pp.speedGround, rb.velocity.y);
        }
    }

    /// <summary>
    /// Adds upward force to player and sends for jump animation
    /// </summary>
    public void Jump() {
        //https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/#:~:text=The%20basic%20method%20of%20jumping,using%20the%20Add%20Force%20method.
        // Impulse applies force immediately, creating a jumping motion
        rb.AddForce(Vector2.up * pp.jumpHeight, ForceMode2D.Impulse);
        //rb.AddForce(Vector2.right * speedGround, ForceMode2D.Impulse);
        playerAnimator.JumpAnim();
        AudioManager.instance.PlayPlayerSound("Jump");
    }
}