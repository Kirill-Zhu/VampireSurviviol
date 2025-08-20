using UnityEngine;
using Zenject;
public class SpawnerSceneInstaller : MonoInstaller
{
    [SerializeField] private DecalsSpawner _decalsSpawner;
    public override void InstallBindings() {
        Container.Bind<DecalsSpawner>().FromInstance(_decalsSpawner);
       
    }
}
