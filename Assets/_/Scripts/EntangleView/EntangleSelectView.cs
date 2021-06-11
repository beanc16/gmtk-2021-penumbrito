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

        private void Start()
        {
            instance = this;

            this.LoadLevel();
        }

        public void LoadLevel()
        {
            var rect = canvasTransform.rect;
            var width = rect.width * 0.5f;
            var height = (rect.height - this.bottomSpacing) * 0.5f;

            this.CreatePanel(width, height, true, true, 0);
            this.CreatePanel(width, height, false, true, 1);
            this.CreatePanel(width, height, true, false, 2);
            this.CreatePanel(width, height, false, false, 3);
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

            GameModel.GetInstance().ActivePanels.Add(false);

            newInstance.Setup(panelIndex);
        }
    }
}