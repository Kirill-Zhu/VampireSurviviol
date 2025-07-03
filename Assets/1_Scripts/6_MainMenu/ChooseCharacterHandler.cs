using Chartacters;
using System.Collections.Generic;
using UnityEngine;


public class ChooseCharacterHandler : MonoBehaviour
{
    [SerializeField] private PlayerSettingsManager _playerSettingsManager;
    [SerializeField] private List<Character> _charactersList;
    public CharacterType _characterType;

    public void ChooseCharactrer() {

        int characterIndex = (int)_characterType;
        _playerSettingsManager.SetCharacret(_charactersList[characterIndex]);
    }
}
