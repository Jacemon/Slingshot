using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI moneyLabel;

    private void Start()
    {
        DepositMoney(0);
    }

    private void DepositMoney(int depositedMoney)
    {
        money += depositedMoney;
        moneyLabel.text = money.ToString();
    }

    private void WithdrawMoney(int withdrawnMoney)
    {
        if (money < withdrawnMoney)
        {
            return;
        }
        money -= withdrawnMoney;
        moneyLabel.text = money.ToString();
    }
}
