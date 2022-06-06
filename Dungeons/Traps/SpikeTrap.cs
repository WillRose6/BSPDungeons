using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : TimedTrap
{
    public Animator anim;

    protected override void OnActivate()
    {
        base.OnActivate();
        anim.SetTrigger("Activate");
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();
        anim.SetTrigger("Deactivate");
    }

    public void PlaySpikeSound()
    {
        DungeonPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
        if (player.currentSubdungeon == subDungeon)
        {
            SFXPlayer.instance.PlayEffect(soundEffect, 1);
        }
    }
}
