using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject display;

    public void SetItemForDisplay(Item item)
    {
        GameObject newDisplay = Instantiate(References.instance.GetItemTemplateByID(item.TemplateID).prefab, display.transform.position, Quaternion.identity);
        SetupDisplay(newDisplay);
    }

    public void SetItemForDisplay(Weapon weapon)
    {
        GameObject newDisplay = Instantiate(References.instance.GetWeaponTemplateByID(weapon.TemplateID).prefab, display.transform.position, Quaternion.identity);
        SetupDisplay(newDisplay);
    }

    public void SetupDisplay(GameObject newDisplay)
    {
        newDisplay.transform.SetParent(transform);
        Destroy(display);
        display = newDisplay;

        Rotator r = newDisplay.GetComponent<Rotator>();
        if (r == null)
        {
            r = newDisplay.AddComponent<Rotator>();
            r.speed = 20;
            r.axis = new Vector3(0, 1, 0);
        }
    }
}
