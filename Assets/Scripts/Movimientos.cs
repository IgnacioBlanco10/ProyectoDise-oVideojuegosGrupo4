using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimientos : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed;

    [Header("Suelo")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashMejoradoSpeed = 18f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.8f;

    [Header("Empuje al recibir daño")]
    [SerializeField] private float empujeFuerza = 6f;
    [SerializeField] private float empujeDuracion = 0.15f;

    private Transform weaponHolder;

    private Rigidbody2D body;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

    // Dash
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldownRemaining = 0f;
    private int direccion = 1;

    // Empuje
    private bool recibiendoEmpuje = false;
    private float empujeTiempoRestante = 0f;

    // Saltos
    private int saltosRestantes;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        weaponHolder = transform.Find("WeaponHolder");

        if (weaponHolder == null)
        {
            Debug.LogError("No se encontró WeaponHolder como hijo del jugador.");
        }

        saltosRestantes = 2;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Dash
        if (dashCooldownRemaining > 0)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;

            if (dashTimeRemaining <= 0)
            {
                isDashing = false;
            }
            else
            {
                return;
            }
        }

        // Empuje al recibir daño
        if (recibiendoEmpuje)
        {
            empujeTiempoRestante -= Time.deltaTime;

            if (empujeTiempoRestante <= 0)
            {
                recibiendoEmpuje = false;
            }
            else
            {
                return;
            }
        }

        // Movimiento
        body.velocity = new Vector2(
            horizontalInput * speed,
            body.velocity.y
        );

        // Saltos
        int maxSaltos = 2;

        if (GameProgress.Instance != null &&
            GameProgress.Instance.nivelLaboratorio >= 2)
        {
            maxSaltos = 3;
        }

        if (IsGrounded() && body.velocity.y <= 0.01f)
        {
            saltosRestantes = maxSaltos;
        }

        if (Input.GetKeyDown(KeyCode.Space) &&
            saltosRestantes > 0)
        {
            if (!IsGrounded())
            {
                playerAnimator.SetTrigger("DoubleJump");
            }

            body.velocity = new Vector2(
                body.velocity.x,
                speed
            );

            saltosRestantes--;
        }

        // Activar dash
        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            dashCooldownRemaining <= 0)
        {
            HacerDash();
        }

        // Animaciones
        playerAnimator.SetFloat("Moving", Mathf.Abs(horizontalInput));
        playerAnimator.SetBool("isGrounded", IsGrounded());

        // Dirección
        if (horizontalInput > 0.01f)
        {
            direccion = 1;
            spriteRenderer.flipX = false;

            if (weaponHolder != null)
            {
                weaponHolder.localScale = new Vector3(1, 1, 1);

                weaponHolder.localPosition = new Vector3(
                    0f,
                    weaponHolder.localPosition.y,
                    weaponHolder.localPosition.z
                );
            }
        }
        else if (horizontalInput < -0.01f)
        {
            direccion = -1;
            spriteRenderer.flipX = true;

            if (weaponHolder != null)
            {
                weaponHolder.localScale = new Vector3(-1, 1, 1);

                weaponHolder.localPosition = new Vector3(
                    0f,
                    weaponHolder.localPosition.y,
                    weaponHolder.localPosition.z
                );
            }
        }
    }

    // Empuje al recibir daño
    public void AplicarEmpuje(Vector2 direccionEmpuje)
    {
        recibiendoEmpuje = true;
        empujeTiempoRestante = empujeDuracion;

        body.velocity = direccionEmpuje.normalized * empujeFuerza;
    }

    // Dash
    private void HacerDash()
    {
        float velocidadDash = dashSpeed;

        if (GameProgress.Instance != null &&
            GameProgress.Instance.nivelLaboratorio >= 1)
        {
            velocidadDash = dashMejoradoSpeed;
        }

        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;

        body.velocity = new Vector2(
            direccion * velocidadDash,
            0f
        );
    }

    // Suelo
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
}