using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un enemigo en el juego.
/// Controla su movimiento, daño recibido, muerte y comportamiento animado.
/// </summary>
public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;

    /// <summary>Conjunto de animaciones por tipo de enemigo.</summary>
    public RuntimeAnimatorController[] animCon;

    /// <summary>Referencia al jugador para perseguirlo.</summary>
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    /// <summary>
    /// Inicializa componentes al activarse el enemigo.
    /// </summary>
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    /// <summary>
    /// Controla el movimiento del enemigo hacia el jugador.
    /// Solo se ejecuta si el enemigo está vivo y no recibiendo daño.
    /// </summary>
    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    /// <summary>
    /// Controla la orientación visual del sprite (flipX) según la posición del jugador.
    /// </summary>
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    /// <summary>
    /// Se ejecuta al activarse el GameObject. Reinicia el estado del enemigo.
    /// </summary>
    void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    /// <summary>
    /// Inicializa los valores del enemigo según datos de generación (SpawnData).
    /// </summary>
    /// <param name="data">Datos de spawn del enemigo.</param>
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    /// <summary>
    /// Detecta colisión con balas, aplica daño y efectos.
    /// Si muere, reproduce animación de muerte y desactiva el objeto.
    /// </summary>
    /// <param name="collision">Collider que ha tocado al enemigo.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
            if (GameManager.Instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

        }
    }

    /// <summary>
    /// Corrutina que aplica retroceso al enemigo tras recibir daño.
    /// </summary>
    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// Desactiva el objeto enemigo (simula muerte).
    /// </summary>
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
