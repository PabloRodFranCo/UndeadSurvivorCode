/// <summary>
/// Hace que un UI element siga la posición del jugador en pantalla.
/// </summary>
public class Follow : MonoBehaviour
{
    RectTransform rect;

    /// <summary>
    /// Obtiene el componente RectTransform al iniciar.
    /// </summary>
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Actualiza la posición del elemento UI para que siga al jugador en la pantalla.
    /// </summary>
    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
    }
}
