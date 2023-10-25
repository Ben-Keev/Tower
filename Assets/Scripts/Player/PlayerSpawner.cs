using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private float spawnX = -4.5f;
    [SerializeField] private float spawnY = -3;
    [Header("Death")]
    [SerializeField] private GameObject dieParticlePrefab;

    public float spawnDelay;

    // For deaths
    private SpriteRenderer spriteRender;
    private PlayerInputHandler input;
    private Rigidbody2D rb;
    private PlayerCollision collision;

    public bool dead;

    private void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputHandler>();
        collision = GetComponent<PlayerCollision>();
    }

    private void Awake()
    {
        // Take player to spawn location
        transform.position = new Vector2(spawnX, spawnY);
    }

    private void FixedUpdate()
    {

        // You can only die if you're not dead.
        if(!dead)
        {   // Checks suffocation. If yes, die and respawn.
            if (collision.IsSuffocated()) StartCoroutine(DieAndRespawn());
        }
    }

    /// <summary>
    /// Re-enables the renderer, input, gravity and teleports the player to their spawn location.
    /// </summary>
    private void Spawn()
    {
        // Enable sprite
        spriteRender.enabled = true;

        // Enable input
        input.enabled = true;

        // Enable gravity
        // https://stackoverflow.com/questions/41264316/setting-rigidbody2d-body-type-to-static-in-code
        rb.bodyType = RigidbodyType2D.Dynamic;

        transform.position = new Vector2(spawnX, spawnY);

        // Alive again!
        dead = !dead;
    }

    /// <summary>
    /// Makes sprite appear as though it's disappeared. Disables input and gravity and plays particle effects.
    /// </summary>
    private void Die()
    {
        dead = true;

        AudioManager.instance.PlayPlayerSound("Die");

        // Disable sprite
        spriteRender.enabled = false;

        // Disable input
        input.enabled = false;

        // Disable gravity
        // https://stackoverflow.com/questions/41264316/setting-rigidbody2d-body-type-to-static-in-code
        rb.bodyType = RigidbodyType2D.Static;

        ParticleSpawner.instance.SpawnParticleAtWorldPosition(dieParticlePrefab, Color.white, transform.position);
    }

    // https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    /// <summary>
    /// A coroutine which respawns the player after a delay
    /// </summary>
    /// <returns>N/A</returns>
    IEnumerator DieAndRespawn()
    {
        Die();
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
    }
}
