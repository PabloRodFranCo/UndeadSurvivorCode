/// <summary>
/// Representa un objeto que puede seleccionarse al subir de nivel.
/// Gestiona su visualización, descripción y efectos (arma, equipamiento o curación).
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// Datos del ítem (tipo, daño, nombre, descripción, ícono, etc.).
    /// </summary>
    public ItemData data;

    /// <summary>
    /// Nivel actual del ítem.
    /// </summary>
    public int level;

    /// <summary>
    /// Referencia al arma asociada (si aplica).
    /// </summary>
    public Weapon weapon;

    /// <summary>
    /// Referencia al equipo asociado (si aplica).
    /// </summary>
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    /// <summary>
    /// Inicializa los elementos visuales y asigna los datos del ítem.
    /// </summary>
    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    /// <summary>
    /// Actualiza la información visual del ítem cuando se muestra en pantalla.
    /// </summary>
    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;

            default:
                textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                break;
        }
    }

    /// <summary>
    /// Se ejecuta cuando el jugador selecciona el ítem. Aplica su efecto y aumenta el nivel.
    /// </summary>
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage + data.baseDamage * data.damages[level];
                    int nextCount = data.counts[level];
                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.Instance.health = GameManager.Instance.maxHealth;
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
