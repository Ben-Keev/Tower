using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Import new input system
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 10f;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;

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
    }

    private void Movement() {
        //  Action from new input system
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;

        // Move the player Character
        rb.velocity = new Vector2(horizontalInput*speed, rb.velocity.y);

        animator.SetBool("Walk", horizontalInput !=0);
    }

    private void Jump() {
        //https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/#:~:text=The%20basic%20method%20of%20jumping,using%20the%20Add%20Force%20method.
        // Impulse applies force immediately, creating a jumping motion
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
    }


}
