using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponTreeNode : MonoBehaviour
{
    [SerializeField]
    private Image weaponSprite;
    [SerializeField]
    private TextMeshProUGUI weaponName;
    [SerializeField]
    private Sprite lockedSprite;
    private WeaponTemplate template;
    public WeaponTemplate Template
    {
        get { return template; }
        set { template = value; }
    }
    [SerializeField]
    private List<WeaponTreeNode> previousNode;
    public System.Collections.Generic.List<WeaponTreeNode> PreviousNode
    {
        get { return previousNode; }
        set { previousNode = value; }
    }
    [SerializeField]
    private List<WeaponTreeNode> nextNodes;
    public List<WeaponTreeNode> NextNodes
    {
        get { return nextNodes; }
        set { nextNodes = value; }
    }
    public void AddNextNode(WeaponTreeNode node)
    {
        NextNodes.Add(node);
    }

    public void Setup(WeaponTemplate template)
    {
        this.Template = template;
        Lock();
    }

    public void Lock()
    {
        weaponSprite.sprite = lockedSprite;
        weaponName.text = "???";
    }

    public void Unlock()
    {
        weaponSprite.sprite = template.sprite;
        weaponName.text = template.Name;
    }

    public void DisplayStats()
    {
        MainUI ui = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();
        ui.transmutationScreen.CompareWeapon(template);
    }

    public void HideStats()
    {
        MainUI ui = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();
        ui.transmutationScreen.UnCompareWeapon();
    }

    public void UpgradeWeapon()
    {
        MainUI ui = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();
        if (ui.transmutationScreen.CheckIfWeaponIsUpgradable(template))
        {
            ui.transmutationScreen.UpgradeWeapon(template);
        }
    }
}
