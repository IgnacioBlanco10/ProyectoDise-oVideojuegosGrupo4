using UnityEngine;
using TMPro;

public class EstacionMejoras : MonoBehaviour
{
    public enum TipoEstacion
    {
        Taller,
        Laboratorio
    }

    [Header("Tipo de estación")]
    [SerializeField] private TipoEstacion tipoEstacion;

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Textos")]
    [SerializeField] private TMP_Text nivelText;
    [SerializeField] private TMP_Text descripcionText;
    [SerializeField] private TMP_Text materialesText;
    [SerializeField] private TMP_Text costoText;

    private bool jugadorCerca = false;

    private void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            panel.SetActive(!panel.activeSelf);

            if (panel.activeSelf)
            {
                ActualizarInterfaz();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movimientos>() != null)
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Movimientos>() != null)
        {
            jugadorCerca = false;

            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    public void ComprarMejora()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogError("No se encontró GameProgress.");
            return;
        }

        bool compraExitosa;

        if (tipoEstacion == TipoEstacion.Taller)
        {
            compraExitosa = GameProgress.Instance.MejorarTaller();
        }
        else
        {
            compraExitosa = GameProgress.Instance.MejorarLaboratorio();
        }

        if (!compraExitosa)
        {
            Debug.Log("No hay suficientes materiales o ya alcanzaste el nivel máximo.");
        }

        ActualizarInterfaz();
    }

    private void ActualizarInterfaz()
    {
        if (GameProgress.Instance == null)
            return;

        int nivel;
        int costo;

        if (tipoEstacion == TipoEstacion.Taller)
        {
            nivel = GameProgress.Instance.nivelTaller;
            costo = GameProgress.Instance.ObtenerCostoTaller();

            switch (nivel)
            {
                case 0:
                    descripcionText.text = "Siguiente mejora:\nEspada mejorada";
                    break;

                case 1:
                    descripcionText.text = "Siguiente mejora:\nHacha de Titanio";
                    break;

                case 2:
                    descripcionText.text = "Siguiente mejora:\nMartillo Gravitacional";
                    break;

                default:
                    descripcionText.text = "Todas las armas desbloqueadas";
                    break;
            }
        }
        else
        {
            nivel = GameProgress.Instance.nivelLaboratorio;
            costo = GameProgress.Instance.ObtenerCostoLaboratorio();

            switch (nivel)
            {
                case 0:
                    descripcionText.text = "Siguiente mejora:\nDash mejorado";
                    break;

                case 1:
                    descripcionText.text = "Siguiente mejora:\nTriple salto";
                    break;

                case 2:
                    descripcionText.text = "Siguiente mejora:\nEscudo de energía";
                    break;

                default:
                    descripcionText.text = "Todas las habilidades desbloqueadas";
                    break;
            }
        }

        nivelText.text = "Nivel actual: " + nivel;
        materialesText.text = "Materiales: " + GameProgress.Instance.materiales;

        if (nivel >= 3)
        {
            costoText.text = "NIVEL MÁXIMO";
        }
        else
        {
            costoText.text = "Costo: " + costo + " materiales";
        }
    }
}