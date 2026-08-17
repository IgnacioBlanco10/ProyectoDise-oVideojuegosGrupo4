using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VidasyGameOver : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject GameOver;

    [Header("Invulnerabilidad")]
    [SerializeField] private float tiempoInvulnerabilidad = 2.5f;

    [Header("Escudo")]
    [SerializeField] private float shieldCooldown = 5f;

    private int currentLives;

    private bool escudoDisponible = true;
    private bool invulnerable = false;

    private void Start()
    {
        currentLives = startingLives;
        UpdateLives();

        if (GameOver != null)
        {
            GameOver.SetActive(false);
        }
    }

    // Daño
    public void ReduceLives()
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

        // Quita una vida.
        currentLives--;
        UpdateLives();

        Debug.Log(
            "Jugador recibió daño. Vidas restantes: "
            + currentLives
        );

        if (currentLives <= 0)
        {
            ShowGameOver();
            return;
        }

        // Después de recibir daño empieza
        // el periodo de invulnerabilidad.
        StartCoroutine(InvulnerabilidadTemporal());
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

    private void UpdateLives()
    {
        if (livesText != null)
        {
            livesText.text =
                "Lives: " + currentLives;
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
        currentLives = startingLives;

        escudoDisponible = true;
        invulnerable = false;

        UpdateLives();
    }
}