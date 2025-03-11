using System.Collections;
using UnityEngine;

namespace Player.StateMachine
{
    public class DashState : State
    {

        [SerializeField] private SpriteRenderer playerSprite;
        private bool _shouldGhostTrail;
        
        public override void Enter()
        {
            StateAnimator.Play("DashStart");
            DoSmearEffect();
            _shouldGhostTrail = true;
            StartCoroutine(GhostTrail());
        }

        public override void Do()
        {
            if (!StateInput.IsDashing) IsComplete = true;
        }

        public override void Exit()
        {
            _shouldGhostTrail = false;
            StopCoroutine(GhostTrail());
        }

        void DoSmearEffect() {
            playerSprite.transform.localScale = new Vector2(1.5f, 0.8f); // Estiramiento
            Invoke(nameof(ResetScale), 0.075f); // Vuelve a la escala normal tras 0.05 segundos
        }

        void ResetScale() {
            playerSprite.transform.localScale = Vector2.one;
        }
        
        IEnumerator GhostTrail() {
            while (_shouldGhostTrail) {
                GameObject smear = new GameObject("Smear");
                SpriteRenderer sr = smear.AddComponent<SpriteRenderer>();
                sr.sprite = playerSprite.sprite;
                sr.color = new Color(1f, 1f, 1f, 0.5f); // Transparente
                smear.transform.position = transform.position;
                smear.transform.localScale = StateInput.transform.localScale;
                Destroy(smear, 0.1f);
                yield return new WaitForSeconds(0.02f);
            }
        }

    }
}