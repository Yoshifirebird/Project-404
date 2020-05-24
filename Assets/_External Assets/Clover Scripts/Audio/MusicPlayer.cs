/*
 * MusicPlayer.cs
 * Created by: Newgame+ LD
 * Created on: 28/3/2020 (dd/mm/yy)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class MusicPlayer : MonoBehaviour {

  public static MusicPlayer instance;
  public AudioClip musicClip;
  public float tempo;
  public Vector2 signature = new Vector2 (4, 4);
  public float curMeasure;
  public bool looping;
  public Vector3 loopStart;
  public Vector3 loopEnd;

  public float debug;
  public bool debugSet;
  public float debugSetter;

  public int debugMusicChoice;
  public bool debugMusicSet;

  public int priority;
  public bool playOnStart;

  void Start () {

    //if(MusicPlayer.instance != this)target = GameObject.FindWithTag ("Music Player").GetComponent<MusicPlayer> ();

    if (instance != null && instance != this) {

      if (priority < MusicPlayer.instance.priority)
        Destroy (gameObject);
      else {

        Destroy (MusicPlayer.instance.gameObject);

      }

    }

    MusicPlayer.instance = this;
    print ("instance set!");
    if (playOnStart) refresh ();

  }

  void Update () {

    convertSecondsToBeatValue ();
    convertBeatValueToSeconds (curMeasure);
    loopCheck ();
    if (debugSet) {
      GetComponent<AudioSource> ().time = debugSetter;
      debugSet = false;
    }

    if (debugMusicSet) {

      StartCoroutine (loadMusic ());
      debugMusicSet = false;
    }
  }

  void refresh () {

    GetComponent<AudioSource> ().time = 0;
    GetComponent<AudioSource> ().clip = musicClip;
    GetComponent<AudioSource> ().Stop ();
    GetComponent<AudioSource> ().PlayDelayed (0.25f);
  }

  void convertSecondsToBeatValue () {

    debug = GetComponent<AudioSource> ().time;
    curMeasure = ((debug / signature.x) * tempo) / 60;
  }

  public float convertBeatValueToSeconds (float value) {

    return (signature.x * ((value * 60) / tempo));
  }

  void loopCheck () {

    if (curMeasure > (loopEnd.x + (loopEnd.y / signature.y))) {
      GetComponent<AudioSource> ().time -= convertBeatValueToSeconds ((loopEnd.x + (loopEnd.y / signature.y)) - (loopStart.x + (loopStart.y / signature.y)));
    }

  }

  public IEnumerator loadMusic () {

    AudioSource source = GetComponent<AudioSource> ();

    while (source.volume > 0) {

      if (!source.isPlaying)
        source.volume = 0;
      source.volume -= Time.deltaTime / 2;

      yield return new WaitForEndOfFrame ();
    }

    refresh ();

    source.volume = 1;

  }

  public void stop (float fadeTime) {

    StartCoroutine (istop (fadeTime));

  }

  public IEnumerator istop (float fadeTime) {

    AudioSource source = GetComponent<AudioSource> ();

    while (source.volume > 0) {

      if (!source.isPlaying)
        source.volume = 0;
      source.volume -= (Time.deltaTime / fadeTime);

      yield return new WaitForEndOfFrame ();
    }

  }

}
