using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace Code.Systems {

    [Serializable]
    class SaveData {
        public CurrencySaveData[] wallet;
    }

    [Serializable]
    class CurrencySaveData {
        public CurrencyType type;
        public int value;
    }
    
    [DisableAutoCreation]
    public partial class FileSavingSystem : SavingSystemBase {

        private const string FileName = "SaveData";

        private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public override SavingType Type => SavingType.JsonIntoFile;

        protected override async Task Load() {
            var currencies = Wallet.SupportedCurrencies;
            var fileData = await File.ReadAllTextAsync(FilePath);
            
            var saveData = JsonUtility.FromJson<SaveData>(fileData);
            
            foreach (var currency in saveData.wallet) {
                var currencyType = currency.type;
                var currencyValue = currency.value;
                
                if (!currencies.ContainsKey(currencyType))
                    continue;
                
                currencies[currencyType].Handler.ChangeCurrencyValue(currencyValue);
            }
        }

        protected override async Task Save() {
            var saveData = new SaveData {
                wallet = Wallet.SupportedCurrencies
                    .Select(currency => new CurrencySaveData {
                        type = currency.Key,
                        value = currency.Value.Handler.ReadValue(Wallet.Entity)
                    })
                    .ToArray()
            };
            
            var json = JsonUtility.ToJson(saveData, true);
            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}
