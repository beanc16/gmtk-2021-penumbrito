using UnityEngine;

namespace Assets.__.Scripts.EntangleView
{
    public class EntangleSelectView : MonoBehaviour
    {
        public static EntangleSelectView Instance => instance;
        private static EntangleSelectView instance;

        [SerializeField] private EntanglePanel selectPanelPrefab;
        [SerializeField] private RectTransform canvasTransform;
        [SerializeField] private float bottomSpacing;

        private GameModel gameModel;

        private void Awake()
        {
            instance = this;

            this.gameModel = GameModel.GetInstance();
            this.LoadLevel();
        }

        public void LoadLevel()
        {
            /*var rect = canvasTransform.rect;
            var width = rect.width;
            var height = (rect.height - this.bottomSpacing) * 0.5f;

            this.CreatePanel(width, height, false, true, 0);
            this.CreatePanel(width, height, true, true, 1);*/

            gameModel.ActivePanels.Add(false);
            this.SetLightStates(0, gameModel.ActivePanels[0]);

            gameModel.ActivePanels.Add(false);
            this.SetLightStates(1, gameModel.ActivePanels[1]);
        }

        private void CreatePanel(float width, float height, bool left, bool top, int panelIndex)
        {
            var newInstance = Instantiate(this.selectPanelPrefab, this.transform);
            var rt = (RectTransform)newInstance.transform;
            rt.localPosition = new Vector3(
                width * 0.5f * (left ? 1 : -1), 
                height * 0.5f * (top ? 1 : -1), 
                0);
            rt.sizeDelta = new Vector2(width, height);

            gameModel.ActivePanels.Add(false);

            newInstance.Setup(panelIndex);

            this.SetLightStates(0, gameModel.ActivePanels[0]);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.UpdatePanel(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.UpdatePanel(1);
            }
        }

        private void UpdatePanel(int panel)
        {
            GameObject quantumController = GameObject.FindGameObjectsWithTag("QuantumController")[0];
            QuantumControlScript quantumControlScript = quantumController.GetComponent<QuantumControlScript>();
            if (!quantumControlScript.CanUpdatePanel())
            {
                return;
            }

            gameModel.ActivePanels[panel] = !gameModel.ActivePanels[panel];

            gameModel.UpdatePlayerEffect(panel, gameModel.ActivePanels[panel]);

            //imagePanel.gameObject.SetActive(gameModel.ActivePanels[this.panelNumber]);
            this.SetLightStates(panel, gameModel.ActivePanels[panel]);
            //Inform systems that need it of who is active and who is not.

            gameModel.UpdateActivePanels();
        }

        private void SetLightStates(int panel, bool active)
        {
            if (gameModel.IndexToCameraEffect.ContainsKey(panel))
            {
                gameModel.IndexToCameraEffect[panel].enabled = !active;
            }
            /*if (gameModel.IndexToLight.ContainsKey(panel))
            {
                gameModel.IndexToLight[panel].enabled = active;
            }

            if (gameModel.IndexToDark.ContainsKey(panel))
            {
                gameModel.IndexToDark[panel].enabled = !active;
            }*/
        }
    }
}