using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Escanea un área circular alrededor del objeto para detectar objetivos dentro de un rango específico.
/// Determina cuál es el objetivo más cercano dentro de los resultados.
/// </summary>
public class Scanner : MonoBehaviour
{
    /// <summary>
    /// Radio de escaneo utilizado para detectar objetivos.
    /// </summary>
    public float scanRange;

    /// <summary>
    /// Capa que define qué objetos serán considerados como objetivos durante el escaneo.
    /// </summary>
    public LayerMask targetLayer;

    /// <summary>
    /// Arreglo de objetivos detectados dentro del rango durante el escaneo.
    /// </summary>
    public RaycastHit2D[] targets;

    /// <summary>
    /// Referencia al objetivo más cercano detectado en el escaneo actual.
    /// </summary>
    public Transform nearestTarget;

    /// <summary>
    /// Realiza el escaneo en cada ciclo de FixedUpdate.
    /// Busca todos los objetivos dentro del rango y determina cuál está más cerca.
    /// </summary>
    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    /// <summary>
    /// Recorre todos los objetivos detectados y devuelve el que esté más cerca de este objeto.
    /// </summary>
    /// <returns>Transform del objetivo más cercano. Devuelve null si no se detecta ninguno.</returns>
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; // Valor arbitrario alto para inicializar la comparación

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
