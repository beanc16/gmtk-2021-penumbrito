using UnityEngine;

namespace Assets.__.Scripts.EntangleView
{
    public class MainMenuSoundController : MonoBehaviour
    {
        private void Start()
        {
            AudioController.PlayMusic("MainMenu");
            AudioController.StopMusic("Rebuilt");
            AudioController.StopMusic("Pre");
            AudioController.StopMusic("Post");
            AudioController.StopMusic("Modern");
            AudioController.StopMusic("Ambient");
        }
    }
}