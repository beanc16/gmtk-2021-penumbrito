using UnityEngine;
using Assets.__.Scripts;
using UnityEngine.UI;

public class NextLevelIcon : MonoBehaviour
{
    [SerializeField] private GameObject activeLevelImage;
    [SerializeField] private GameObject inactiveLevelImage;
    [SerializeField] private Button button;

    private GameModel gameModel;

    private void Awake()
    {
        if (activeLevelImage == null)
        {
            Debug.LogWarning("activeAbilityImage in AbilityIconToggle not set in inspector under Ability Icons Panel");
        }

        if (inactiveLevelImage == null)
        {
            Debug.LogWarning("inactiveAbilityImage in AbilityIconToggle not set in inspector under Ability Icons Panel");
        }

        gameModel = GameModel.GetInstance();
    }

    private void Update()
    {
        SetActive(gameModel.GameWon);
    }

    public void OnClick()
    {
        SceneHandler.LoadNextLevel();
    }

    public void SetActive(bool isActive)
    {
        button.interactable = isActive;
        // Ability is enabled
        if (isActive)
        {
            activeLevelImage.SetActive(true);
            inactiveLevelImage.SetActive(false);
        }

        // Ability is disabled
        else
        {
            activeLevelImage.SetActive(false);
            inactiveLevelImage.SetActive(true);
        }
    }
}
