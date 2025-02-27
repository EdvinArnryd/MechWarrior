using UnityEngine;
using Events;

public class PauseEvent : EventHandler.GameEvent
{
    private GameObject pauseMenuUI;

    public PauseEvent(GameObject ui)
    {
        pauseMenuUI = ui;
    }

    public override void OnBegin(bool bFirstTime)
    {
        Time.timeScale = 0f; // Pause game
        pauseMenuUI.SetActive(true); // Show UI

        // Unlock and show cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void OnEnd()
    {
        Time.timeScale = 1f; // Resume game
        pauseMenuUI.SetActive(false); // Hide UI

        // Lock and hide cursor when resuming
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public override bool IsDone()
    {
        return false;
    }
}