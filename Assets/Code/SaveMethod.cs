using Code.Systems;
using Unity.Entities;

namespace Code {

    public class SaveMethod<T> : ISaveMethod where T : SavingSystemBase, new() {
        public SystemHandle Handle { get; }
        
        public SaveMethod(Wallet wallet, World world) {
            var reference = world.CreateSystemManaged<T>();
            reference.Initialize(wallet);
            
            Handle = reference.SystemHandle;
        }
    }
    
    public interface ISaveMethod {
        public SystemHandle Handle { get; }
    }
}
