using Unity.Burst;
using Unity.Entities;

partial class PlayerUIDashBarSystem : SystemBase {
    protected override void OnUpdate() {
        Entities.ForEach((ref PlayerMover playerMover) => {
            if (PlayerDashUIAuthoring.Instance != null) {
                PlayerDashUIAuthoring.Instance.SetReloadTime(playerMover.DashReloadTime);
                PlayerDashUIAuthoring.Instance.SetTimer(playerMover.DashTimer);
            }

        }).WithoutBurst().Run();
    }
}
