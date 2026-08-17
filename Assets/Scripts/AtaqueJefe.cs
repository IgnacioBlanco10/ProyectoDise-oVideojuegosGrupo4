using System.Collections;
using UnityEngine;

public class AtaqueJefe : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float rangoAtaque = 1.5f;
    [SerializeField] private LayerMask capaJugador;

    [Header("Tiempo")]
    [SerializeField] private float tiempoEntreAtaques = 5f;
    [SerializeField] private float duracionAtaque = 0.5f;

    [Header("Animacion")]
    [SerializeField] private Animator animador;

    private float siguienteAtaque = 0f;
    private bool atacando = false;

    private VidasyGameOver sistemaVidas;

    private void Start()
    {
        sistemaVidas = FindObjectOfType<VidasyGameOver>();

        if (sistemaVidas == null)
        {
            Debug.LogError("No se encontro VidasyGameOver.");
        }

        if (puntoAtaque == null)
        {
            Debug.LogError("No se asigno PuntoAtaqueJefe.");
        }
    }

    private void Update()
    {
        if (atacando)
            return;

        if (Time.time < siguienteAtaque)
            return;

        if (puntoAtaque == null)
            return;

        // Solo comienza el ataque si el jugador esta dentro del rango.
        Collider2D jugador = Physics2D.OverlapCircle(
            puntoAtaque.position,
            rangoAtaque,
            capaJugador
        );

        if (jugador != null)
        {
            StartCoroutine(RealizarAtaque());
            siguienteAtaque = Time.time + tiempoEntreAtaques;
        }
    }

    private IEnumerator RealizarAtaque()
    {
        atacando = true;

        if (animador != null)
        {
            animador.SetTrigger("Atacar");
        }

        // Espera hasta la mitad de la animacion.
        yield return new WaitForSeconds(duracionAtaque / 2f);

        // Comprueba OTRA VEZ si el jugador sigue cerca.
        Collider2D jugadorGolpeado = Physics2D.OverlapCircle(
            puntoAtaque.position,
            rangoAtaque,
            capaJugador
        );

        if (jugadorGolpeado != null && sistemaVidas != null)
        {
            sistemaVidas.ReduceLives();

            Debug.Log(
                "Boss golpeo al jugador."
            );
        }
        else
        {
            Debug.Log(
                "El jugador salio del rango y esquivo el ataque."
            );
        }

        yield return new WaitForSeconds(
            duracionAtaque / 2f
        );

        atacando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque == null)
            return;

        Gizmos.DrawWireSphere(
            puntoAtaque.position,
            rangoAtaque
        );
    }
}