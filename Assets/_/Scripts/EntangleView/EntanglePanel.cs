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
<<<<<<< HEAD

            this.selectButton.onClick.AddListener(this.OnClickedPanel);
        }

        private void OnDestroy()
        {
            this.selectButton.onClick.RemoveAllListeners();
        }

        private void OnClickedPanel()
        {
            GameObject quantumController = GameObject.FindGameObjectsWithTag("QuantumController")[0];
            QuantumControlScript quantumControlScript = quantumController.GetComponent<QuantumControlScript>();
            if (!quantumControlScript.CanUpdatePanel())
            {
                return;
            }

            var gameModel = GameModel.GetInstance();
            gameModel.ActivePanels[this.panelNumber] = !gameModel.ActivePanels[this.panelNumber];

            gameModel.UpdatePlayerEffect(this.panelNumber, gameModel.ActivePanels[this.panelNumber]);

            //imagePanel.gameObject.SetActive(gameModel.ActivePanels[this.panelNumber]);
            this.SetLightStates(gameModel.ActivePanels[this.panelNumber]);
            //Inform systems that need it of who is active and who is not.

            gameModel.UpdateActivePanels();
=======
>>>>>>> 86d0e030d9d2b1c13a517e267fd56038c2c997f5
        }

        public void Setup(int panelNumber)
        {
            this.panelNumber = panelNumber;
            imagePanel.gameObject.SetActive(false);
        }
    }
}