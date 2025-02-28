using UnityEngine;
using Events;

namespace Events
{

    public class SlowMotionEvent : EventHandler.GameEvent
    {
        private float slowMotionScale;
        private float timer;

        public SlowMotionEvent(float duration, float slowMotionScale = 0.2f)
        {
            this.slowMotionScale = slowMotionScale;
            timer = duration;
        }

        public override void OnBegin(bool bFirstTime)
        {
            //Activate slowmotion
            Time.timeScale = slowMotionScale;
        }

        public override void OnUpdate()
        {
            //Use unscaled time when timeScale is reduced
            timer -= Time.unscaledDeltaTime;
        }

        public override void OnEnd()
        {
            //Reset time to 1 when event is finished
            Time.timeScale = 1f;
        }

        public override bool IsDone()
        {
            //Returns true when timer is finished
            return timer <= 0;
        }
    }
}