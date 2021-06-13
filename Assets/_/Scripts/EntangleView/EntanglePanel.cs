using Assets.__.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__.Scripts.EntangleView
{
    [RequireComponent(typeof(Button))]
    public class EntanglePanel : MonoBehaviour
    {
        private Button selectButton;
        private Image imagePanel;
        private int panelNumber;

        private void Awake()
        {
            this.selectButton = this.GetComponent<Button>();
            this.imagePanel = this.GetComponentInChildren<Image>();
        }

        public void Setup(int panelNumber)
        {
            this.panelNumber = panelNumber;
            imagePanel.gameObject.SetActive(false);
        }
    }
}