using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private const float duracion = 0.8f;
    private const float velocidadFlotacion = 1.2f;
    private const float tamanoFuente = 4f;

    private TextMeshPro texto;
    private float tiempoRestante;
    private Color colorInicial;

    public static void Mostrar(Vector3 posicion, int danio, Color? color = null)
    {
        GameObject popupObj = new GameObject("DamagePopup");
        popupObj.transform.position = posicion;

        TextMeshPro texto = popupObj.AddComponent<TextMeshPro>();
        texto.text = danio.ToString();
        texto.fontSize = tamanoFuente;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = color ?? Color.yellow;

        Renderer renderer = popupObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 100;
        }

        popupObj.AddComponent<DamagePopup>();
    }

    private void Awake()
    {
        texto = GetComponent<TextMeshPro>();
        tiempoRestante = duracion;
        colorInicial = texto.color;
    }

    private void Update()
    {
        transform.position += Vector3.up * velocidadFlotacion * Time.deltaTime;

        tiempoRestante -= Time.deltaTime;

        float alpha = Mathf.Clamp01(tiempoRestante / duracion);
        texto.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);

        if (tiempoRestante <= 0)
        {
            Destroy(gameObject);
        }
    }
}
