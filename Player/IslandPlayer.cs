using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPlayer : Player
{
    protected override void CreateCamera()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<PlayerCamera>();
        mainCamera.playerObject = gameObject;
        mainCamera.transform.position = transform.position + mainCamera.offset;
    }
}
