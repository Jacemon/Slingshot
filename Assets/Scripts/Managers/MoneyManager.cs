using Entities;
using TMPro;
using Tools.Interfaces;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour, ISavable
    {
        public long money;
        public TextMeshProUGUI moneyLabel;

        private void Awake()
        {
            GlobalEventManager.UnityEvents.OnLoad.AddListener(LoadData);
            GlobalEventManager.UnityEvents.OnSave.AddListener(SaveData);
        
            DepositMoney(0);
        }

        private void OnEnable()
        {
            GlobalEventManager.onTargetHitCart += TargetHitCart;

            GlobalEventManager.onMoneyWithdraw += WithdrawMoney;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onTargetHitCart -= TargetHitCart;
            
            GlobalEventManager.onMoneyWithdraw -= WithdrawMoney;
        }

        private void TargetHitCart(Target target)
        {
            DepositMoney(target.money);
        }

        public void ReloadData()
        {
            float digit = money;
            string[] names = { "", "K", "M", "B", "T", "Qa", "Qi" };
            var n = 0;

            while (n < names.Length - 1 && digit >= 1000)
            {
                digit /= 1000;
                n++;
            }
            moneyLabel.text = $"{digit:#0.##}{names[n]}";
        }
        
        private void DepositMoney(long depositedMoney)
        {
            if (depositedMoney < 0)
            {
                return;
            }
            money += depositedMoney;
            
            ReloadData();
        }

        private bool WithdrawMoney(long withdrawnMoney)
        {
            if (money < withdrawnMoney)
            {
                return false;
            }
            money -= withdrawnMoney;
            
            ReloadData();
            return true;
        }
        
        public void SaveData()
        {
            PlayerPrefs.SetString("money", money.ToString());
        }
        
        public void LoadData()
        {
            money = long.Parse(PlayerPrefs.GetString("money"));
            ReloadData();
            
            Debug.Log($"MoneyManager was loaded: {money}");
        }
    }
}
