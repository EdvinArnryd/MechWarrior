using UnityEngine;
using Events;

public class PauseManager : MonoBehaviour
{
    //Get Set the UI in the inspector
    public GameObject pauseMenuUI;
    
    void Update()
    {
        //Trigger the pauseevent if escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Check if the current event is pauseevent, if it is, remove it
            if (EventHandler.Main.CurrentEvent is PauseEvent pauseEvent)
            {
                EventHandler.Main.RemoveEvent(pauseEvent);
            }
            else
            {
                //Add the pauseevent to the action stack
                EventHandler.Main.PushEvent(new PauseEvent(pauseMenuUI));
            }
        }
    }
}