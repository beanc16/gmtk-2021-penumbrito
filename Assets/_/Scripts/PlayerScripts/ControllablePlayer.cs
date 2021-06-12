using UnityEngine;

namespace Assets.__.Scripts.PlayerScripts
{
    public class ControllablePlayer : MonoBehaviour
    {
        public int PlayerIndex;

        void OnCollisionEnter2D(Collision2D collision)
        {
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            if (collision.gameObject.tag == "Hazard")
            {
                SceneHandler.RestartCurrentScene();
            }
        }
    }
}