using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador de audio global del juego.
/// Maneja música de fondo (BGM) y efectos de sonido (SFX).
/// Usa múltiples canales de audio para reproducir efectos simultáneamente.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>Instancia global del AudioManager (singleton).</summary>
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    /// <summary>
    /// Enum que define los efectos de sonido disponibles por nombre e índice.
    /// </summary>
    public enum Sfx
    {
        Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win
    }

    /// <summary>
    /// Inicializa la instancia y crea los objetos de audio necesarios.
    /// </summary>
    private void Awake()
    {
        instance = this;
        Init();
    }

    /// <summary>
    /// Inicializa los reproductores de BGM y SFX.
    /// </summary>
    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    /// <summary>
    /// Reproduce o detiene la música de fondo según el parámetro.
    /// </summary>
    /// <param name="isPlay">True para reproducir, False para detener.</param>
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    /// <summary>
    /// Activa o desactiva el filtro de efectos de BGM (AudioHighPass).
    /// </summary>
    /// <param name="isPlay">True para activar efecto, False para desactivar.</param>
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    /// <summary>
    /// Reproduce un efecto de sonido específico, eligiendo canal disponible.
    /// Algunos efectos se seleccionan aleatoriamente dentro de un rango.
    /// </summary>
    /// <param name="sfx">Tipo de efecto a reproducir.</param>
    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[i].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

}
