using UnityEngine;

public class EnemigoVida : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 60;

    [Header("Empuje al recibir daño")]
    [SerializeField] private float fuerzaEmpuje = 4f;
    [SerializeField] private float duracionEmpuje = 0.15f;

    [Header("Recompensa (opcional, solo si este enemigo debe soltar algo al morir)")]
    [SerializeField] private GameObject itemAlMorir;
    [SerializeField] private Transform puntoSpawnItem;

    private int vidaActual;
    private Rigidbody2D rb;
    private SlimePatrol patrol;

    private void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        patrol = GetComponent<SlimePatrol>();
    }

    public void RecibirDano(int dano, Vector2? origenGolpe = null)
    {
        vidaActual -= dano;

        Debug.Log("Enemigo recibió " + dano + " de daño. Vida restante: " + vidaActual);

        if (origenGolpe.HasValue && rb != null)
        {
            Vector2 direccion = ((Vector2)transform.position - origenGolpe.Value).normalized;
            rb.velocity = direccion * fuerzaEmpuje;

            if (patrol != null)
            {
                patrol.PausarPorEmpuje(duracionEmpuje);
            }
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("Enemigo derrotado");

        if (itemAlMorir != null)
        {
            Vector3 posicion = puntoSpawnItem != null ? puntoSpawnItem.position : transform.position;
            Instantiate(itemAlMorir, posicion, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
