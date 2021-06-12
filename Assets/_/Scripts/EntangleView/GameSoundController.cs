using UnityEngine;

namespace Assets.__.Scripts.EntangleView
{
    public class GameSoundController : MonoBehaviour
    {
        private GameModel gameModel;

        private void Start()
        {
            gameModel = GameModel.GetInstance();
            AudioController.StopMusic("MainMenu");

            AudioController.PlayMusic("Modern");
            AudioController.PlayMusic("Post");
            AudioController.PlayMusic("Pre");
            AudioController.PlayMusic("Rebuilt");
        }

        private void Update()
        {
            AudioController.MuteMusic("Modern", gameModel.ActivePanels[0]);
            AudioController.MuteMusic("Post", gameModel.ActivePanels[1]);
            AudioController.MuteMusic("Pre", gameModel.ActivePanels[2]);
            AudioController.MuteMusic("Rebuilt", gameModel.ActivePanels[3]);
        }
    }
}