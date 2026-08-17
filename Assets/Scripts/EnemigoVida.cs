using UnityEngine;

public class EnemigoVida : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 60;

    private int vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDano(int dano)
    {
        vidaActual -= dano;

        Debug.Log("Enemigo recibió " + dano + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("Enemigo derrotado");
        Destroy(gameObject);
    }
}