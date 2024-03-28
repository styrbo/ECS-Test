using UnityEngine;

namespace Code {
    public class Main : MonoBehaviour {
        
        public static Wallet Wallet { get; private set; }
        
        private void Awake() {
            
            Wallet = new Wallet();
            Wallet.InitializeSystems();
        }
    }
}
