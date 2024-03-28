using UnityEngine;
using UnityEngine.UI;

namespace Code.UI {
    public class SavePanel : MonoBehaviour {

        public Button saveButton;
        public Button loadButton;
        
        public void Init(SavingType type) {
            saveButton.onClick.AddListener(() => Main.Wallet.Save(type));
            loadButton.onClick.AddListener(() => Main.Wallet.Load(type));
        }
    }
}
