using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://blog.yarsalabs.com/player-movement-with-new-input-system-in-unity
public class PlayerInputHandler : MonoBehaviour
{   
    private PlayerController controller;
    private PlayerInput playerInput;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Waits for Jump input from new input system. Triggers jump.
    /// </summary>
    private void JumpHandle()
    {
        bool jump = playerInput.actions["Jump"].WasPressedThisFrame();

        if (jump && controller.grounded) controller.Jump();
    }

    /// <summary>
    /// Waits for blockswap input from new input system. Triggers blockswap.
    /// </summary>
    private void BlockSwapHandle()
    {
        bool blockSwap = playerInput.actions["BlockSwap"].WasPressedThisFrame();

        if (blockSwap) controller.BlockSwap();
    }

    /// <summary>
    /// Waits for pause input from new input system.
    /// </summary>
    private void PauseHandle()
    {
        bool pause = playerInput.actions["Pause"].WasPressedThisFrame();

        if (pause) GameManager.instance.Pause();
    }

    /// <summary>
    /// Waits for movement float from new input system. Triggers movement
    /// </summary>
    private void MovementHandle()
    {
        float horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
        
        controller.Movement(horizontalInput);
    }

    private void FixedUpdate()
    {
        MovementHandle();
    }

    private void Update()
    {
        BlockSwapHandle();
        JumpHandle();
        PauseHandle();
    }
}
