/// <summary>
/// Controla la posición, rotación y renderizado de una mano (izquierda o derecha) del personaje,
/// ajustándose a la orientación del sprite del jugador.
/// </summary>
public class Hand : MonoBehaviour
{
    /// <summary>
    /// Indica si esta es la mano izquierda (true) o la derecha (false).
    /// </summary>
    public bool isLeft;

    /// <summary>
    /// Componente SpriteRenderer de la mano.
    /// </summary>
    public SpriteRenderer spriter;

    /// <summary>
    /// Componente SpriteRenderer del jugador (padre).
    /// </summary>
    SpriteRenderer player;

    // Posiciones predefinidas para la mano derecha.
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0f);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0f);

    // Rotaciones predefinidas para la mano izquierda.
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    /// <summary>
    /// Obtiene la referencia al SpriteRenderer del jugador.
    /// </summary>
    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    /// <summary>
    /// Ajusta posición, rotación y orden de renderizado de la mano en función de si el sprite del jugador está invertido.
    /// </summary>
    private void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
