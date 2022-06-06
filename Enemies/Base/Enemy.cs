using System.Collections;
using UnityEngine;

public class Enemy : LivingBeing
{
    public BulletPattern bulletPattern;
    protected DungeonPlayer player;
    public float rotationSpeed;
    public SubFloor subDungeon;
    protected bool canRotate;
    public int XP;

    public ItemTemplate[] itemDrops;
    public float ChanceToDropItem;

    protected override void Start()
    {
        base.Start();
        if (!bulletPattern)
        {
            bulletPattern = GetComponentInChildren<BulletPattern>();
        }

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
        }

        subDungeon.enemiesInDungeon++;
        if (bulletPattern) {
            StartCoroutine(Fire());
        }
    }

    public virtual IEnumerator Fire()
    {
        while (true)
        {
            if (subDungeon == player.currentSubdungeon)
            {
                PerformAttack();
                yield return new WaitForSeconds(Random.Range(bulletPattern.minFireRate, bulletPattern.maxFireRate));
            }
            else
            {
                yield return null;
            }
        }
    }

    public virtual void PerformAttack()
    {
    }

    public virtual void Resume()
    {
    }

    public virtual void Update()
    {
        if (canRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime * rotationSpeed);
        }
    }

    public override void Die()
    {
        if (Random.Range(0, 100) <= ChanceToDropItem)
        {
            DropItem();
        }
        GivePlayerXP();
        subDungeon.enemiesInDungeon--;
        base.Die();
    }

    public virtual void GivePlayerXP()
    {
        player.inventory.RecieveWeaponXP(XP);
    }

    public virtual void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    public void DropItem()
    {
        Vector3 position = transform.position;
        position.y = 1f;
        ItemPickUp i = Instantiate(References.instance.itemPickUpPrefab, position, Quaternion.identity).GetComponent<ItemPickUp>();
        i.SetItem(itemDrops);
    }
}
