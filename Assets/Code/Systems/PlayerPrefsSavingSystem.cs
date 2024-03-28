using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace Code.Systems {
    
    [DisableAutoCreation]
    public partial class PlayerPrefsSavingSystem : SavingSystemBase {
        
        public override SavingType Type => SavingType.PlayerPrefs;

        protected override Task Load() {
            foreach (var (type, currency) in Wallet.SupportedCurrencies) {
                var key = GetCurrencySaveKey(type);
            
                if (!PlayerPrefs.HasKey(key))
                    continue;
                
                currency.Handler.ChangeCurrencyValue(PlayerPrefs.GetInt(key));
            }
            
            return Task.CompletedTask;
        }

        protected override Task Save() {
            foreach (var (type, currency) in Wallet.SupportedCurrencies) {
                var key = GetCurrencySaveKey(type);
                
                PlayerPrefs.SetInt(key, currency.Handler.ReadValue(Wallet.Entity));
            }
            
            return Task.CompletedTask;
        }
        
        private string GetCurrencySaveKey(CurrencyType type) {
            return $"{type}_SaveKey";
        }
    }
}
