using UnityEngine;
using Events;

public class PauseEvent : EventHandler.GameEvent
{
    private GameObject pauseMenuUI;
    private bool isPaused = false;

    public PauseEvent(GameObject ui)
    {
        pauseMenuUI = ui;
    }

    public override void OnBegin(bool bFirstTime)
    {
        if (!isPaused)
        {
            Time.timeScale = 0f; // Pause game
            pauseMenuUI.SetActive(true); // Show UI
            isPaused = true;
            Debug.Log("Game Paused");
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Unpause on ESC
        {
            isPaused = false;
        }
    }

    public override void OnEnd()
    {
        Time.timeScale = 1f; // Resume game
        pauseMenuUI.SetActive(false); // Hide UI
        Debug.Log("Game Resumed");
    }

    public override bool IsDone()
    {
        return !isPaused; // Event ends when unpaused
    }
}
