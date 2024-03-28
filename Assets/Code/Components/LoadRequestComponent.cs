using Unity.Entities;

namespace Code.Components {
    public struct LoadRequestComponent : IComponentData {
        public SavingType Type;
    }
}
