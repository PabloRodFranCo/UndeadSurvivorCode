using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reubica automáticamente objetos cuando salen de un área determinada.
/// Utiliza lógica diferente según la etiqueta del objeto (por ejemplo, "Ground" o "Enemy").
/// </summary>
public class Reposition : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente Collider2D del objeto.
    /// </summary>
    Collider2D coll;

    /// <summary>
    /// Obtiene el componente Collider2D al iniciar el objeto.
    /// </summary>
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Se ejecuta cuando otro collider sale del área de colisión del trigger 2D.
    /// Si el objeto saliente tiene la etiqueta "Area", se reposiciona este objeto.
    /// </summary>
    /// <param name="collision">El collider que ha salido del área del trigger.</param>
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                // Calcula la dirección hacia el jugador y mueve el terreno en esa dirección
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else
                {
                    transform.Translate(dirX * 40, dirY * 40, 0);
                }
                break;

            case "Enemy":
                // Si el collider está habilitado, mueve al enemigo a una nueva posición aleatoria cercana al jugador
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
