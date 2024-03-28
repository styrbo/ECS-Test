using System;
using Code.Components;
using Unity.Entities;

namespace Code.Systems {
    
    [DisableAutoCreation]
    public partial class WalletSystem : SystemBase {

        private Wallet _wallet;
        
        protected override void OnCreate() {
            _wallet = Main.Wallet;
        }

        protected override void OnUpdate() {
            Entities.ForEach((Entity entity, ref CurrencyChangeRequest request) => {
                
                if (!_wallet.SupportedCurrencies.ContainsKey(request.Type)) {
                    throw new InvalidOperationException($"Currency type {request.Type} is not supported");
                }

                var handler = _wallet.SupportedCurrencies[request.Type].Handler;
                handler.ChangeCurrencyValue(request.NewValue);
                EntityManager.RemoveComponent<CurrencyChangeRequest>(entity);
            }).WithStructuralChanges().Run();
        }
    }
}
