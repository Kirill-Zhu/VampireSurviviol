using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerLvlUpUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> _skillSprites;
    [SerializeField] private List<Button> _skillButtons;

    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;

    private void OnEnable() {
        PlayerLvlUpManager.Instance.LvlUpUIIsOpened = true;
    }
    private void OnDisable() {
        _playerInput.SwitchCurrentActionMap("Player");
        PlayerLvlUpManager.Instance.LvlUpUIIsOpened = false;
    }

    public void ResumeGame() {
        PlayerLvlUpManager.Instance.ResumeGame();
    }
}
