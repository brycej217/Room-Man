using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Player player;
    private Weapon weapon;

    [Header("Health")]
    [SerializeField]
    private Image health;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI weaponText;

    private void Awake()
    {
        player = Player.Instance;
    }

    private void OnEnable()
    {
        player.OnHealthChange += OnHealthUpdate;
        player.OnWeaponSwap += OnWeaponSwapUpdate;
    }

    private void OnDisable()
    {
        player.OnHealthChange -= OnHealthUpdate;
        player.OnWeaponSwap -= OnWeaponSwapUpdate;
    }

    private void Start()
    {
        healthText.text = player.maxHealth.ToString();
    }

    private void OnWeaponSwapUpdate()
    {
        weapon = player.currentWeapon;
        if (weapon)
        {
            weapon.OnClipChange += OnClipUpdate;
        }
        weaponText.text = weapon.currentClipSize.ToString();
    }

    private void OnHealthUpdate()
    {
        health.fillAmount = player.currentHealth / player.maxHealth;
        healthText.text = player.currentHealth.ToString();
    }

    private void OnClipUpdate()
    {
        weaponText.text = weapon.currentClipSize.ToString();
    }
}
