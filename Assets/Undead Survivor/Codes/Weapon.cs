using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un arma que puede equiparse al jugador.
/// Administra su comportamiento, tipo, daño, cantidad de proyectiles y velocidad.
/// </summary>
public class Weapon : MonoBehaviour
{
    /// <summary>
    /// ID del tipo de arma.
    /// </summary>
    public int id;

    /// <summary>
    /// ID del prefab asociado al proyectil.
    /// </summary>
    public int prefabId;

    /// <summary>
    /// Daño base del arma.
    /// </summary>
    public float damage;

    /// <summary>
    /// Cantidad de proyectiles que dispara o rota el arma.
    /// </summary>
    public int count;

    /// <summary>
    /// Velocidad de ataque o rotación del arma.
    /// </summary>
    public float speed;

    /// <summary>
    /// Temporizador para controlar la velocidad de disparo.
    /// </summary>
    public float timer;

    /// <summary>
    /// Referencia al jugador que posee el arma.
    /// </summary>
    Player player;

    /// <summary>
    /// Se ejecuta al inicializar el arma. Obtiene la referencia al jugador desde el GameManager.
    /// </summary>
    void Awake()
    {
        player = GameManager.Instance.player;
    }

    /// <summary>
    /// Controla la lógica de actualización del arma en cada frame.
    /// Incluye rotación o disparo automático y pruebas de subida de nivel.
    /// </summary>
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }

        // Prueba manual de subida de nivel con la tecla "Jump"
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    /// <summary>
    /// Aumenta las estadísticas del arma y actualiza los proyectiles si es necesario.
    /// </summary>
    /// <param name="damage">Nuevo valor de daño base.</param>
    /// <param name="count">Cantidad adicional de proyectiles.</param>
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Inicializa el arma con los datos de un objeto de tipo ItemData.
    /// </summary>
    /// <param name="data">Datos del arma (ItemData).</param>
    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.Instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            case 1:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Genera y posiciona proyectiles alrededor del jugador para armas tipo rotatoria.
    /// </summary>
    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector2.zero);
        }
    }

    /// <summary>
    /// Dispara un proyectil hacia el objetivo más cercano, si existe.
    /// </summary>
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
