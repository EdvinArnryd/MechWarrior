using UnityEngine;
using Events;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventHandler.Main.PushEvent(new PauseEvent(pauseMenuUI));
        }
    }
}