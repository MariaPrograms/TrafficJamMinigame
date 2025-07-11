using TMPro;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI subtitle;
    [SerializeField] MeshRenderer winnerCar;
    [SerializeField] MeshRenderer LoserCar;

    void OnEnable()
    {
        bool winner = GameManager.Instance.PlayerManager.DidPlayerWin();

        title.text = winner ? "Winner!" : "Oh no!";
        subtitle.text = winner ? "Good Job!" : "Better luck next time!";

        winnerCar.material.color = LoserCar.material.color = GameManager.Instance.PlayerManager.GetPlayerColor();
        winnerCar.gameObject.SetActive(winner);
        LoserCar.gameObject.SetActive(!winner);
    }

    public void PlayAgain()
    {
        winnerCar.gameObject.SetActive(false);
        LoserCar.gameObject.SetActive(false);
        GameManager.Instance.ResetGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
