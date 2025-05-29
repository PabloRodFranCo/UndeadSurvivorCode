/// <summary>
/// Controla el comportamiento del jugador, incluyendo movimiento, animación, daño recibido y entrada del usuario.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Vector de entrada del jugador (dirección del movimiento).
    /// </summary>
    public Vector2 inputVec;

    /// <summary>
    /// Velocidad base del jugador, modificada por la clase Character.
    /// </summary>
    public float speed;

    /// <summary>
    /// Componente encargado de detectar enemigos u objetos cercanos.
    /// </summary>
    public Scanner scanner;

    /// <summary>
    /// Arreglo de manos del jugador (izquierda y derecha) usadas para mostrar armas.
    /// </summary>
    public Hand[] hands;

    /// <summary>
    /// Controladores de animación asociados a cada personaje jugable.
    /// </summary>
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    /// <summary>
    /// Inicializa componentes del jugador (rigidbody, render, animaciones y manos).
    /// </summary>
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    /// <summary>
    /// Se ejecuta al activar el objeto. Aplica la velocidad y el controlador de animación según el personaje elegido.
    /// </summary>
    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.Instance.playerId];
    }

    /// <summary>
    /// Mueve al jugador físicamente según la entrada y el tiempo fijo de Unity.
    /// </summary>
    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    /// <summary>
    /// Se ejecuta cuando se detecta movimiento con el sistema de Input de Unity.
    /// </summary>
    /// <param name="value">Vector 2D con la dirección de movimiento.</param>
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    /// <summary>
    /// Actualiza animaciones y orientación del sprite después del movimiento.
    /// </summary>
    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    /// <summary>
    /// Se ejecuta mientras el jugador colisiona con otro objeto. Aplica daño con el tiempo y termina el juego si la salud llega a cero.
    /// </summary>
    /// <param name="collision">Información de la colisión.</param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if (GameManager.Instance.health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}
