using Code.Systems;
using Unity.Entities;

namespace Code {

    public class SaveMethod<T> : ISaveMethod where T : SavingSystemBase, new() {
        public SystemHandle Handle { get; }
        
        public SaveMethod(World world) {
            Handle = world.CreateSystem<T>();
        }
    }
    
    public interface ISaveMethod {
        public SystemHandle Handle { get; }
    }
}
