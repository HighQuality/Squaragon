using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Modes
{
    abstract class Mode
    {
        public MainScene Scene;
        private Action<float> update;
        protected Action onStateChanged;
        public float Difficulty = 1.5f;

        public Mode(MainScene scene)
        {
            this.Scene = scene;
        }

        public void TriggerUpdate(float deltaTime)
        {
            if (update != null)
                update(deltaTime);
        }

        public void ChangeState(Action<float> state)
        {
            if (onStateChanged != null)
            {
                onStateChanged();
                onStateChanged = null;
            }
            update = state;
        }

        public void ChangeState()
        {
            if (onStateChanged != null)
                onStateChanged();
        }
    }
}
