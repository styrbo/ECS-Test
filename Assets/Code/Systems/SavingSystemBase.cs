using System;
using System.Threading.Tasks;
using Code.Components;
using Unity.Entities;
using UnityEngine;

namespace Code.Systems {
    
    [DisableAutoCreation]
    public abstract partial class SavingSystemBase : SystemBase {
        protected Wallet Wallet { get; private set; }
        
        public abstract SavingType Type { get; }

        public void Initialize(Wallet wallet) {
            Wallet = wallet;
        }

        protected override void OnUpdate() {
            Entities.ForEach((ref LoadRequestComponent component) => {
                if (component.Type != Type)
                    return;
                
                StartLoadJob();
            }).WithoutBurst().Run();
            
            Entities.ForEach((ref SaveRequestComponent component) => {
                if (component.Type != Type)
                    return;
                
                StartSaveJob();
            }).WithoutBurst().Run();
        }
        
        private void StartLoadJob() {
            RunJob(Load);
        }
        
        private void StartSaveJob() {
            RunJob(Save);
        }
        
        private void RunJob(Func<Task> action) {
            action().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.LogError(task.Exception);
                }
            });
        }

        protected abstract Task Load();

        protected abstract Task Save();
    }
}
