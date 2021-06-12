using UnityEngine;

namespace Assets.__.Scripts.Common
{
    public class QuitToMainMenu : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneHandler.LoadScene("MainMenu");
            }
        }
    }
}