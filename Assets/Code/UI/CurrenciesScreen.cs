using UnityEngine;
using UnityEngine.UI;

namespace Code.UI {
    public class CurrenciesScreen : MonoBehaviour {
        
        public CurrencyPanel goldPanel;
        public CurrencyPanel crystalPanel;
        
        public Button saveButton;
        public Button loadButton;

        private void Awake() {
            goldPanel.Init(CurrencyType.Gold);
            crystalPanel.Init(CurrencyType.Crystal);
            
            saveButton.onClick.AddListener(OnPressToSaveButton);
            loadButton.onClick.AddListener(OnPressToLoadButton);
        }
        
        private void OnPressToSaveButton() {
            Main.Wallet.Save();
        }
        
        private void OnPressToLoadButton() {
            Main.Wallet.Load();
        }
    }
}
