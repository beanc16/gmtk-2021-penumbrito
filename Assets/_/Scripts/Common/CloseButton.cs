using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CloseButton : MonoBehaviour
{
    [SerializeField] private GameObject thingToClose;
    [SerializeField] private bool destroyThing;

    private Button button;

    private void Start()
    {
        this.button = this.GetComponent<Button>();
        this.button.onClick.AddListener(this.OnClose);
    }

    private void OnDestroy()
    {
        this.button.onClick.RemoveAllListeners();
    }

    private void OnClose()
    {
        if (this.destroyThing)
        {
            Destroy(this.thingToClose);
        }
        else
        {
            this.thingToClose.SetActive(false);
        }
    }
}
