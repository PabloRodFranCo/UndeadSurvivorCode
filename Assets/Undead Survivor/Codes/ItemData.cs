/// <summary>
/// Define los datos de un ítem del juego, incluyendo su tipo, nombre, niveles de mejora y comportamiento visual o funcional.
/// Este ScriptableObject puede ser instanciado desde el menú de Unity.
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// Tipos posibles de ítems.
    /// </summary>
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    /// <summary>
    /// Tipo del ítem (arma, accesorio o curación).
    /// </summary>
    public ItemType itemType;

    /// <summary>
    /// Identificador único del ítem.
    /// </summary>
    public int itemId;

    /// <summary>
    /// Nombre del ítem mostrado al jugador.
    /// </summary>
    public string itemName;

    /// <summary>
    /// Descripción del ítem, puede incluir valores formateables.
    /// </summary>
    [TextArea]
    public string itemDesc;

    /// <summary>
    /// Icono que representa al ítem en la interfaz.
    /// </summary>
    public Sprite itemIcon;

    [Header("# Level Data")]
    /// <summary>
    /// Daño base que inflige el ítem en su primer nivel.
    /// </summary>
    public float baseDamage;

    /// <summary>
    /// Cantidad base de ataques/proyectiles en su primer nivel.
    /// </summary>
    public int baseCount;

    /// <summary>
    /// Lista de multiplicadores de daño por nivel.
    /// </summary>
    public float[] damages;

    /// <summary>
    /// Lista de incrementos de cantidad por nivel.
    /// </summary>
    public int[] counts;

    [Header("# Weapon")]
    /// <summary>
    /// Prefab del proyectil utilizado si el ítem es un arma a distancia.
    /// </summary>
    public GameObject projectile;

    /// <summary>
    /// Sprite que se muestra en la mano si el ítem es un arma.
    /// </summary>
    public Sprite hand;
}
