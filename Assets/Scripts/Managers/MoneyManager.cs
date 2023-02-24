using Entities;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public int money; // < uint < long < ulong < BigInteger
        public TextMeshProUGUI moneyLabel;

        private void Awake()
        {
            GlobalEventManager.OnTargetHitCart.AddListener(TargetHitCart);
            DepositMoney(0);
        }
        
        private void TargetHitCart(Target target)
        {
            DepositMoney(target.money);
            Destroy(target.gameObject);
        }

        public bool DepositMoney(int depositedMoney)
        {
            if (depositedMoney < 0)
            {
                return false;
            }
            money += depositedMoney;
            
            var moneyString = money switch
            {
                >= 1000000 => $"{money / 1000000.0f:F1}M",
                >= 1000 => $"{money / 1000.0f:F1}K",
                _ => money.ToString()
            };

            moneyLabel.text = moneyString;
            return true;
        }

        public bool WithdrawMoney(int withdrawnMoney)
        {
            if (money < withdrawnMoney)
            {
                return false;
            }
            money -= withdrawnMoney;
            
            var moneyString = money switch
            {
                > 1000000 => $"{money / 1000000.0f:F1}M",
                > 1000 => $"{money / 1000.0f:F1}K",
                _ => money.ToString()
            };
            
            moneyLabel.text = moneyString;
            return true;
        }
    }
}
