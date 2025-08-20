using Unity.Entities;

partial class PlayerUIUltiBarSystem : SystemBase {

    protected override void OnUpdate() {
        Entities.ForEach((ref Skills skills) => {
            if (PlayerUltiUI.Instance != null) {
                PlayerUltiUI.Instance.SetReloadTime(skills.UltiReloadTime);
                PlayerUltiUI.Instance.SetTimer(skills.UltiTimer);
            }

        }).WithoutBurst().Run();
    }
}
