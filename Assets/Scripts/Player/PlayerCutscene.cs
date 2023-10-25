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

    IEnumerator CutsceneMovement()
    {
        controller.Movement(1);
        yield return new WaitForSeconds(0.5f);
        controller.Jump();
    }

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
