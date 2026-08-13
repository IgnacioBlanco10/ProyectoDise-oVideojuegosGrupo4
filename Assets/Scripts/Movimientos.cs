using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimientos : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Transform weaponHolder;

    private Rigidbody2D body;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

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
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Movimiento horizontal
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, speed);
        }

        // Animaciones
        playerAnimator.SetFloat("Moving", Mathf.Abs(horizontalInput));
        playerAnimator.SetBool("isGrounded", IsGrounded());

        // Mirar hacia la derecha
        if (horizontalInput > 0.01f)
        {
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

        // Mirar hacia la izquierda
        else if (horizontalInput < -0.01f)
        {
            spriteRenderer.flipX = true;

            if (weaponHolder != null)
            {
                weaponHolder.localScale = new Vector3(-1, 1, 1);

                weaponHolder.localPosition = new Vector3(
                    -0.25f,
                    weaponHolder.localPosition.y,
                    weaponHolder.localPosition.z
                );
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
}