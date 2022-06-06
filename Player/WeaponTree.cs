using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeaponTree : MonoBehaviour
{
    public List<WeaponTreeNode> nodes;
    public List<GameObject> links;
    public GameObject node;
    public Transform parent;
    public int xDifference, yDifference;

    public void CreateTree(WeaponTemplate startingWeapon)
    {
        if (nodes.Count == 0)
        {
            DrawNodesToTree(startingWeapon, 0, 0, null);
        }

        if (links.Count == 0)
        {
            DrawLinksBetweenNodes(nodes[0]);
        }
        UnlockWeapons();
    }

    public void DrawNodesToTree(WeaponTemplate currentWeapon, int x, int y, WeaponTreeNode previous)
    {
        WeaponTreeNode w = null;
        WeaponTreeNode overlap = CheckIfOverlappingNode(x, y);
        if (overlap == null)
        {
            w = Instantiate(node, parent).GetComponent<WeaponTreeNode>();
        }
        else
        {
            w = overlap;
        }

        w.Setup(currentWeapon);
        w.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

        if (previous)
        {
            w.PreviousNode.Add(previous);
            previous.AddNextNode(w);
        }

        nodes.Add(w);

        int flip = yDifference;
        int counter = 0;
        for(int i = 0; i < currentWeapon.weaponTransmutations.Length; i++)
        {
            if(i % 2 == 0)
            {
                counter++;
            }
            DrawNodesToTree(currentWeapon.weaponTransmutations[i], x + xDifference, y + (flip * counter), w);
            flip = -flip;
        }
    }

    public void DrawLinksBetweenNodes(WeaponTreeNode currentNode)
    {
        foreach(WeaponTreeNode node in currentNode.NextNodes)
        {
            GameObject g = new GameObject();
            g.transform.SetParent(parent);
            g.transform.SetAsFirstSibling();
            g.name = "Link";
            Image i = g.AddComponent<Image>();
            RectTransform t = i.GetComponent<RectTransform>();
            Vector3 difference = node.transform.position - currentNode.transform.position;
            t.sizeDelta = new Vector2(difference.magnitude, 10f);
            t.pivot = new Vector2(0, 0.5f);
            t.position = currentNode.transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            t.rotation = Quaternion.Euler(0, 0, angle);
            links.Add(g);

            DrawLinksBetweenNodes(node);
        }
    }

    public WeaponTreeNode CheckIfOverlappingNode(int x, int y)
    {
        foreach(WeaponTreeNode n in nodes)
        {
            RectTransform t = n.GetComponent<RectTransform>();
            if(t.anchoredPosition.x == x && t.anchoredPosition.y == y)
            {
                return n;
            }
        }

        return null;
    }

    public WeaponTreeNode GetNodeForWeaponTemplate(WeaponTemplate template)
    {
        foreach(WeaponTreeNode n in nodes)
        {
            if(n.Template == template)
            {
                return n;
            }
        }

        return null;
    }

    public void UnlockWeapons()
    {
        MainUI ui = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();
        List<WeaponTemplate> templates = new List<WeaponTemplate>();
        foreach(Weapon w in ui.transmutationScreen.unlockedWeapons)
        {
            WeaponTemplate wt = References.instance.GetWeaponTemplateByID(w.TemplateID);
            if (!templates.Contains(wt))
            {
                templates.Add(wt);
            }
        }

        foreach(WeaponTemplate t in templates)
        {
            if (t != null)
            {
                WeaponTreeNode n = GetNodeForWeaponTemplate(t);

                if (n)
                {
                    n.Unlock();
                }
            }
        }
    }
}
