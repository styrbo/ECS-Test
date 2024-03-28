using Unity.Entities;

namespace Code.Components {
    public struct CurrencyChangeRequest : IComponentData {
        public CurrencyType Type;
        public int NewValue;
    }
}
