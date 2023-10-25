using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutscene : MonoBehaviour
{
    PlayerController controller;
    //  Determines whether cutscene was already called
    bool called = false;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Scripted movement of the player during the main menu's cutscene.
    /// </summary>
    /// <returns>N/A</returns>
    IEnumerator CutsceneMovement()
    {
        controller.Movement(1);
        yield return new WaitForSeconds(0.5f);
        controller.Jump();
    }

    /// <summary>
    /// Initiates player's scripted movements after pressing play. Then loads game after a delay.
    /// </summary>
    public void PlayCutscene()
    {
        if (!called)
        {
            called = true;
            StartCoroutine(CutsceneMovement());
            StartCoroutine(LoadAfterDelay());
        }
    }

    /// <summary>
    /// On cutscene finished, load game
    /// </summary>
    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(4);
        GameManager.instance.LoadLevel();
    }
}
