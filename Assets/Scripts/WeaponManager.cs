using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Sprites de armas")]
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite titaniumAxeSprite;
    [SerializeField] private Sprite gravityHammerSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ActualizarArma();
    }

    private void Update()
    {
        ActualizarArma();
    }

    private void ActualizarArma()
    {
        if (GameProgress.Instance == null)
            return;

        int nivel = GameProgress.Instance.nivelTaller;

        if (nivel <= 1)
        {
            spriteRenderer.sprite = swordSprite;
        }
        else if (nivel == 2)
        {
            spriteRenderer.sprite = titaniumAxeSprite;
        }
        else if (nivel >= 3)
        {
            spriteRenderer.sprite = gravityHammerSprite;
        }
    }
}