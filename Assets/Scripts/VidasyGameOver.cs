using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VidasyGameOver : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damagePerHit = 20;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private GameObject GameOver;

    [Header("Invulnerabilidad")]
    [SerializeField] private float tiempoInvulnerabilidad = 2.5f;

    [Header("Escudo")]
    [SerializeField] private float shieldCooldown = 5f;

    private int currentHealth;

    private bool escudoDisponible = true;
    private bool invulnerable = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (GameOver != null)
        {
            GameOver.SetActive(false);
        }
    }

    // Daño
    public void ReduceLives(Vector2? origenGolpe = null)
    {
        // Si está en periodo de invulnerabilidad,
        // no recibe otro golpe.
        if (invulnerable)
        {
            Debug.Log("Jugador invulnerable. Daño ignorado.");
            return;
        }

        // Si el laboratorio está en nivel 3,
        // el escudo bloquea el golpe.
        if (GameProgress.Instance != null &&
            GameProgress.Instance.nivelLaboratorio >= 3 &&
            escudoDisponible)
        {
            ActivarEscudo();
            return;
        }

        currentHealth -= damagePerHit;
        UpdateHealthBar();

        Debug.Log(
            "Jugador recibió daño. Vida restante: "
            + currentHealth
        );

        Movimientos jugador = FindFirstObjectByType<Movimientos>();

        if (jugador != null)
        {
            DamagePopup.Mostrar(jugador.transform.position + Vector3.up * 0.5f, damagePerHit, Color.red);

            Animator animadorJugador = jugador.GetComponent<Animator>();

            if (animadorJugador != null)
            {
                animadorJugador.SetTrigger("Hit");
            }

            if (origenGolpe.HasValue)
            {
                Vector2 direccionEmpuje = (Vector2)jugador.transform.position - origenGolpe.Value;
                jugador.AplicarEmpuje(direccionEmpuje);
            }
        }

        if (currentHealth <= 0)
        {
            ShowGameOver();
            return;
        }

        // Después de recibir daño empieza
        // el periodo de invulnerabilidad.
        StartCoroutine(InvulnerabilidadTemporal());
    }

    // Muerte inmediata (por ejemplo, sin oxígeno), sin escudo ni invulnerabilidad.
    public void Morir()
    {
        currentHealth = 0;
        UpdateHealthBar();
        ShowGameOver();
    }

    // Invulnerabilidad después de recibir daño
    private IEnumerator InvulnerabilidadTemporal()
    {
        invulnerable = true;

        Debug.Log(
            "Invulnerabilidad activada por "
            + tiempoInvulnerabilidad
            + " segundos."
        );

        yield return new WaitForSeconds(
            tiempoInvulnerabilidad
        );

        invulnerable = false;

        Debug.Log("Invulnerabilidad terminada.");
    }

    // Escudo
    private void ActivarEscudo()
    {
        escudoDisponible = false;

        Debug.Log(
            "¡Escudo de energía activado! Daño bloqueado."
        );

        StartCoroutine(RecargarEscudo());
    }

    private IEnumerator RecargarEscudo()
    {
        yield return new WaitForSeconds(
            shieldCooldown
        );

        escudoDisponible = true;

        Debug.Log("Escudo de energía recargado.");
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void ShowGameOver()
    {
        if (GameOver != null)
        {
            GameOver.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        if (GameOver != null)
        {
            GameOver.SetActive(false);
        }

        ResetLives();

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ResetLives()
    {
        currentHealth = maxHealth;

        escudoDisponible = true;
        invulnerable = false;

        UpdateHealthBar();
    }
}
