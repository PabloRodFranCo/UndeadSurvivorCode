using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la aparición de enemigos en puntos específicos del escenario.
/// La frecuencia y características de aparición dependen del tiempo de juego y los datos de nivel definidos.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Puntos donde los enemigos pueden aparecer. Se obtienen de los hijos del objeto Spawner.
    /// </summary>
    public Transform[] spawnPoint;

    /// <summary>
    /// Datos de aparición por nivel, incluyendo frecuencia y atributos del enemigo.
    /// </summary>
    public SpawnData[] spawnData;

    /// <summary>
    /// Tiempo máximo dividido por el número de niveles para calcular duración de cada fase.
    /// </summary>
    public float levelTime;

    /// <summary>
    /// Nivel actual basado en el tiempo transcurrido del juego.
    /// </summary>
    int level;

    /// <summary>
    /// Temporizador interno para controlar cuándo generar un nuevo enemigo.
    /// </summary>
    float timer;

    /// <summary>
    /// Inicializa los puntos de aparición y calcula el tiempo por nivel basado en la duración total del juego.
    /// </summary>
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.Instance.maxGameTime / spawnData.Length;
    }

    /// <summary>
    /// Controla la lógica de aparición durante el juego. Determina el nivel actual y genera enemigos periódicamente.
    /// </summary>
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        float gameTime = GameManager.Instance.gameTime;

        // Determina el nivel actual basado en el tiempo de juego
        if (gameTime < 120f)
            level = 0;
        else if (gameTime < 240f)
            level = 1;
        else
            level = 2;

        // Genera enemigo si ha pasado el tiempo adecuado
        if (timer > spawnData[level].spawnTime / 2)
        {
            timer = 0;
            Spawn();
        }
    }

    /// <summary>
    /// Instancia un enemigo desde el pool en una posición aleatoria y lo inicializa con los datos del nivel actual.
    /// </summary>
    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

/// <summary>
/// Representa los datos de configuración para los enemigos de cada nivel.
/// Incluye frecuencia de aparición, tipo de sprite, salud y velocidad.
/// </summary>
[System.Serializable]
public class SpawnData
{
    /// <summary>
    /// Tiempo entre apariciones de enemigos en este nivel.
    /// </summary>
    public float spawnTime;

    /// <summary>
    /// Tipo de sprite que se asignará al enemigo.
    /// </summary>
    public int spriteType;

    /// <summary>
    /// Salud inicial del enemigo.
    /// </summary>
    public int health;

    /// <summary>
    /// Velocidad de movimiento del enemigo.
    /// </summary>
    public float speed;
}
