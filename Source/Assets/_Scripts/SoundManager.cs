using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip themeMusic;
    public AudioClip popSound;
    public AudioClip intenseMusic;
    public AudioClip timeTicking;
    public AudioClip levelWinSound;
    public AudioClip gameOverSound;
    public AudioClip wholeGameWinSound;
    public AudioClip backgroundMusic;

    [Header("Audio Components")]
    public AudioSource themePlayer;
    public AudioSource backgroundMusicPlayer;
    public AudioSource popSoundPlayer;
    public AudioSource gameOverSoundPlayer;
    public AudioSource levelWinSoundPlayer;
    public AudioSource timeTickingSoundPlayer;
    public AudioSource wholeGameWinSoundPlayer;
    public AudioSource intenseMusicSoundPlayer;

    private bool intenseStarted = false;
    private bool tickingStarted = false;

    /*void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            PlayPopSound();
    }*/

    public bool checkForIntense (float curTime) {
        if (curTime == 0.0f) return false;
        if (tickingStarted || intenseStarted) return false;
        return (curTime <= intenseMusic.length + timeTicking.length);
    }

    public bool checkForTimeTicking (float curTime) {
        if (curTime == 0.0f) return false;
        if (tickingStarted) return false;
        return (curTime <= timeTicking.length);
    }

    public void PlayTheme () {
        if (themePlayer.isPlaying) return;
        themePlayer.Play();
    }

    public void PlayBackground () {
        if (backgroundMusicPlayer.isPlaying) return;
        backgroundMusicPlayer.Play();
    }
    public void StopBackground () {
        backgroundMusicPlayer.Stop();
    }

    public void StopTheme () {
        //return;
        themePlayer.Stop();
    }

    public void PlayPopSound () {
        popSoundPlayer.Play();
    }

    public void PlayClockTicking () {
        StopTheme();
        tickingStarted = true;
        timeTickingSoundPlayer.Play();
        StartCoroutine(StopTicking());
    }

    public void PlayIntense () {
        StopTheme();
        intenseStarted = true;
        intenseMusicSoundPlayer.Play();
        StartCoroutine(StopIntense());
    }

    IEnumerator StopTicking () {
        yield return new WaitForSeconds(timeTicking.length);
        tickingStarted = false;
    }
    IEnumerator StopIntense () {
        yield return new WaitForSeconds(intenseMusic.length);
        intenseStarted = false;
    }

    public void StopTickingSound () {
        tickingStarted = false;
        timeTickingSoundPlayer.Stop();
        //PlayTheme();
    }
    public void StopIntenseSound () {
        intenseStarted = false;
        intenseMusicSoundPlayer.Stop();
        //PlayTheme();
    }

   // public IEnumerator LoadThemeAfter (AudioClip clip) {
   //     yield return new WaitForSeconds(clip.length);
   //     PlayTheme();
   // }

    public void PlayWholeGameWin () {
        wholeGameWinSoundPlayer.Play();
    }

    public void PlayGameOver () {
        gameOverSoundPlayer.Play();
    }

    public void PlayLevelWin () {
        levelWinSoundPlayer.Play();
    }
}