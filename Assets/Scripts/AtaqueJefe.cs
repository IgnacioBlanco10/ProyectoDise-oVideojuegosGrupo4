using System.Collections;
using UnityEngine;

public class AtaqueJefe : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float rangoAtaque = 12f;
    [SerializeField] private LayerMask capaJugador;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Vector2 offsetDisparo = new Vector2(0.8f, 0.2f);

    [Header("Tiempo")]
    [SerializeField] private float tiempoEntreAtaques = 5f;
    [SerializeField] private float duracionAtaque = 0.5f;

    [Header("Animacion")]
    [SerializeField] private Animator animador;

    private float siguienteAtaque = 0f;
    private bool atacando = false;

    private void Start()
    {
        if (puntoAtaque == null)
        {
            Debug.LogError("No se asigno PuntoAtaqueJefe.");
        }

        if (proyectilPrefab == null)
        {
            Debug.LogError("No se asigno el prefab del proyectil.");
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

        // Dispara solo si el jugador esta dentro del rango de deteccion.
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

        // Espera hasta la mitad de la animacion antes de disparar.
        yield return new WaitForSeconds(duracionAtaque / 2f);

        if (proyectilPrefab != null)
        {
            Vector3 posicionDisparo = transform.TransformPoint(offsetDisparo);
            Instantiate(proyectilPrefab, posicionDisparo, Quaternion.identity);
        }

        yield return new WaitForSeconds(duracionAtaque / 2f);

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
