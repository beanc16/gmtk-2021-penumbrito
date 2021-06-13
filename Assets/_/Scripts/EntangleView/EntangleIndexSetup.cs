using Assets.__.Scripts.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Assets.__.Scripts.EntangleView
{
    public class EntangleIndexSetup : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private PlayerEffect playerEffect;

        // Use this for initialization
        private void Awake()
        {
            var lights = GetComponentsInChildren<LightSourceSubscriber>();
            foreach(var light in lights)
            {
                light.Setup(this.index);
            }

            var lightTargets = GetComponentsInChildren<LightSourceTarget>();
            foreach (var target in lightTargets)
            {
                var spriteRendere = target.GetComponent<SpriteRenderer>();
                spriteRendere.sortingLayerName = "Zone" + (index + 1);
            }

            GameModel.GetInstance().IndexToPlayerEffect.Add(index, playerEffect);

            //GetComponentInChildren<TextMeshPro>().sortingLayerID = SortingLayer.NameToID("Zone" + (index + 1));

            var player = GetComponentInChildren<ControllablePlayer>();
            player.SetPlayerIndex(index);
        }
    }
}