using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/RefactoredAdvancedPlayerMovement.cs
    
    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [HideInInspector] public bool suffocated = false;

    // Size from which the player may die.
    // https://www.youtube.com/watch?v=jxCVHBMdTWo

    [Header("Death Bounds")]
    public Vector2 deathBoxSize;
    // Center of the death box
    public Transform deathBoxCenter;

    // Overlapcircle checks if grounded
    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/RefactoredAdvancedPlayerMovement.cs
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    public bool IsSuffocated()
    {
        suffocated = Physics2D.OverlapBox(deathBoxCenter.position, deathBoxSize, 0, groundLayer);
        return suffocated;
    }

    // Draws deathbox in unity editor
    //https://www.youtube.com/watch?v=jxCVHBMdTWo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(deathBoxCenter.position, deathBoxSize);
    }
}
