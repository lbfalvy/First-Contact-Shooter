using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Selector
{
    [HideInInspector] public NameTag target;
    public KeyCode Select;
    ShipControl sc;
    Camera cam;

    public void Start(ShipControl shipControl, Camera camera)
    {
        sc = shipControl;
        cam = camera;
    }

    public void Update()
    {
        if (Input.GetKeyDown(Select))
            selectTag();
    }

    void selectTag() {
        NameTag closest = findClosestTag();
        changeSelection(closest);
    }
    NameTag findClosestTag() {
        Vector2 center = cam.pixelRect.size / 2;
        var allTags = Component.FindObjectsOfType<NameTag>()
            .Where( x => x.Visible && x.Selectable ).ToArray();
        if (allTags.Length == 0) return null;
        float closestDistance = allTags.Min( 
            x => ( center - x.Position ).magnitude
        );
        return allTags.First( 
            x => ( center - x.Position ).magnitude == closestDistance
        );
    }
    void changeSelection(NameTag selection) {
        target?.Unselect();
        target = selection;
        target?.Select();
    }
}
