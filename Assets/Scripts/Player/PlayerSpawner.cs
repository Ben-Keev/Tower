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
    private SpriteRenderer renderer;
    private PlayerInputHandler input;
    private Rigidbody2D rb;
    private PlayerCollision collision;

    private bool dead;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
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
        {
            if (collision.IsSuffocated()) StartCoroutine(DieAndRespawn());
        }
    }

    // It's not a spawn per se, just a reset of values.
    private void Spawn()
    {
        // Enable sprite
        renderer.enabled = true;

        // Enable input
        input.enabled = true;

        // Enable gravity
        // https://stackoverflow.com/questions/41264316/setting-rigidbody2d-body-type-to-static-in-code
        rb.bodyType = RigidbodyType2D.Dynamic;

        transform.position = new Vector2(spawnX, spawnY);

        dead = !dead;
    }

    private void Die()
    {
        dead = true;

        // Disable sprite
        renderer.enabled = false;

        // Disable input
        input.enabled = false;

        // Disable gravity
        // https://stackoverflow.com/questions/41264316/setting-rigidbody2d-body-type-to-static-in-code
        rb.bodyType = RigidbodyType2D.Static;

        ParticleSpawner.instance.SpawnParticleAtWorldPosition(dieParticlePrefab, Color.white, transform.position);
    }

    // https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    IEnumerator DieAndRespawn()
    {
        Die();
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
    }
}
