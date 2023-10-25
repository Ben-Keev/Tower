using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // https://github.com/naoisecollins/2023GD2a-PlayerController/blob/main/Assets/Scripts/GameManager.cs
    public static GameManager instance;

    [HideInInspector] public bool isPaused;
    [SerializeField] private Canvas pauseMenu;

    private void initSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Gets pause menu object
    // https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html
    private void initPause()
    {   
        // Find empty containing canvas
        GameObject pauseTag = GameObject.FindWithTag("Pause");
        // Store component
        if (pauseTag != null) pauseMenu = pauseTag.GetComponent<Canvas>();
    }

    private void Awake()
    {
        initSingleton();
    }

    // Loads after audio instance initilised.
    private void Start()
    {
        AudioManager.instance.PlayBackgroundWind();
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.enabled = true;
    }

    public void Resume()
    {
        isPaused = true;
        Time.timeScale = 1f;
        pauseMenu.enabled = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Runs on main menu
    public void LoadLevel()
    {
        AudioManager.instance.StopAudio();
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    // https://discussions.unity.com/t/since-onlevelwasloaded-is-deprecated-in-5-4-0b15-what-should-be-use-instead/163521/3

    // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html

}
