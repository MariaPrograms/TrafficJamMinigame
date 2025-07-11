using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] private MoneyUi playerMoneyUI;
    [SerializeField] private Transform moneyUiHolder;
    [SerializeField] private Image countdownFill;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI gameStartCountdownText;

    void Start()
    {
        GameManager.Instance.AddActionOnGameStart(SetUI);//For game restart
        GameManager.Instance.OnCountdown += UpdateCountdown;
    }

    void OnEnable()
    {
        SetUI();
    }

    void Update()
    {
        //Setting Countdown Text
        if (GameManager.Instance.Playing)
        {
            countdownText.text = $"Countdown: {GameManager.Instance.roundTime - GameManager.Instance.currentTime:mm\\:ss}";
            countdownFill.fillAmount = (float)(GameManager.Instance.currentTime / GameManager.Instance.roundTime);
        }
    }

    public void UpdateCountdown(int countdown)
    {
        gameStartCountdownText.text = countdown.ToString();
        gameStartCountdownText.gameObject.SetActive(countdown > 0);
    }

    private void SetUI()
    {
        List<Player> players = GameManager.Instance.PlayerManager.Players;

        //disable any UI prefabs made when there was a higher player count
        if (moneyUiHolder.childCount > players.Count)
        {
            for (int i = moneyUiHolder.childCount - 1; i >= players.Count; i--)
            {
                moneyUiHolder.GetChild(i).gameObject.SetActive(false);
            }
        }

        //Create/Get money ui object and connect them to relevant player
        for (int i = 0; i < players.Count; i++)
        {
            MoneyUi ui;
            if (moneyUiHolder.childCount > i)
            {
                ui = moneyUiHolder.GetChild(i).GetComponent<MoneyUi>();
                ui.gameObject.SetActive(true);
            }
            else
            {
                ui = Instantiate(playerMoneyUI, moneyUiHolder);
            }
            players[i].Car.onMoneyChanged += ui.UpdateAmount;
            ui.SetUi(players[i].ColorInfo.Button);
        }
    }

    public void Pause()
    {
        GameManager.Instance.SetPauseState(true);
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        GameManager.Instance.SetPauseState(false);
        pauseMenu.SetActive(false);
    }

    public void Reset()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.ResetGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
