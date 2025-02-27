using UnityEngine;
using Events;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EventHandler.Main.CurrentEvent is PauseEvent pauseEvent)
            {
                EventHandler.Main.RemoveEvent(pauseEvent);
            }
            else
            {
                EventHandler.Main.PushEvent(new PauseEvent(pauseMenuUI));
            }
        }
    }
}