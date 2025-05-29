using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona los logros del juego, como desbloquear personajes y mostrar notificaciones.
/// Se basa en condiciones del GameManager y usa PlayerPrefs para guardar progreso.
/// </summary>
public class AchiveManager : MonoBehaviour
{
/// <summary>Referencias a los personajes bloqueados en la UI.</summary>
    public GameObject[] lockCharacter;    

/// <summary>Referencias a los personajes desbloqueados en la UI.</summary>
    public GameObject[] unlockCharacter;    

/// <summary>Interfaz que muestra la notificación cuando se desbloquea un logro.</summary>
    public GameObject uiNotice;

 // Enum privado que representa los logros del juego.
    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait; 

    /// <summary>
    /// Inicializa logros al iniciar el juego. Crea claves en PlayerPrefs si no existen.
    /// </summary>
    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData")) 
        {
            Init(); 
        }
    }

    /// <summary>
    /// Inicializa PlayerPrefs con los valores base de los logros.
    /// </summary>
    void Init()
    {

        PlayerPrefs.SetInt("MyData", 1); 

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    /// <summary>
    /// Al comenzar la escena, desbloquea personajes según los logros alcanzados.
    /// </summary>
    void Start()
    {
        UnlockCharacter();
    }

    /// <summary>
    /// Activa/desactiva personajes según los logros almacenados.
    /// </summary>
    void UnlockCharacter()
    {
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1; 

            lockCharacter[i].SetActive(!isUnlock); 
            unlockCharacter[i].SetActive(isUnlock); 
        }
    }
    
    /// <summary>
    /// Evalúa en cada frame si se cumplen las condiciones de algún logro.
    /// </summary>
    void LateUpdate()
    {
        foreach (Achive achive in achives)
        { 
            CheckAchive(achive);
        }
    }


    /// <summary>
    /// Verifica si se ha cumplido un logro, lo guarda y muestra aviso visual.
    /// </summary>
    /// <param name="achive">Logro a comprobar</param>
    void CheckAchive(Achive achive) 
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.Instance.kill >= 10;
                break;

            case Achive.UnlockBean: 
                isAchive = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;
                break;
        }
        
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int i=0;i<uiNotice.transform.childCount;i++)
            {
                bool isActive = i == (int)achive; 
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive); 
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    /// <summary>
    /// Corrutina que muestra una notificación visual durante 5 segundos.
    /// </summary>
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait; 

        uiNotice.SetActive(false);
    }
}
