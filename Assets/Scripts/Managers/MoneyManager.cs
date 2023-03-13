using System;
using Entities;
using TMPro;
using Tools.Interfaces;
using Tools.ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public LongReference money;
        public TextMeshProUGUI moneyLabel;

        private void Awake()
        {
            ReloadMoney();
        }

        private void OnEnable()
        {
            GlobalEventManager.onTargetHitCart += TargetHitCart;
            GlobalEventManager.onMoneyWithdraw += WithdrawMoney;

            money.onValueChanged += ReloadMoney;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onTargetHitCart -= TargetHitCart;
            GlobalEventManager.onMoneyWithdraw -= WithdrawMoney;

            money.onValueChanged -= ReloadMoney;
        }

        private void TargetHitCart(Target target)
        {
            DepositMoney(target.money);
        }

        private void ReloadMoney()
        {
            moneyLabel.text = FormatInteger(money.Value);
        }
        
        private void DepositMoney(long depositedMoney)
        {
            if (depositedMoney < 0)
            {
                return;
            }
            money.Value += depositedMoney;
        }

        private bool WithdrawMoney(long withdrawnMoney)
        {
            if (money.Value < withdrawnMoney)
            {
                return false;
            }
            money.Value -= withdrawnMoney;
            return true;
        }
        
        public static string FormatInteger(long digit)
        {
            string[] names = { "", "K", "M", "B", "T", "Qa", "Qi" };
            var n = 0;
            
            while (n < names.Length - 1 && digit >= 1000)
            {
                digit /= 1000;
                n++;
            }

            return $"{digit:#0.##}{names[n]}";
        }
    }
}
