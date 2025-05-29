/// <summary>
/// Controla el estado de pausa del juego, incluyendo la activación del menú de pausa y la reanudación.
/// </summary>
public class PauseManager : MonoBehaviour
{
    /// <summary>
    /// Referencia al menú de pausa de la interfaz.
    /// </summary>
    public GameObject pauseMenuUI;

    /// <summary>
    /// Indica si el juego está actualmente en pausa.
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// Escucha la tecla Escape para pausar o reanudar el juego.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    /// <summary>
    /// Reanuda el juego desactivando el menú y restaurando el tiempo.
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Pausa el juego activando el menú y deteniendo el tiempo.
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// Carga la escena del menú principal y reanuda el tiempo.
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameStart");
    }
}
