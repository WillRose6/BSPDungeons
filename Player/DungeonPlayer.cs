using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayer : Player
{
    public GameObject projectile;
    public float FireRate;
    private float fireCountdown;
    public SubFloor currentSubdungeon;
    public Vector3 projectileOffset;

    [HideInInspector]
    public DungeonUI ui;

    protected override void Start()
    {
        base.Start();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<DungeonUI>();
        ui.Begin(StartHealth);
        fireCountdown = FireRate;

    }

    protected override void Update()
    {
        base.Update();
        fireCountdown -= Time.deltaTime;
    }

    protected override void FaceTowardsInput()
    {
        if (!ui.inventoryHolderObject.activeInHierarchy)
        {
            base.FaceTowardsInput();
        }
    }

    protected override void ProcessInput()
    {
        base.ProcessInput();

        if (InventoryHidden())
        {
            if (Input.GetButton("Fire"))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (fireCountdown <= 0)
        {
            ChangeFrozenAmount(5);
            anim.Attack();
            fireCountdown = FireRate;
        }
    }

    protected override bool InventoryHidden()
    {
        return !ui.inventoryHolderObject.activeInHierarchy;
    }

    void SpawnProjectile()
    {
        Instantiate(projectile, (transform.position + projectileOffset) + transform.forward, GetRotationTowardsMouse());
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        ui.UpdatePlayerHealthBar(StartHealth, currentHealth);
        ui.FlashDamage();
        StartCoroutine(EmphasizeHit());
    }


    private IEnumerator EmphasizeHit()
    {
        Time.timeScale = 0.3f;
        StartCoroutine(mainCamera.Shake(0.075f, 0.4f));
        yield return new WaitForSecondsRealtime(0.04f);
        Time.timeScale = 1;
    }

    public override void Die()
    {
        base.Die();
        StartCoroutine(GameManager.instance.GameOver());
    }
}
