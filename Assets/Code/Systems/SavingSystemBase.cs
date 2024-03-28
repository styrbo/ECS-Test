using System.Threading.Tasks;
using Code.Components;
using Unity.Entities;
using UnityEngine;

namespace Code.Systems {
    
    [DisableAutoCreation]
    public abstract partial class SavingSystemBase : SystemBase {
        protected Wallet Wallet { get; private set; }

        protected override void OnCreate() {
            base.OnCreate();

            Wallet = Main.Wallet;
            StartLoadJob();
        }

        protected override void OnUpdate() {

            Entities.ForEach((ref LoadRequestComponent _) => {
                StartLoadJob();
            }).WithoutBurst().Run();
            
            Entities.ForEach((ref SaveRequestComponent _) => {
                StartSaveJob();
            }).WithoutBurst().Run();
        }
        
        private void StartLoadJob() {
            Job.WithoutBurst().WithCode(() => Load().Wait()).Run();
        }
        
        private void StartSaveJob() {
            Job.WithoutBurst().WithCode(() => Save().Wait()).Run();
        }

        protected abstract Task Load();

        protected abstract Task Save();
    }
}
