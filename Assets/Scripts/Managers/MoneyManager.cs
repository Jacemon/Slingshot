using TMPro;
using Tools.ScriptableObjects.References;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public LongReference money;
        public TextMeshProUGUI moneyLabel;
        
        private static LongReference _money;
        
        private void Awake()
        {
            _money = money;
            ReloadMoney();
        }

        private void OnEnable()
        {
           _money.onValueChanged += ReloadMoney;
        }
        
        private void OnDisable()
        {
            _money.onValueChanged -= ReloadMoney;
        }

        public static void DepositMoney(long depositedMoney)
        {
            if (depositedMoney < 0)
            {
                return;
            }
            _money.Value += depositedMoney;
        }

        public static bool WithdrawMoney(long withdrawnMoney)
        {
            if (_money.Value < withdrawnMoney)
            {
                return false;
            }
            _money.Value -= withdrawnMoney;
            return true;
        }
        
        private void ReloadMoney()
        {
            moneyLabel.text = FormatInteger(_money.Value);
        }

        public static string FormatInteger(decimal digit)
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
