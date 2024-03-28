using System;
using System.Collections.Generic;
using System.Linq;
using Code.Components;
using Code.Systems;
using Unity.Entities;

namespace Code {
    public class Wallet {
        public readonly Dictionary<CurrencyType, ICurrency> SupportedCurrencies;

        private readonly World _world;
        private WalletSystemGroup _systemGroup;
        
        public Entity Entity { get; }

        public Wallet() {
            _world = World.DefaultGameObjectInjectionWorld;

            // add here new currencies
            SupportedCurrencies = new Dictionary<CurrencyType, ICurrency> {
                { CurrencyType.Gold, new Currency<GoldComponent>(_world) },
                { CurrencyType.Crystal, new Currency<CrystalComponent>(_world) }
            };

            var components = SupportedCurrencies.Values
                .Select(currency => currency.ComponentType)
                .ToArray()
                .Concat(new[] { ComponentType.ReadOnly<WalletComponent>() });

            Entity = _world.EntityManager.CreateEntity(components.ToArray());
        }
        
        public void InitializeSystems() {
            var supportedSaveMethods = new []{
                typeof(PlayerPrefsSavingSystem),
            };

            _systemGroup = _world.CreateSystemManaged<WalletSystemGroup>();
            
            _systemGroup.AddSystemToUpdateList(_world.CreateSystem<WalletSystem>());
            
            foreach (var saveMethod in supportedSaveMethods) {
                _systemGroup.AddSystemToUpdateList(_world.CreateSystem(saveMethod));
            }
            
            _systemGroup.AddSystemToUpdateList(_world.CreateSystem<WalletCleanupSystem>());
        }

        public int GetCurrencyValue(CurrencyType type) {
            if (!SupportedCurrencies.TryGetValue(type, out var currency))
                throw new InvalidOperationException($"Currency type {type} is not supported");

            return currency.Handler.ReadValue(Entity);
        }

        public void SetCurrencyValue(int value, CurrencyType type) {
            _world.EntityManager.AddComponentData(Entity, new CurrencyChangeRequest {
                Type = type,
                NewValue = value
            });
            
            _systemGroup.Update();
        }
        
        public void ResetCurrencyValue(CurrencyType type) {
            SetCurrencyValue(0, type);
        }

        public void Save() {
            _world.EntityManager.AddComponent<SaveRequestComponent>(Entity);
            _systemGroup.Update();
        }

        public void Load() {
            _world.EntityManager.AddComponent<LoadRequestComponent>(Entity);
            _systemGroup.Update();
        }

        public static string GetCurrencySaveKey(CurrencyType type) {
            return $"Currency_{type}";
        }
    }
}
