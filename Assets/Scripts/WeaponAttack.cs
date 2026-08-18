using System.Collections;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform hilt;

    [Header("Ataque")]
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackAngle = -70f;
    [SerializeField] private float attackCooldown = 0.35f;

    [Header("Daño")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float hitRadius = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Idle - sostener arma")]
    [SerializeField] private float idleSwayAmplitude = 4f;
    [SerializeField] private float idleSwaySpeed = 2f;

    private Transform weapon;
    private SpriteRenderer bodyRenderer;
    private Vector3 restLocalPosition;
    private Quaternion restLocalRotation;
    private bool isAttacking = false;
    private float cooldownRemaining = 0f;
    private float attackAngleCurrent = 0f;

    void Start()
    {
        if (playerAnimator == null)
            playerAnimator = GetComponent<Animator>();

        bodyRenderer = GetComponent<SpriteRenderer>();

        Transform weaponHolder = transform.Find("WeaponHolder");
        if (weaponHolder != null)
            weapon = weaponHolder.Find("Weapon");

        if (weapon == null)
        {
            Debug.LogError("No se encontró Weapon dentro de WeaponHolder.");
            return;
        }

        if (hand == null || handLeft == null || hilt == null)
            Debug.LogError("Asigná los empties 'hand', 'handLeft' e 'hilt' en el Inspector de WeaponAttack.");

        restLocalPosition = weapon.localPosition;
        restLocalRotation = weapon.localRotation;
    }

    void LateUpdate()
    {
        if (weapon == null || hand == null || handLeft == null || hilt == null)
            return;

        if (cooldownRemaining > 0)
            cooldownRemaining -= Time.deltaTime;

        if (Input.GetKeyDown(attackKey) && !isAttacking && cooldownRemaining <= 0)
        {
            StartCoroutine(Atacar());
        }

        float targetAngle = isAttacking
            ? attackAngleCurrent
            : Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmplitude;

        Transform currentHand = (bodyRenderer != null && bodyRenderer.flipX) ? handLeft : hand;

        // Vuelve a la pose base (arma en reposo, sin desplazamientos previos)
        weapon.localPosition = restLocalPosition;
        weapon.localRotation = restLocalRotation;

        // Alinea el hilt (punto de agarre del arma) con la mano correspondiente
        Vector3 delta = currentHand.position - hilt.position;
        weapon.position += delta;

        // Rota el arma alrededor del hilt, no de su propio centro
        weapon.RotateAround(hilt.position, Vector3.forward, targetAngle);
    }

    private IEnumerator Atacar()
    {
        isAttacking = true;
        cooldownRemaining = attackCooldown;

        if (playerAnimator != null)
            playerAnimator.SetTrigger("Attack");

        float half = attackDuration * 0.5f;
        float elapsed = 0f;

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            attackAngleCurrent = Mathf.Lerp(0, attackAngle, elapsed / half);
            yield return null;
        }

        // Punto de máxima extensión del golpe: se comprueba el impacto una sola vez.
        GolpearEnemigos();

        elapsed = 0f;

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            attackAngleCurrent = Mathf.Lerp(attackAngle, 0, elapsed / half);
            yield return null;
        }

        attackAngleCurrent = 0f;
        isAttacking = false;
    }

    private void GolpearEnemigos()
    {
        Collider2D[] golpeados = Physics2D.OverlapCircleAll(
            weapon.position,
            hitRadius,
            enemyLayer
        );

        foreach (Collider2D golpeado in golpeados)
        {
            EnemigoVida vida = golpeado.GetComponentInParent<EnemigoVida>();

            if (vida != null)
            {
                vida.RecibirDano(damage, weapon.position);
                DamagePopup.Mostrar(golpeado.transform.position + Vector3.up * 0.3f, damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weapon == null)
            return;

        Gizmos.DrawWireSphere(weapon.position, hitRadius);
    }
}
