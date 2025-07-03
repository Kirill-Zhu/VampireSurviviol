using UnityEngine;
using Chartacters;
public class PlayerSettingsManager : MonoBehaviour
{
    
    [SerializeField] private PlayerSettings _playerSettings;

 
    public void SetCharacret(Character character) {
        _playerSettings.Character = character;
    }
}
public enum CharacterType {
    None = 0,
    Human = 1,
    Vampire = 2
}