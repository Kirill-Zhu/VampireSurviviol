using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerSettingsInstaller", menuName = "Custom/NewPlayerSettingsInstaller")]
public class PlayerSettingsInstaller : ScriptableObjectInstaller
{

    public PlayerSettings _settings;
    public override void InstallBindings()
    {
        Container.Bind<PlayerSettings>().FromScriptableObject(_settings).AsSingle();
    }
}