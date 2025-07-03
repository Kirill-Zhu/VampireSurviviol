using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthUIAuthoring : MonoBehaviour {
    public static PlayerHealthUIAuthoring instance;

    [SerializeField] private Slider _slider;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }
    public void SetHealth(int value) { 
        _slider.value = value; }
}