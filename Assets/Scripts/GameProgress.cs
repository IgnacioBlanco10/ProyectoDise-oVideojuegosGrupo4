using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    public int materiales = 100;
    public int nivelTaller = 0;
    public int nivelLaboratorio = 0;

    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ReiniciarProgreso();
    }   

    public int ObtenerCostoTaller()
    {
        return ObtenerCosto(nivelTaller);
    }

    public int ObtenerCostoLaboratorio()
    {
        return ObtenerCosto(nivelLaboratorio);
    }

    private int ObtenerCosto(int nivelActual)
    {
        switch (nivelActual)
        {
            case 0:
                return 10;

            case 1:
                return 20;

            case 2:
                return 30;

            default:
                return 0;
        }
    }

    public bool MejorarTaller()
    {
        if (nivelTaller >= 3)
            return false;

        int costo = ObtenerCostoTaller();

        if (materiales < costo)
            return false;

        materiales -= costo;
        nivelTaller++;

        GuardarProgreso();

        return true;
    }

    public bool MejorarLaboratorio()
    {
        if (nivelLaboratorio >= 3)
            return false;

        int costo = ObtenerCostoLaboratorio();

        if (materiales < costo)
            return false;

        materiales -= costo;
        nivelLaboratorio++;

        GuardarProgreso();

        return true;
    }

    public void AgregarMateriales(int cantidad)
    {
        materiales += cantidad;
        GuardarProgreso();
    }

    private void GuardarProgreso()
    {
        PlayerPrefs.SetInt("Materiales", materiales);
        PlayerPrefs.SetInt("NivelTaller", nivelTaller);
        PlayerPrefs.SetInt("NivelLaboratorio", nivelLaboratorio);

        PlayerPrefs.Save();
    }

    private void CargarProgreso()
    {
        materiales = PlayerPrefs.GetInt("Materiales", 100);
        nivelTaller = PlayerPrefs.GetInt("NivelTaller", 0);
        nivelLaboratorio = PlayerPrefs.GetInt("NivelLaboratorio", 0);
    }

    public void ReiniciarProgreso()
    {
        PlayerPrefs.DeleteKey("Materiales");
        PlayerPrefs.DeleteKey("NivelTaller");
        PlayerPrefs.DeleteKey("NivelLaboratorio");

        materiales = 100;
        nivelTaller = 0;
        nivelLaboratorio = 0;
    }
}