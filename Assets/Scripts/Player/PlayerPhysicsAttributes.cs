using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] public float speedGround = 5f;
    [Tooltip("Fraction that ground speed will be reduced by when in air")]
    [SerializeField] public float speedFractionAir = 0.75f;

    [Header("Jump")]
    [SerializeField] public float jumpHeight = 10f;

    [Header("Gravity")]
    [SerializeField] public float gravityScale = 2.6f;

    [Tooltip("How fast the player falls, separate from their gravity when jumping")]
    [SerializeField] public float fallingGravityScale = 3.5f;

    public void GravityHandle(Rigidbody2D rb)
    {
        // Player is rising, gravity scale is set lower.
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }

        // Player is falling, gravity scale is set higher.
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
    }

}
