using UnityEngine;
using UnityEngine.UI;
public class PlayerUltiUI : MonoBehaviour
{
    public static PlayerUltiUI Instance;
    [SerializeField] private Slider _dashSlider;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

    }

    public void SetReloadTime(float reloadTime) {
        _dashSlider.maxValue = reloadTime;
    }
    public void SetTimer(float timer) {
        _dashSlider.value = timer;
    }
    public bool CanUlti() {
        return _dashSlider.value == _dashSlider.maxValue;
    }
}
