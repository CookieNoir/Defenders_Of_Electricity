using System.Collections.Generic;
using UnityEngine;

public class AreaControl : MonoBehaviour
{
    public bool isActive = false;
    public Node[] nodes;
    public Light lightComponent;

    private void Start()
    {
        TurnOff();
    }

    public void TurnOff()
    {
        ChangeActivity(false);
    }

    private void ChangeActivity(bool value)
    {
        isActive = value;
        for (int i = 0; i < nodes.Length; ++i)
        {
            nodes[i].isActive = isActive;
        }
        if (lightComponent) lightComponent.enabled = isActive;
    }

    private void OnMouseDown()
    {
        ChangeActivity(!isActive);
    }
}
