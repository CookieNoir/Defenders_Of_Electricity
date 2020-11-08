using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFactory : MonoBehaviour
{
    public static WeaponFactory instance;

    public GameObject[] _allTurret;
    public int[] cost;
    public Color selectedColor;
    public Color deselectedColor;
    public MaskableGraphic[] icons;

    private int selected = -1;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Select(-1);
    }

    public void Purchase(int value)
    {
        Select(value);
    }

    private void Select(int index)
    {
        selected = index;
        for (int i = 0; i < icons.Length; ++i)
        {
            if (i == selected) icons[i].color = selectedColor;
            else icons[i].color = deselectedColor;
        }
    }

    public GameObject GetTurretToBuild()
    {
        if (selected < 0)
            return null;
        else
        {
            if (GameController.instance.PlayerMoney >= cost[selected])
            {
                GameController.instance.UpdateMoney(-cost[selected]);
                int value = selected;
                Select(-1);
                return _allTurret[value];
            }
            else return null;
        }
    }
}
