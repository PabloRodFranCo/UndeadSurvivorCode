/// <summary>
/// Representa una bala disparada por el jugador u otro objeto, con daño y capacidad de penetración.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Daño que inflige la bala al impactar con un enemigo.
    /// </summary>
    public float damage;

    /// <summary>
    /// Cantidad de enemigos que puede atravesar antes de desaparecer. Si es -100, la bala es inmortal.
    /// </summary>
    public int per;

    Rigidbody2D rigid;

    /// <summary>
    /// Se llama al iniciar. Obtiene la referencia al componente Rigidbody2D.
    /// </summary>
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Inicializa los valores de la bala y la lanza en una dirección determinada si tiene penetración.
    /// </summary>
    /// <param name="damage">Daño que infligirá la bala.</param>
    /// <param name="per">Cantidad de enemigos que puede atravesar.</param>
    /// <param name="dir">Dirección en la que se moverá la bala.</param>
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    /// <summary>
    /// Se llama cuando la bala colisiona con un collider 2D.
    /// Si colisiona con un enemigo, reduce la penetración.
    /// Si la penetración llega a 0, la bala se detiene y se desactiva.
    /// </summary>
    /// <param name="collision">Collider con el que colisiona.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Se llama cuando la bala sale del área de juego. Se desactiva si no es inmortal.
    /// </summary>
    /// <param name="collision">Collider del área de juego.</param>
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
