using TMPro;
using UnityEngine;

namespace Assets.__.Scripts.PlayerScripts
{
    public class ControllablePlayer : MonoBehaviour
    {
        [SerializeField] public TextMeshPro effectText;
        [HideInInspector] public int PlayerIndex;

        public void SetPlayerIndex(int playerIndex)
        {
            this.PlayerIndex = playerIndex;

            var effect = GameModel.GetInstance().IndexToPlayerEffect[PlayerIndex].ToString().ToUpper();
            //effectText.text = effect[0].ToString();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            if (collision.gameObject.tag == "Hazard")
            {
                GameObject quantumController = GameObject.FindGameObjectsWithTag("QuantumController")[0];
                QuantumControlScript quantumControlScript = quantumController.GetComponent<QuantumControlScript>();
                quantumControlScript.OnPlayerHitHazard(collision.gameObject, false);
            }

            if (collision.gameObject.tag == "DeadlyHazard")
            {
                GameObject quantumController = GameObject.FindGameObjectsWithTag("QuantumController")[0];
                QuantumControlScript quantumControlScript = quantumController.GetComponent<QuantumControlScript>();
                quantumControlScript.OnPlayerHitHazard(collision.gameObject, true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            if (collision.gameObject.tag == "Hazard")
            {
                GameObject quantumController = GameObject.FindGameObjectsWithTag("QuantumController")[0];
                QuantumControlScript quantumControlScript = quantumController.GetComponent<QuantumControlScript>();
                quantumControlScript.OnPlayerHitHazard(collision.gameObject, false);
            }

            if (collision.gameObject.tag == "DeadlyHazard")
            {
                GameObject quantumController = GameObject.FindGameObjectsWithTag("QuantumController")[0];
                QuantumControlScript quantumControlScript = quantumController.GetComponent<QuantumControlScript>();
                quantumControlScript.OnPlayerHitHazard(collision.gameObject, true);
            }

            if (collision.gameObject.tag == "WinZone")
            {
                if (GameModel.GetInstance().WinningPlayers.Contains(this) == false)
                {
                    GameModel.GetInstance().WinningPlayers.Add(this);
                }
                SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer)
                {
                    spriteRenderer.enabled = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
        }
    }
}