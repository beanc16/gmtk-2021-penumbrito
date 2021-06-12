using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.__.Scripts.EntangleView
{
    public class LightSourceSubscriber : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private bool active;

        private void Awake()
        {
            if (active)
            {
                GameModel.GetInstance().IndexToLight.Add(index, this.GetComponent<Light2D>());
            }
            else
            {
                GameModel.GetInstance().IndexToDark.Add(index, this.GetComponent<Light2D>());
            }
        }
    }
}