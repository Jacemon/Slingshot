using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI moneyLabel;

    private void Start()
    {
        AddMoney(0);
    }

    private void AddMoney(int additionalMoney)
    {
        money += additionalMoney;
        moneyLabel.text = money.ToString();
    }
}
