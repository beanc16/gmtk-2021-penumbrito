using UnityEngine;
using Assets.__.Scripts.PlayerScripts;
using Assets.__.Scripts;

public class AbilityIconToggle : MonoBehaviour
{
    [SerializeField] private PlayerEffect ability;
    [SerializeField] private GameObject activeAbilityImage;
    [SerializeField] private GameObject inactiveAbilityImage;

    private GameModel gameModel;

    private void Awake()
    {
        if (activeAbilityImage != null)
        {
            Debug.LogWarning("activeAbilityImage in AbilityIconToggle not set in inspector under Ability Icons Panel");
        }

        if (inactiveAbilityImage != null)
        {
            Debug.LogWarning("inactiveAbilityImage in AbilityIconToggle not set in inspector under Ability Icons Panel");
        }

        gameModel = GameModel.GetInstance();
    }

    private void Update()
    {
        if(gameModel.ActivePlayerEffects.ContainsKey(ability))
        {
            SetActive(gameModel.ActivePlayerEffects[ability] > 0);
        }
    }

    public void SetActive(bool isActive)
    {
        // Ability is enabled
        if (isActive)
        {
            activeAbilityImage.SetActive(true);
            inactiveAbilityImage.SetActive(false);
        }

        // Ability is disabled
        else
        {
            activeAbilityImage.SetActive(false);
            inactiveAbilityImage.SetActive(true);
        }
    }
}
