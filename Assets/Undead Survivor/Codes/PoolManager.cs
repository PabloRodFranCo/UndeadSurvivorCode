using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Administra un conjunto de objetos (pools) reutilizables para mejorar el rendimiento
/// al evitar instanciaciones y destrucciones frecuentes de objetos en tiempo de ejecución.
/// </summary>
public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// Arreglo de prefabs que serán utilizados para crear los objetos del pool.
    /// Cada índice corresponde a un tipo diferente de objeto.
    /// </summary>
    public GameObject[] prefabs;

    /// <summary>
    /// Arreglo de listas, donde cada lista contiene objetos del mismo tipo (según el prefab correspondiente).
    /// </summary>
    List<GameObject>[] pools;

    /// <summary>
    /// Inicializa las listas para cada tipo de objeto durante el ciclo de vida de Awake.
    /// </summary>
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    /// <summary>
    /// Obtiene un objeto del pool correspondiente al índice especificado.
    /// Si no hay objetos disponibles (inactivos), se instancia uno nuevo a partir del prefab correspondiente.
    /// </summary>
    /// <param name="index">Índice del tipo de objeto (prefab) a obtener.</param>
    /// <returns>
    /// Un objeto activo del pool correspondiente. Puede ser un objeto previamente usado o uno nuevo.
    /// </returns>
    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
