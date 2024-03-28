using System;
using System.Collections.Generic;
using System.Linq;
using Code.Components;
using Code.Systems;
using Unity.Entities;

namespace Code {
    public class Wallet {
        
        public readonly Dictionary<CurrencyType, ICurrency> SupportedCurrencies;
        public readonly Dictionary<SavingType, ISaveMethod> SupportedSaveMethods;

        private readonly World _world;
        private WalletSystemGroup _systemGroup;
        
        public Entity Entity { get; }

        public Wallet() {
            _world = World.DefaultGameObjectInjectionWorld;

            // add here new currencies and save methods here
            SupportedCurrencies = new Dictionary<CurrencyType, ICurrency> {
                { CurrencyType.Gold, new Currency<GoldComponent>(_world) },
                { CurrencyType.Crystal, new Currency<CrystalComponent>(_world) }
            };
            SupportedSaveMethods = new Dictionary<SavingType, ISaveMethod> {
                { SavingType.PlayerPrefs, new SaveMethod<PlayerPrefsSavingSystem>(_world) },
                { SavingType.JsonIntoFile, new SaveMethod<FileSavingSystem>(_world) }
            };

            var components = SupportedCurrencies.Values
                .Select(currency => currency.ComponentType)
                .ToArray()
                .Concat(new[] { ComponentType.ReadOnly<WalletComponent>() });

            Entity = _world.EntityManager.CreateEntity(components.ToArray());
        }
        
        public void InitializeSystems() {
            _systemGroup = _world.CreateSystemManaged<WalletSystemGroup>();
            
            _systemGroup.AddSystemToUpdateList(_world.CreateSystem<WalletSystem>());
            
            foreach (var (_, system) in SupportedSaveMethods) {
                _systemGroup.AddSystemToUpdateList(system.Handle);
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

        public void Save(SavingType type) {
            if(!SupportedSaveMethods.ContainsKey(type))
                throw new InvalidOperationException($"Save method {type} is not supported");
            
            _world.EntityManager.AddComponentData(Entity, new SaveRequestComponent {
                Type = type,
            });
            _systemGroup.Update();
        }

        public void Load(SavingType type) {
            if(!SupportedSaveMethods.ContainsKey(type))
                throw new InvalidOperationException($"Save method {type} is not supported");
            
            _world.EntityManager.AddComponentData(Entity, new LoadRequestComponent {
                Type = type,
            });
            _systemGroup.Update();
        }
    }
}
