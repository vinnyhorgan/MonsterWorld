using System;
using Raylib_cs;

namespace MonsterWorld.Transitions
{
    enum TransitionState
    {
        In,
        Out
    }

    class Transition
    {
        private float _halfDuration;
        private float _timer = 0.0f;
        private TransitionState _state = TransitionState.Out;

        public Transition(float duration)
        {
            _halfDuration = duration / 2f;
        }

        public event EventHandler StateChanged;
        public event EventHandler Completed;

        public float Value
        {
            get { return Raymath.Clamp(_timer / _halfDuration, 0.0f, 1.0f); }
        }

        public virtual void Update(float dt)
        {
            if (_state == TransitionState.Out)
            {
                _timer += dt;

                if (_timer >= _halfDuration)
                {
                    _state = TransitionState.In;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                _timer -= dt;

                if (_timer <= 0.0f)
                {
                    Completed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public virtual void Draw() {}
    }
}
