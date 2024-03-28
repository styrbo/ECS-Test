using UnityEngine;

namespace Code.UI {
    public class CurrenciesScreen : MonoBehaviour {
        
        public CurrencyPanel goldPanel;
        public CurrencyPanel crystalPanel;
        
        public SavePanel prefsSavePanel, jsonSavePanel;

        private void Awake() {
            goldPanel.Init(CurrencyType.Gold);
            crystalPanel.Init(CurrencyType.Crystal);
            
            prefsSavePanel.Init(SavingType.PlayerPrefs);
            jsonSavePanel.Init(SavingType.JsonIntoFile);
        }
    }
}
