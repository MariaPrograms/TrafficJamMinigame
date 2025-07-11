using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Image background;
    [SerializeField] private Animation animation;
    [SerializeField] private TextMeshProUGUI moneyAddedText;

    public void SetUi(Sprite backgroundSprite)
    {
        moneyText.text = $"$0";
        background.sprite = backgroundSprite;
        moneyAddedText.color = new Color(1, 1, 1, 0);
    }

    public void UpdateAmount(int newAmount, int amountAdded)
    {
        moneyAddedText.text = $"{(amountAdded > 0 ? "+" : "-")}${FormatToKMB(Mathf.Abs(amountAdded))}";
        animation.Play();
        moneyText.text = $"${FormatToKMB(newAmount)}";
    }


    //Formatting the string to use K M and B where appropriate
    public string FormatToKMB(int num)
    {
        if (num > 999999999 || num < -999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999999 || num < -999999)
        {

            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999 || num < -999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }
}
