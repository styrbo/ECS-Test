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

        protected override void OnCreate() {
            base.OnCreate();

            Wallet = Main.Wallet;
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
            Job.WithoutBurst().WithCode(() => {
                try {
                    action.Invoke().Wait();
                } catch (Exception e) {
                    Debug.LogError(e);
                }
            }).Run();
        }

        protected abstract Task Load();

        protected abstract Task Save();
    }
}
