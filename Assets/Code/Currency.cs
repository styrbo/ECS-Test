using Unity.Entities;

namespace Code {
    public class Currency<T> : ICurrency where T : unmanaged, ICurrencyComponent {
        public Currency(World entityWorld) {
            ComponentType = ComponentType.ReadOnly<T>();
            Handler = new CurrencyHandler<T>(entityWorld);
        }

        public ComponentType ComponentType { get; }
        public ICurrencyHandler Handler { get; }
    }

    public interface ICurrency {
        public ComponentType ComponentType { get; }
        public ICurrencyHandler Handler { get; }
    }
}
