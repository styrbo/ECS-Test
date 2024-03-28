using Unity.Entities;

namespace Code {
    public partial class WalletSystemGroup : ComponentSystemGroup {
        protected override void OnCreate() {
            base.OnCreate();
            
            EnableSystemSorting = false;
        }
    }
}
