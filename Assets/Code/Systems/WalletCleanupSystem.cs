using Code.Components;
using Unity.Entities;

namespace Code.Systems {
    
    [DisableAutoCreation]
    public partial class WalletCleanupSystem : SystemBase {
        
        protected override void OnUpdate() {
            
            
            Entities.ForEach((ref Entity entity, ref WalletComponent _) => {
                EntityManager.RemoveComponent<LoadRequestComponent>(entity);
                EntityManager.RemoveComponent<SaveRequestComponent>(entity);
            }).WithStructuralChanges().Run();
        }
    }
}
