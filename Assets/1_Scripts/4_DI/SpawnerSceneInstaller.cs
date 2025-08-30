using UnityEngine;
using Zenject;
public class SpawnerSceneInstaller : MonoInstaller
{
    [SerializeField] private DecalsSpawner _decalsSpawner;
    [SerializeField] private PlayerGunGraphicsMonobeh _playerGunGraphicsMonobeh;
    public override void InstallBindings() {
        Container.Bind<DecalsSpawner>().FromInstance(_decalsSpawner);
        Container.Bind<PlayerGunGraphicsMonobeh>().FromInstance(_playerGunGraphicsMonobeh);
    }
}
