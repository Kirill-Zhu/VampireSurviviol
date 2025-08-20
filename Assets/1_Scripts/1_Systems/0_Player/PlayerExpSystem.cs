using Unity.Entities;


partial class PlayerExpSystem : SystemBase{
    private int _currentLvl=1;
  
 
    protected override void OnUpdate() {
        foreach (var PlayerExp in SystemAPI.Query<RefRO<PlayerExp>>()) {
            if (PlayerExp.ValueRO.Lvl > _currentLvl) {
                PlayerLvlUpManager.Instance.LvlUp();
                UnityEngine.Debug.Log("LvlUpSYstemBase");
                _currentLvl++;
            }
        }
    }
}
