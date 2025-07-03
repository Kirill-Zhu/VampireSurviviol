using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial class PlayerUIHealtBarSystem : SystemBase {

    protected override void OnUpdate() {
        Entities.ForEach((ref PlayerHealth playerHealth) => {
            if (PlayerHealthUIAuthoring.instance != null) {
                PlayerHealthUIAuthoring.instance.SetHealth(playerHealth.Health);
            }

        }).WithoutBurst().Run();
    }
}
