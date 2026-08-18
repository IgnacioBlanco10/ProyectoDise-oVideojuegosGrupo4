using UnityEngine;
using UnityEngine.UI;

public class OxygenTimer : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 60f;
    [SerializeField] private Image oxygenBarFill;

    private float oxigenoActual;
    private bool muerto = false;

    private void Start()
    {
        oxigenoActual = maxOxygen;
        ActualizarBarra();
    }

    private void Update()
    {
        if (muerto)
            return;

        oxigenoActual -= Time.deltaTime;

        if (oxigenoActual <= 0)
        {
            oxigenoActual = 0;
            muerto = true;

            VidasyGameOver sistemaVidas = FindFirstObjectByType<VidasyGameOver>();

            if (sistemaVidas != null)
            {
                sistemaVidas.Morir();
            }
        }

        ActualizarBarra();
    }

    private void ActualizarBarra()
    {
        if (oxygenBarFill != null)
        {
            oxygenBarFill.fillAmount = oxigenoActual / maxOxygen;
        }
    }
}
