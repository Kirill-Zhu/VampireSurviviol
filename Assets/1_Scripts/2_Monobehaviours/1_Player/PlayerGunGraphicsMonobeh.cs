using System.Collections.Generic;
using UnityEngine;

public class PlayerGunGraphicsMonobeh : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gunGraphicsList;
    private GunType _gunType;

    public static PlayerGunGraphicsMonobeh Instance;

    private void Awake() {
        if(Instance == null)    
           Instance = this;
        else
            Destroy(this.gameObject);
    }
    public void ChangeGunGraphics(GunType gunType) {

        Debug.Log("Change gun graphics");
        if (gunType == _gunType)
            return;
        _gunType = gunType;

        foreach (var gun in _gunGraphicsList) {
            if(!gun.activeInHierarchy)
                continue;   
            else
                gun.SetActive(false);
        }
        _gunGraphicsList[(int)_gunType].SetActive(true);
    }
}
