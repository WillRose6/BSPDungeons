using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonUI : MainUI
{
    [Header("Startup screen")]
    public TextMeshProUGUI introLevelText;
    public TextMeshProUGUI introAttemptsText;

    [Header("Game Over")]
    public GameObject GameOverScreen;

    [Header("Health bar")]
    [SerializeField]
    private Image healthBarImage;
    public Animator healthBarAnim;
    public Image[] hearts;
    private List<Image> heartsInUse = new List<Image>();
    private Image currentHeart;
    private float[] Boundaries;
    [SerializeField]
    private int healthPerHeart;

    [Header("XP bars")]
    public Image[] xpBars;

    [Header("Level Complete Screen")]
    [SerializeField]
    public GameObject LevelCompleteScreen;

    public override void Begin(float startHealth)
    {
        EnableHearts(startHealth);
        currentHeart = heartsInUse[heartsInUse.Count - 1];
        Boundaries = new float[heartsInUse.Count];

        float segments = ((float)1 / Boundaries.Length);
        for (int i = 0; i < Boundaries.Length; i++)
        {
            Boundaries[i] = (i * segments);
        }
    }

    public void EnableHearts(float health)
    {
        int amountOfHearts = Mathf.RoundToInt(health / healthPerHeart);
        for (int i = 0; i < amountOfHearts; i++)
        {
            if (hearts[i])
            {
                hearts[i].gameObject.SetActive(true);
                heartsInUse.Add(hearts[i]);
            }
            else
            {
                Debug.LogError("Not enough hearts!");
            }
        }
    }

    public void UpdatePlayerHealthBar(float startHealth, float currentHealth)
    {
        float amountToRemoveFromHeart = 0;
        UpdateHearts(currentHealth / startHealth, ref amountToRemoveFromHeart);
        currentHeart.fillAmount = amountToRemoveFromHeart;
    }

    public void UpdateHearts(float healthPercentage, ref float amountToRemoveFromHeart)
    {
        for (int i = 0; i < Boundaries.Length; i++)
        {
            if (healthPercentage > Boundaries[i])
            {
                currentHeart = heartsInUse[i];
                amountToRemoveFromHeart = (healthPercentage - Boundaries[i]) * 3;
            }
            else
            {
                heartsInUse[i].fillAmount = 0;
            }
        }
    }

    public void FlashDamage()
    {
        healthBarAnim.Play("Blood_Flash");
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void SetStartupScreen(int levelNum, int attempt)
    {
        introLevelText.SetText("Floor " + levelNum);
        introAttemptsText.SetText("Attempt " + attempt);
    }


    public void ShowLevelCompleteScreen()
    {
        LevelCompleteScreen.SetActive(true);
    }
}
