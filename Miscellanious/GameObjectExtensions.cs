using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void ResetTag(this GameObject obj)
    {
        obj.tag = "Untagged";
    }
}
