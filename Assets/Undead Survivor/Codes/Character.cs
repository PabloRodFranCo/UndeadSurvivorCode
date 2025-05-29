/// <summary>
/// Proporciona estadísticas modificadas en función del ID del jugador activo.
/// </summary>
public class Character : MonoBehaviour
{
    /// <summary>
    /// Velocidad base del personaje. Aumenta si el jugador tiene ID 0.
    /// </summary>
    public static float Speed
    {
        get { return GameManager.Instance.playerId == 0 ? 1.1f : 1f; }
    }

    /// <summary>
    /// Velocidad de ataque del arma. Aumenta si el jugador tiene ID 1.
    /// </summary>
    public static float WeaponSpeed
    {
        get { return GameManager.Instance.playerId == 1 ? 1.1f : 1f; }
    }

    /// <summary>
    /// Frecuencia de ataque del arma (menor valor es mejor). Disminuye si el jugador tiene ID 1.
    /// </summary>
    public static float WeaponRate
    {
        get { return GameManager.Instance.playerId == 1 ? 0.9f : 1f; }
    }

    /// <summary>
    /// Daño infligido por el personaje. Aumenta si el jugador tiene ID 2.
    /// </summary>
    public static float Damage
    {
        get { return GameManager.Instance.playerId == 2 ? 1.2f : 1f; }
    }

    /// <summary>
    /// Número adicional de proyectiles o ataques simultáneos. Aumenta si el jugador tiene ID 3.
    /// </summary>
    public static int Count
    {
        get { return GameManager.Instance.playerId == 3 ? 1 : 0; }
    }
}
