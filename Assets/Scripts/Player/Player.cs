using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region INSTANCE VARIABLES
    public static Player Instance { get; private set; }

    [Header("References")]
    private CharacterController characterController;
    private Player player;
    public Animator animator;

    [Header("Input")]
    private PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction lookAction;
    public InputAction shootAction;
    public InputAction reloadAction;
    public InputAction meleeAction;
    public InputAction lassoAction;
    public InputAction swapAction;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public event Action OnHealthChange;
    public static event Action OnDeath;

    [Header("Weapons")]
    public Transform hand;
    public List<GameObject> weapons;
    public Weapon currentWeapon;
    public event Action OnWeaponSwap;

    [Header("Upgrades")]
    public List<Upgrade> upgrades;

    [Header("Abilities")]
    public float meleeDamage;
    public float meleeRange;
    public float meleeCooldown;
    public LayerMask enemyMask;
    public event Action OnMeleeCooldown;

    [Header("Camera")]
    public new Transform camera;
    [SerializeField]
    private float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("Movement")]
    public float movementSpeed = 5f;
    public float groundSpeed = 5f;
    public float airSpeed = 2.5f;
    private Vector3 previousPosition;
    private Vector3 movementVelocity;
    private Vector3 velocity;

    [Header("Jump")]
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float gravityMultiplier = 10f;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    #endregion

    #region INSTANTIATION

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        upgrades = new List<Upgrade>();
        characterController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        previousPosition = characterController.transform.position;

        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        lookAction = playerInput.actions.FindAction("Look");
        shootAction = playerInput.actions.FindAction("Shoot");
        reloadAction = playerInput.actions.FindAction("Reload");
        meleeAction = playerInput.actions.FindAction("Melee");
        lassoAction = playerInput.actions.FindAction("Lasso");
        swapAction = playerInput.actions.FindAction("Swap");
    }

    #endregion

    #region HEALTH

    public void SubtractHealth(float amount)
    {
        currentHealth -= amount;
        OnHealthChange.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }

    #endregion

    #region ABILITIES

    public void ApplyUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        upgrade.ApplyUpgrade(this);
    }

    public void Melee()
    {
        StartCoroutine(MeleeCooldown());
    }

    private IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        OnMeleeCooldown?.Invoke();
    }

    #endregion

    #region WEAPON

    public void ApplyWeapon(GameObject weapon)
    {
        if (currentWeapon)
        {
            currentWeapon.gameObject.SetActive(false);
        }
        weapons.Add(weapon);
        currentWeapon = weapon.GetComponent<Weapon>();
        OnWeaponSwap?.Invoke();
    }

    public void Shoot()
    {
        if (currentWeapon)
        {
            currentWeapon.Shoot();
        }
    }

    public void Reload()
    {
        if (currentWeapon)
        {
            currentWeapon.Reload();
        }
    }

    public void SwapWeapon(float direction)
    {
        if (currentWeapon == null)
        {
            return;
        }

        currentWeapon.gameObject.SetActive(false);

        int currentIndex = weapons.IndexOf(player.currentWeapon.gameObject);
        int nextIndex;

        if (direction > 0) // upwards/positive direction
        {
            nextIndex = (currentIndex + 1) % weapons.Count;
        }
        else // downwards/negative direction
        {
            nextIndex = (currentIndex - 1 + weapons.Count) % weapons.Count;
        }
        currentWeapon = weapons[nextIndex].GetComponent<Weapon>();
        currentWeapon.gameObject.SetActive(true);
        OnWeaponSwap?.Invoke();
    }

    #endregion

    #region MOVEMENT

    public void Look(Vector2 lookVector)
    {
        float mouseX = lookVector.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookVector.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void Move(Vector2 movementInput)
    {
        Vector3 movementVector = transform.right * movementInput.x + transform.forward * movementInput.y;

        characterController.Move(movementVector * movementSpeed * Time.fixedDeltaTime);

        movementVelocity = (characterController.transform.position - previousPosition) / Time.deltaTime;
        animator.SetFloat("Velocity X", movementVelocity.x);
        animator.SetFloat("Velocity Z", movementVelocity.z);

        previousPosition = characterController.transform.position;
    }

    public void ApplyGravity(bool grounded)
    {
        if (grounded)
        {
            velocity.y = -2f;
        }

        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        characterController.Move(velocity * Time.deltaTime);
        animator.Play("JUMP");
    }

    public float GetSpeed()
    {
        return movementVelocity.magnitude;
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    #endregion
}