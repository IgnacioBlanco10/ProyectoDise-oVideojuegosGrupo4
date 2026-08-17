using System.Collections;
using UnityEngine;

public class Atacar : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private Transform armaVisual;
    [SerializeField] private float rangoAtaque = 1f;
    [SerializeField] private LayerMask capaEnemigos;

    [Header("Daño")]
    [SerializeField] private int danoEspada = 10;
    [SerializeField] private int danoHacha = 20;
    [SerializeField] private int danoMartillo = 30;

    [Header("Tiempo entre ataques")]
    [SerializeField] private float tiempoEntreAtaques = 0.4f;

    [Header("Animación del arma")]
    [SerializeField] private float velocidadAnimacion = 12f;

    private float proximoAtaque = 0f;
    private bool atacando = false;

    // Posición original del punto de ataque
    private float distanciaPuntoAtaque;

    private void Start()
    {
        if (puntoAtaque != null)
        {
            distanciaPuntoAtaque = Mathf.Abs(puntoAtaque.localPosition.x);
        }

        ColocarArmaEnReposo();
    }

    private void Update()
    {
        // Mientras no ataca, mantiene la espada vertical
        // correctamente según la dirección del personaje.
        if (!atacando)
        {
            ColocarArmaEnReposo();
            ActualizarPuntoAtaque();
        }

        // Ataque con F
        if (Input.GetKeyDown(KeyCode.F) &&
            Time.time >= proximoAtaque &&
            !atacando)
        {
            AtacarEnemigo();

            if (armaVisual != null)
            {
                StartCoroutine(AnimarAtaque());
            }

            proximoAtaque = Time.time + tiempoEntreAtaques;
        }
    }

    private bool MirandoIzquierda()
    {
        if (armaVisual == null)
            return false;

        // Movimientos.cs cambia la escala X del WeaponHolder:
        // 1 = derecha
        // -1 = izquierda
        return armaVisual.localScale.x < 0;
    }

    private void ColocarArmaEnReposo()
    {
        if (armaVisual == null)
            return;

        if (MirandoIzquierda())
        {
            // Al estar invertida en X, usamos -90
            // para que visualmente apunte hacia arriba.
            armaVisual.localRotation =
                Quaternion.Euler(0f, 0f, -90f);
        }
        else
        {
            armaVisual.localRotation =
                Quaternion.Euler(0f, 0f, 90f);
        }
    }

    private void ActualizarPuntoAtaque()
    {
        if (puntoAtaque == null)
            return;

        Vector3 posicion = puntoAtaque.localPosition;

        if (MirandoIzquierda())
        {
            posicion.x = -distanciaPuntoAtaque;
        }
        else
        {
            posicion.x = distanciaPuntoAtaque;
        }

        puntoAtaque.localPosition = posicion;
    }

    private void AtacarEnemigo()
    {
        if (puntoAtaque == null)
        {
            Debug.LogWarning(
                "No se ha asignado el PuntoAtaque."
            );
            return;
        }

        Collider2D[] enemigosGolpeados =
            Physics2D.OverlapCircleAll(
                puntoAtaque.position,
                rangoAtaque,
                capaEnemigos
            );

        int dano = ObtenerDano();

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            enemigo.SendMessage(
                "RecibirDano",
                dano,
                SendMessageOptions.DontRequireReceiver
            );
        }

        Debug.Log(
            "Ataque realizado. Daño: " + dano
        );
    }

    private int ObtenerDano()
    {
        if (GameProgress.Instance == null)
        {
            return danoEspada;
        }

        int nivelTaller =
            GameProgress.Instance.nivelTaller;

        if (nivelTaller <= 1)
        {
            return danoEspada;
        }

        if (nivelTaller == 2)
        {
            return danoHacha;
        }

        return danoMartillo;
    }

    private IEnumerator AnimarAtaque()
    {
        atacando = true;

        bool izquierda = MirandoIzquierda();

        // Posición vertical inicial.
        Quaternion rotacionReposo;

        if (izquierda)
        {
            rotacionReposo =
                Quaternion.Euler(0f, 0f, -90f);
        }
        else
        {
            rotacionReposo =
                Quaternion.Euler(0f, 0f, 90f);
        }

        // Horizontal:
        // con escala normal apunta a la derecha;
        // con escala X negativa apunta a la izquierda.
        Quaternion rotacionGolpe =
            Quaternion.Euler(0f, 0f, 0f);

        // Baja de vertical a horizontal
        while (
            Quaternion.Angle(
                armaVisual.localRotation,
                rotacionGolpe
            ) > 1f
        )
        {
            armaVisual.localRotation =
                Quaternion.RotateTowards(
                    armaVisual.localRotation,
                    rotacionGolpe,
                    velocidadAnimacion * 100f *
                    Time.deltaTime
                );

            yield return null;
        }

        armaVisual.localRotation = rotacionGolpe;

        // Pausa pequeña para que el golpe se aprecie
        yield return new WaitForSeconds(0.08f);

        // Regresa de horizontal a vertical
        while (
            Quaternion.Angle(
                armaVisual.localRotation,
                rotacionReposo
            ) > 1f
        )
        {
            armaVisual.localRotation =
                Quaternion.RotateTowards(
                    armaVisual.localRotation,
                    rotacionReposo,
                    velocidadAnimacion * 100f *
                    Time.deltaTime
                );

            yield return null;
        }

        armaVisual.localRotation = rotacionReposo;

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