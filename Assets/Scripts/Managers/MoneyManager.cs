using Entities;
using TMPro;
using Tools.Interfaces;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour, ISavable
    {
        public int money; // < uint < long < ulong < BigInteger
        public TextMeshProUGUI moneyLabel;

        private void Awake()
        {
            GlobalEventManager.OnTargetHitCart.AddListener(TargetHitCart);
            
            GlobalEventManager.OnSave.AddListener(SaveData);
            GlobalEventManager.OnLoad.AddListener(LoadData);
            
            DepositMoney(0);
        }
        
        private void TargetHitCart(Target target)
        {
            DepositMoney(target.money);
            Destroy(target.gameObject);
        }

        public void ReloadData()
        {
            moneyLabel.text = money switch
            {
                >= 1000000 => $"{money / 1000000.0f:F1}M",
                >= 1000 => $"{money / 1000.0f:F1}K",
                _ => money.ToString()
            };
        }
        
        public bool DepositMoney(int depositedMoney)
        {
            if (depositedMoney < 0)
            {
                return false;
            }
            money += depositedMoney;
            
            ReloadData();
            return true;
        }

        public bool WithdrawMoney(int withdrawnMoney)
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
            PlayerPrefs.SetInt("money", money);
        }
        
        public void LoadData()
        {
            money = PlayerPrefs.GetInt("money");
            ReloadData();
            
            Debug.Log("MoneyManager was loaded");
        }
    }
}
