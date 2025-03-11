using UnityEngine;

namespace Player.StateMachine
{
    public class DashState : State
    {
        public override void Enter()
        {
            StateAnimator.Play("DashStart");
            // DoSmearEffect();
        }

        public override void Do()
        {
            if (!StateInput.IsDashing) IsComplete = true;
        }
        
        void DoSmearEffect() {
            StateInput.transform.localScale = new Vector2(1.5f, 0.8f); // Estiramiento
            Invoke(nameof(ResetScale), 0.075f); // Vuelve a la escala normal tras 0.05 segundos
        }

        void ResetScale() {
            StateInput.transform.localScale = Vector2.one;
        }
    }
}