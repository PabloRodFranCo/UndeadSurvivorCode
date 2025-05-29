/// <summary>
/// Gestiona la interfaz de subida de nivel y la selección de mejoras del jugador.
/// </summary>
public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    /// <summary>
    /// Inicializa el componente UI y los ítems disponibles.
    /// </summary>
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    /// <summary>
    /// Muestra el panel de subida de nivel y detiene el juego temporalmente.
    /// </summary>
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    /// <summary>
    /// Oculta el panel de subida de nivel y reanuda el juego.
    /// </summary>
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.Instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    /// <summary>
    /// Aplica la mejora seleccionada por el jugador.
    /// </summary>
    /// <param name="index">Índice del ítem seleccionado.</param>
    public void Select(int index)
    {
        items[index].OnClick();
    }

    /// <summary>
    /// Selecciona aleatoriamente tres ítems únicos y los muestra.
    /// </summary>
    void Next()
    {
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                break;
        }

        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
