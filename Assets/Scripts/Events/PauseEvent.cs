using UnityEngine;

namespace Events

{
    public class PauseEvent : EventHandler.GameEvent
    {
        // Get the UI Panel
        private GameObject pauseMenuUI;

        public PauseEvent(GameObject ui)
        {
            // Set UI
            pauseMenuUI = ui;
        }

        public override void OnBegin(bool bFirstTime)
        {
            //set timescale to 0 to pause the game
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
        }

        public override void OnEnd()
        {
            //Start unpause the game
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
        }


        public override bool IsDone()
        {
            //return false to not automatically remove itself
            return false;
        }
    }
}