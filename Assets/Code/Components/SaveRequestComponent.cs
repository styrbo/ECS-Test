using Unity.Entities;

namespace Code.Components {
    public struct SaveRequestComponent : IComponentData {
        public SavingType Type;
    }
}
