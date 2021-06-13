using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.__.Scripts.PlayerScripts;

public class AbilityIconToggle : MonoBehaviour
{
    [SerializeField] private PlayerEffect ability;
    [SerializeField] private GameObject activeAbilityImage;
    [SerializeField] private GameObject inactiveAbilityImage;

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
