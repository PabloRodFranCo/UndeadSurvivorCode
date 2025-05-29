/// <summary>
/// Muestra información del jugador en la interfaz HUD (vida, experiencia, nivel, tiempo restante, enemigos derrotados).
/// </summary>
public class HUD : MonoBehaviour
{
    /// <summary>
    /// Tipos de información que se pueden mostrar en el HUD.
    /// </summary>
    public enum InfoType { Exp, Level, Kill, Time, Health }

    /// <summary>
    /// Tipo de información que este componente representará.
    /// </summary>
    public InfoType type;

    Text myText;
    Slider mySlider;

    /// <summary>
    /// Obtiene los componentes de texto o slider según el tipo de HUD.
    /// </summary>
    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    /// <summary>
    /// Actualiza el valor mostrado en el HUD según el tipo seleccionado.
    /// </summary>
    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[
                    Mathf.Min(GameManager.Instance.level, GameManager.Instance.nextExp.Length - 1)
                ];
                mySlider.value = curExp / maxExp;
                break;

            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.level);
                break;

            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.Instance.kill);
                break;

            case InfoType.Time:
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Health:
                float curHealth = GameManager.Instance.health;
                float maxHealth = GameManager.Instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
