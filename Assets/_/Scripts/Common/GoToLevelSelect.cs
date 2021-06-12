using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GoToLevelSelect : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        this.button = this.GetComponent<Button>();
        this.button.onClick.AddListener(this.OnMainMenu);
    }

    private void OnDestroy()
    {
        this.button.onClick.RemoveAllListeners();
    }

    private void OnMainMenu()
    {
        SceneHandler.LoadScene("LevelSelect");
    }
}
