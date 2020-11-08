using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isActive;
    private bool _prevIsActive;
    public Tower weapon;

    private Color _baseNodeColor;

    private void Start()
    {
        _prevIsActive = isActive;
    }

    // Переписать и слушать переменую isActive и при изменении менять
    private void Update()
    {
        if (isActive != _prevIsActive)
        {
            if (weapon)
                weapon.GetComponent<Tower>().isActive = isActive;
            _prevIsActive = isActive;
        }
    }

    private void OnMouseDown()
    {
        if (!weapon && isActive)
        {
            GameObject turretToBuild = WeaponFactory.instance.GetTurretToBuild();
            if (!turretToBuild)
                return;

            weapon = Instantiate(turretToBuild, transform.position, transform.rotation).GetComponent<Tower>();
            weapon.isActive = true;
        }
    }
}
