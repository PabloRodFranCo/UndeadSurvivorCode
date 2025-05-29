/// <summary>
/// Controlador principal del juego. Gestiona el estado de la partida, UI, jugador y flujo del juego.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Instancia única del GameManager (singleton).
    /// </summary>
    public static GameManager Instance;

    [Header("# Game Control")]
    /// <summary>Indica si la partida está activa.</summary>
    public bool isLive;

    /// <summary>Tiempo actual de juego transcurrido.</summary>
    public float gameTime;

    /// <summary>Tiempo máximo permitido de juego antes de forzar la victoria.</summary>
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    /// <summary>ID del personaje seleccionado por el jugador.</summary>
    public int playerId;

    /// <summary>Salud actual del jugador.</summary>
    public float health;

    /// <summary>Salud máxima del jugador.</summary>
    public float maxHealth = 100;

    /// <summary>Nivel actual del jugador.</summary>
    public int level;

    /// <summary>Número de enemigos derrotados.</summary>
    public int kill;

    /// <summary>Experiencia acumulada actual del jugador.</summary>
    public int exp;

    /// <summary>Lista de experiencia necesaria para subir cada nivel.</summary>
    public int[] nextExp = { 3, 5, 9, 10, 20, 50, 110, 230, 310, 440, 550 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;
    public GameObject uiPauseMenu;

    [Header("# UI")]
    public GameObject uiMapSelectMenu;
    public GameObject hudCanvas;

    [Header("# Map Options")]
    /// <summary>Lista de mapas disponibles para el jugador.</summary>
    public GameObject[] mapTilemaps;

    /// <summary>
    /// Inicializa la instancia y ajusta los FPS.
    /// </summary>
    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// Inicia el juego con el personaje seleccionado.
    /// </summary>
    /// <param name="id">ID del personaje.</param>
    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        uiMapSelectMenu.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    /// <summary>
    /// Comienza el juego en el mapa elegido y activa los elementos principales.
    /// </summary>
    /// <param name="mapIndex">Índice del mapa a activar.</param>
    public void StartGameWithMap(int mapIndex)
    {
        foreach (GameObject map in mapTilemaps)
            map.SetActive(false);

        if (mapIndex >= 0 && mapIndex < mapTilemaps.Length)
            mapTilemaps[mapIndex].SetActive(true);

        uiMapSelectMenu.SetActive(false);
        hudCanvas.SetActive(true);

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    /// <summary>
    /// Finaliza la partida con derrota.
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    /// <summary>
    /// Rutina de fin de partida (derrota).
    /// </summary>
    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    /// <summary>
    /// Finaliza la partida con victoria.
    /// </summary>
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    /// <summary>
    /// Rutina de fin de partida (victoria).
    /// </summary>
    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    /// <summary>
    /// Reinicia la escena actual (reiniciar juego).
    /// </summary>
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Sale de la aplicación.
    /// </summary>
    public void GameQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Actualización del juego por frame. Controla tiempo, escape y victoria automática.
    /// </summary>
    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiPauseMenu.activeSelf)
                Resume();
            else
                Pause();
        }

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    /// <summary>
    /// Otorga experiencia al jugador y gestiona subida de nivel.
    /// </summary>
    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    /// <summary>
    /// Pausa el tiempo del juego y desactiva la palanca de control.
    /// </summary>
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    /// <summary>
    /// Reanuda el tiempo del juego y reactiva la palanca de control.
    /// </summary>
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;

        if (uiPauseMenu != null)
            uiPauseMenu.SetActive(false);
    }

    /// <summary>
    /// Pausa el juego y muestra el menú de pausa.
    /// </summary>
    public void Pause()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;

        if (uiPauseMenu != null)
            uiPauseMenu.SetActive(true);
    }
}
