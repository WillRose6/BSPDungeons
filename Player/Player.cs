using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingBeing
{
    public Rigidbody rb;
    public float movespeed;
    Vector3 moveVelocity;
    public float Dampening;
    public float RotationSpeed;
    public GameObject playerCameraPrefab;
    public Inventory inventory;
    public PlayerAnimator anim;
    public AudioClip openInventoryClip;

    [Header("Variables")]
    public float InteractRange;
    public float frozenAmount;

    [HideInInspector]
    public MainUI mainUI;

    //Change later
    public static string playerName = "Will";

    protected PlayerCamera mainCamera;
    private GameObject closestInteractableObject;

    public enum ControllerType
    {
        MOUSE,
        CONTROLLER,
    }

    public ControllerType controllerType;

    protected override void Start()
    {
        base.Start();
        mainUI = GameObject.FindGameObjectWithTag("UI").GetComponent<MainUI>();

        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (!inventory)
        {
            inventory = GetComponent<Inventory>();
        }


        ChangeFrozenAmount(0);
        CreateCamera();
        PlayerAudioManager audioManager = mainCamera.GetComponentInChildren<PlayerAudioManager>();
        audioManager.player = this;
    }

    protected virtual void CreateCamera()
    {
        mainCamera = Instantiate(playerCameraPrefab, transform.position, Quaternion.identity).GetComponent<PlayerCamera>();
        mainCamera.playerObject = gameObject;
        mainCamera.transform.position = transform.position + mainCamera.offset;
    }

    protected virtual void Update()
    {
        FaceTowardsInput();
        ProcessInput();
        CheckForInteractableObjects();
    }

    protected virtual bool CheckForInteractableObjects()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, InteractRange);
        GameObject closest = FindClosestToPlayerFromOverlapSphere<InteractableObject>(cols);
        closestInteractableObject = closest;
        if (closest)
        {
            if (closest.GetComponent<InteractableObject>().CanInteract())
            {
                mainUI.ShowNotification("Press E to interact.", false);
                return true;
            }
        }
        mainUI.CloseNotification();

        return false;
    }

    protected virtual void FaceTowardsInput()
    {
        if (controllerType == ControllerType.MOUSE)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, GetRotationTowardsMouse(), (Time.deltaTime * RotationSpeed) * GetFrozenMultiplier());
        }
    }

    protected Quaternion GetRotationTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = mainCamera.GetComponentInChildren<Camera>().WorldToScreenPoint(transform.position);
        Vector3 dir = mousePos - playerPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(-angle + 90f, Vector3.up);
    }

    Vector3 GetDirectionFromMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = mainCamera.GetComponentInChildren<Camera>().WorldToScreenPoint(transform.position);
        return mousePos - playerPos;
    }

    Vector3 GetDirectionToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = mainCamera.GetComponentInChildren<Camera>().WorldToScreenPoint(transform.position);
        return playerPos - mousePos;
    }

    protected virtual void ProcessInput()
    {
        if (InventoryHidden())
        {
            //Movement
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 inputs = new Vector3(Mathf.Abs(horizontal) > 0.25f ? horizontal : 0, 0f, Mathf.Abs(vertical) > 0.25f ? vertical : 0);
            if (horizontal != 0 && vertical != 0)
            {
                inputs = inputs / 1.3f;
            }
            moveVelocity = Vector3.Lerp(moveVelocity, inputs * movespeed, Time.deltaTime * Dampening);
            moveVelocity *= GetFrozenMultiplier();
            moveVelocity.y = rb.velocity.y;

            if (Input.GetButtonDown("Interact"))
            {
                Interact();
            }

            if (Input.GetButtonDown("Save"))
            {
                GameSerializer.instance.SaveGame();
            }
        }
        else
        {
            moveVelocity = Vector3.zero;
        }

        anim.UpdateMovementAnimation(moveVelocity);

        if (Input.GetButtonDown("Inventory"))
        {
            ToggleInventory(!mainUI.inventoryHolderObject.activeInHierarchy);
        }

        if (Input.GetButtonDown("Console"))
        {
            mainUI.ToggleConsole(!mainUI.Console.activeInHierarchy);
        }
    }

    protected virtual bool InventoryHidden()
    {
        return true;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVelocity;
    }

    void Interact()
    {
        if (closestInteractableObject)
        {
            closestInteractableObject.GetComponent<InteractableObject>().Interact();
        }
    }

    public void ToggleInventory(bool toggle)
    {
        if (toggle)
        {
            FreezePlayer();
            SFXPlayer.instance.PlayEffect(openInventoryClip, 1);
        }
        else
        {
            Unfreeze();
        }

        mainUI.ToggleInventory(toggle);

    }

    GameObject FindClosestToPlayerFromOverlapSphere<T>(Collider[] objects)
    {
        GameObject closest = null;
        float closestDistance = 100000.0f;

        foreach(Collider t in objects)
        {
            if (t.gameObject.tag != "IgnoreInteraction")
            {
                float distance = Vector3.Distance(t.transform.position, transform.position);
                if (distance <= closestDistance)
                {
                    if (t.gameObject.GetComponent<T>() != null)
                    {
                        closest = t.gameObject;
                        closestDistance = distance;
                    }
                }
            }
        }

        return closest;
    }

    public void FreezePlayer()
    {
        ChangeFrozenAmount(100);
    }

    public void Unfreeze()
    {
        ChangeFrozenAmount(0);
    }

    public void ChangeFrozenAmount(float newAmount)
    {
        frozenAmount = newAmount / 100;
    }

    public float GetFrozenMultiplier()
    {
        return 1 - frozenAmount;
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    public override void Die()
    {
        if (!Dead)
        {
            base.Die();
            FreezePlayer();
            rb.velocity = Vector3.zero;
            anim.Die();
        }
    }

    public void HideMesh()
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer smr in renderers)
        {
            smr.enabled = false;
        }
    }
}
