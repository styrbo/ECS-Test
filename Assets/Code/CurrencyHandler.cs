using Unity.Collections;
using Unity.Entities;

namespace Code {
    
    public interface ICurrencyHandler {
        
        void ChangeCurrencyValue(int newValue);
        int ReadValue(Entity entity);
    }

    public class CurrencyHandler<T> : ICurrencyHandler where T : unmanaged, ICurrencyComponent {

        private readonly World _world;
        private EntityQuery _query;
        

        public CurrencyHandler(World world) {
            _world = world;
            _query = world.EntityManager.CreateEntityQuery(typeof(T));
        }

        public void ChangeCurrencyValue(int newValue) {
            var entities = _query.ToEntityArray(Allocator.TempJob);
            
            foreach (var entity in entities) {
                var component = _world.EntityManager.GetComponentData<T>(entity);
                component.Value = newValue;
                _world.EntityManager.SetComponentData(entity, component);
            }
            
            entities.Dispose();
        }

        public int ReadValue(Entity entity) {
            return _world.EntityManager.GetComponentData<T>(entity).Value;
        }
    }
}
