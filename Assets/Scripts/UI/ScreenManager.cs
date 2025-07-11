using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] GameObject gameplayScreen;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endGameScreen;

    [SerializeField] GameObject gameplayCam;
    [SerializeField] GameObject screenCam;

    GameManager manager => GameManager.Instance;

    void Start()
    {
        Restart();
        manager.AddActionOnGameStart(GameStart);
        manager.AddActionOnGameEnd(GameEnd);
        manager.AddActionOnReset(Restart);
    }

    private void Restart()
    {
        gameplayCam.SetActive(false);
        screenCam.SetActive(true);

        startScreen.SetActive(true);
        gameplayScreen.SetActive(false);
        endGameScreen.SetActive(false);
    }

    private void GameStart()
    {
        gameplayCam.SetActive(true);
        screenCam.SetActive(false);

        gameplayScreen.SetActive(true);
        startScreen.SetActive(false);
        endGameScreen.SetActive(false);
    }

    private void GameEnd()
    {
        gameplayCam.SetActive(false);
        screenCam.SetActive(true);

        startScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        endGameScreen.SetActive(true);
    }

}

