using Unity.Burst;
using Unity.Entities;
using Zenject;

partial class ChangeGunGraphics : SystemBase {

    private double _lasetElapsedTime;
    private float _reloadTime;
    private GunType _gunType;
 
    private PlayerGunGraphicsMonobeh _playerGunGraphicsMonobeh;
    protected override void OnCreate() {
        _reloadTime = 0.3f;
    }
    protected override void OnUpdate() {
        if (SystemAPI.Time.ElapsedTime -_lasetElapsedTime < _reloadTime)
            return;
        _lasetElapsedTime = SystemAPI.Time.ElapsedTime;

        Entities.ForEach((in ControlledGun controlledGun) => {
            _gunType = controlledGun.gunType;
        }).WithoutBurst().Run();

        _playerGunGraphicsMonobeh = PlayerGunGraphicsMonobeh.Instance;
        _playerGunGraphicsMonobeh.ChangeGunGraphics(_gunType);

    }
}
