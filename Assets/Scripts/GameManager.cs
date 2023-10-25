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

    // https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html
    /// <summary>
    /// Gets pause GameObject when in a game level.
    /// </summary>
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
    }

    /// <summary>
    /// Stops time an enables the pauseMenu canvas.
    /// </summary>
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.enabled = true;
    }

    /// <summary>
    /// Resumes time and disables the pauseMenu canvas.
    /// </summary>
    public void Resume()
    {
        isPaused = true;
        Time.timeScale = 1f;
        pauseMenu.enabled = false;
    }

    /// <summary>
    /// Loads main menu
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Stops audio from main menu and loads into the gamescene
    /// </summary>
    public void LoadLevel()
    {
        AudioManager.instance.StopAudio();
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

}
