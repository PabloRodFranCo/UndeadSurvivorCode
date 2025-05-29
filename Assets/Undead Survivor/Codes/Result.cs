using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la visualización de los títulos de resultado del juego, como "Win" o "Lose".
/// </summary>
public class Result : MonoBehaviour
{
    /// <summary>
    /// Arreglo de GameObjects que representan los títulos de resultado.
    /// titles[0] se espera que sea el mensaje de "Derrota".
    /// titles[1] se espera que sea el mensaje de "Victoria".
    /// </summary>
    public GameObject[] titles;

    /// <summary>
    /// Muestra el título de "Derrota" activando el GameObject correspondiente.
    /// </summary>
    public void Lose()
    {
        titles[0].SetActive(true);
    }

    /// <summary>
    /// Muestra el título de "Victoria" activando el GameObject correspondiente.
    /// </summary>
    public void Win()
    {
        titles[1].SetActive(true);
    }
}
