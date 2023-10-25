using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    // To check if player is dead
    private PlayerSpawner spawner;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spawner = GetComponent<PlayerSpawner>();
    }

    /// <summary>
    /// Triggers Walk animation
    /// </summary>
    /// <param name="horizontalInput">Horizontal Input as a float affects the speed of the animation</param>
    public void WalkAnim(float horizontalInput)
    {
        // plays if the player is grounded
        animator.SetBool("Walk", horizontalInput != 0);

        // Sets the speed of the animation
        animator.SetFloat("Analogue", horizontalInput);
    }

    /// <summary>
    /// Triggers Jump Animation
    /// </summary>
    public void JumpAnim()
    {
        animator.SetTrigger("Jump");
    }

    /// <summary>
    /// Sets when jumping animation should be interrupted, or when Y is decreasing
    /// </summary>
    /// <param name="grounded">Whether grounded</param>
    /// <param name="velocityY">Speed of Y movement</param>
    public void RisingFallingAnim(bool grounded, float velocityY)
    {
        animator.SetBool("Rising", velocityY > 0);

        // Play bump sound effect. As dying disables velocity, we must also check player is alive.
        if (!grounded && velocityY == 0 && !spawner.dead)
        AudioManager.instance.PlayPlayerSound("Bump");

        // Continues falling animation while not grounded.
        animator.SetBool("Falling", !grounded && velocityY == 0);

        animator.SetBool("Grounded", grounded);
    }

    /// <summary>
    /// Flips the player by inverting its scale on the x axis.
    /// </summary>
    /// <param name="facingRight">Direction player is facing</param>
    /// <returns>facingRight of opposing direction</returns>
    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/RefactoredAdvancedPlayerMovement.cs
    public bool Flip(bool facingRight)
    {
        // Flip the player's scale on the x axis
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        return !facingRight;
    }
}