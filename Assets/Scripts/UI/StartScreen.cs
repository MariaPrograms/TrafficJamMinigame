using System;
using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown colorDropdown;
    [SerializeField] private TextMeshProUGUI enemyAmountText;

    private PlayerManager playerManager => GameManager.Instance.PlayerManager;

    void Start()
    {
        foreach (var color in GameManager.Instance.ColorsInUse.colorsToUse)
        {
            colorDropdown.options.Add(new TMP_Dropdown.OptionData(color.Name, color.Button, Color.white));
        }
        colorDropdown.value = colorDropdown.options.FindIndex(a => a.text == playerManager.PlayerColor);
        UpdateEnemyText();
    }

    public void SetPlayerColor(Int32 index)
    {
        playerManager.SetPlayerColor(colorDropdown.options[index].text);
    }

    public void AddEnemy()
    {
        playerManager.UpdateEnemyCount(1);
        UpdateEnemyText();
    }

    public void RemoveEnemy()
    {
        playerManager.UpdateEnemyCount(-1);
        UpdateEnemyText();
    }

    private void UpdateEnemyText()
    {
        enemyAmountText.text = playerManager.EnemyCount.ToString();
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
