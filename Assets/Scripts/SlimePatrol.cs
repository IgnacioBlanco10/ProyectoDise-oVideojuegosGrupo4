using UnityEngine;

public class SlimePatrol : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float patrolDistance = 3f;

    private Vector3 puntoInicial;
    private int direccion = 1;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private float pausaEmpujeRestante = 0f;

    private void Start()
    {
        puntoInicial = transform.position;
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PausarPorEmpuje(float duracion)
    {
        pausaEmpujeRestante = duracion;
    }

    private void FixedUpdate()
    {
        if (pausaEmpujeRestante > 0)
        {
            pausaEmpujeRestante -= Time.fixedDeltaTime;
            return;
        }

        float distanciaRecorrida = transform.position.x - puntoInicial.x;

        if (distanciaRecorrida > patrolDistance)
        {
            direccion = -1;
        }
        else if (distanciaRecorrida < -patrolDistance)
        {
            direccion = 1;
        }

        body.velocity = new Vector2(direccion * speed, body.velocity.y);

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direccion < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Movimientos>() != null)
        {
            VidasyGameOver sistemaVidas = FindFirstObjectByType<VidasyGameOver>();

            if (sistemaVidas != null)
            {
                sistemaVidas.ReduceLives(transform.position);
            }
        }
    }
}
