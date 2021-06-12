﻿using TMPro;
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
            effectText.text = effect[0].ToString();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            if (collision.gameObject.tag == "Hazard")
            {
                SceneHandler.RestartCurrentScene();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "WinZone")
            {
                GameModel.GetInstance().CountInWinZone++;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "WinZone")
            {
                GameModel.GetInstance().CountInWinZone--;
            }
        }
    }
}