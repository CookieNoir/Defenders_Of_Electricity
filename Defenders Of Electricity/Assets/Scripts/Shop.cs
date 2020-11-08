using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    WeaponFactory _weaponFactory;

    private void Start()
    {
        _weaponFactory = WeaponFactory.instance;
    }


}
