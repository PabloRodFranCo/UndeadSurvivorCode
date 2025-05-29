/// <summary>
/// Representa una mejora (gear) aplicada al jugador, que puede modificar velocidad o velocidad de ataque.
/// </summary>
public class Gear : MonoBehaviour
{
    /// <summary>
    /// Tipo de ítem que representa este equipo (guante, zapato, etc.).
    /// </summary>
    public ItemData.ItemType type;

    /// <summary>
    /// Valor de mejora que se aplicará al jugador (proporcional).
    /// </summary>
    public float rate;

    /// <summary>
    /// Inicializa el objeto Gear con los datos del ítem y lo aplica al jugador.
    /// </summary>
    /// <param name="data">Datos del ítem con los valores de mejora.</param>
    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];

        ApplyGear();
    }

    /// <summary>
    /// Aumenta el nivel del equipo, actualizando el valor de mejora y aplicando el cambio.
    /// </summary>
    /// <param name="rate">Nuevo valor de mejora.</param>
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    /// <summary>
    /// Aplica la mejora correspondiente según el tipo del equipo (velocidad o velocidad de ataque).
    /// </summary>
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    /// <summary>
    /// Mejora la velocidad de ataque de las armas del jugador, dependiendo del tipo de arma.
    /// </summary>
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:
                    speed = 0.3f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    /// <summary>
    /// Mejora la velocidad de movimiento del jugador.
    /// </summary>
    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.Instance.player.speed = speed + speed * rate;
    }
}
