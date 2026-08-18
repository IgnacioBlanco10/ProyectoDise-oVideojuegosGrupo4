using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float homingDuration = 2.5f;

    [Header("Visual")]
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private Color color = new Color(1f, 0.2f, 0.2f);
    [SerializeField] private float radius = 0.15f;

    private Transform objetivo;
    private float tiempoRestante;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (vfxPrefab != null)
        {
            Instantiate(vfxPrefab, transform.position, Quaternion.identity, transform);

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
        }
        else if (spriteRenderer != null && spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = CrearSpriteCirculo();
            spriteRenderer.color = color;
        }
    }

    private void Start()
    {
        tiempoRestante = homingDuration;

        Movimientos jugador = FindFirstObjectByType<Movimientos>();

        if (jugador != null)
        {
            objetivo = jugador.transform;
        }
    }

    private void Update()
    {
        if (tiempoRestante <= 0)
        {
            Destroy(gameObject);
            return;
        }

        tiempoRestante -= Time.deltaTime;

        if (objetivo != null)
        {
            Vector2 direccion = (objetivo.position - transform.position).normalized;
            transform.position += (Vector3)(direccion * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movimientos>() != null)
        {
            VidasyGameOver sistemaVidas = FindFirstObjectByType<VidasyGameOver>();

            if (sistemaVidas != null)
            {
                sistemaVidas.ReduceLives(transform.position);
            }

            Destroy(gameObject);
        }
    }

    private Sprite CrearSpriteCirculo()
    {
        int tamano = 32;
        Texture2D textura = new Texture2D(tamano, tamano);
        Vector2 centro = new Vector2(tamano / 2f, tamano / 2f);
        float radioPixeles = tamano / 2f - 1f;

        for (int x = 0; x < tamano; x++)
        {
            for (int y = 0; y < tamano; y++)
            {
                float distancia = Vector2.Distance(new Vector2(x, y), centro);
                textura.SetPixel(x, y, distancia <= radioPixeles ? Color.white : Color.clear);
            }
        }

        textura.filterMode = FilterMode.Bilinear;
        textura.Apply();

        return Sprite.Create(
            textura,
            new Rect(0, 0, tamano, tamano),
            new Vector2(0.5f, 0.5f),
            tamano / (radius * 2f)
        );
    }
}
