using UnityEngine;
using Events;

public class PauseUIManager : MonoBehaviour
{
    public void ResumeGame()
    {
        if (EventHandler.Main.CurrentEvent is PauseEvent pauseEvent)
        {
            pauseEvent.OnEnd();
            EventHandler.Main.RemoveEvent(pauseEvent);
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time before quitting
        UnityEditor.EditorApplication.ExitPlaymode();
       // Application.Quit();
    }
}