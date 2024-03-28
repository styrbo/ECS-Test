using Unity.Entities;

namespace Code {
    public interface ICurrencyComponent : IComponentData {
        public int Value { get; set; }
    }
}
