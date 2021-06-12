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

            this.selectButton.onClick.AddListener(this.OnClickedPanel);
        }

        private void OnDestroy()
        {
            this.selectButton.onClick.RemoveAllListeners();
        }

        private void OnClickedPanel()
        {
            var gameModel = GameModel.GetInstance();
            gameModel.ActivePanels[this.panelNumber] = !gameModel.ActivePanels[this.panelNumber];

            //imagePanel.gameObject.SetActive(gameModel.ActivePanels[this.panelNumber]);
            this.SetLightStates(gameModel.ActivePanels[this.panelNumber]);
            //Inform systems that need it of who is active and who is not.
        }

        public void Setup(int panelNumber)
        {
            var gameModel = GameModel.GetInstance();
            this.panelNumber = panelNumber;
            imagePanel.gameObject.SetActive(false);
            this.SetLightStates(gameModel.ActivePanels[this.panelNumber]);
            //Set panel colour based on character ability?
        }

        private void SetLightStates(bool active)
        {
            var gameModel = GameModel.GetInstance();
            gameModel.IndexToLight[this.panelNumber].enabled = active;
            gameModel.IndexToDark[this.panelNumber].enabled = !active;
        }
    }
}