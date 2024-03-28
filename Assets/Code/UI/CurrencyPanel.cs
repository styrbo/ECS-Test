using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI {
    public class CurrencyPanel : MonoBehaviour {
        public TMP_Text displayedText;
        public Button resetButton;
        public Button increaseButton;
        
        public string format = "Gold: {0}";
        
        private CurrencyType _type;

        public void Init(CurrencyType type) {
            _type = type;
            
            resetButton.onClick.AddListener(OnPressToResetButton);
            increaseButton.onClick.AddListener(OnPressToIncreaseButton);
        }
        
        private void SetTextValue(int value) {
            displayedText.text = string.Format(format, value);
        }

        private void OnPressToResetButton() {
            Main.Wallet.ResetCurrencyValue(_type);
        }

        private void OnPressToIncreaseButton() {
            Main.Wallet.SetCurrencyValue(Main.Wallet.GetCurrencyValue(_type) + 1, _type);
        }

        private void Update() {
            SetTextValue(Main.Wallet.GetCurrencyValue(_type));
        }
    }
}
