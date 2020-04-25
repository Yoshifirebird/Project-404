/*
 * MainMenuUIController.cs
 * Created by: Ambrosia
 * Created on: 25/4/2020 (dd/mm/yy)
 * Created for: needing a UI for the main menu
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: Extract fading into it's own class
//TODO: Make a globals class with static references of scripts and objects commonly used in scene, e.g. Player, Camera, FadeManager(?)
public class MainMenuUIController : MonoBehaviour
{
    [Header("Fading to black")]
    [SerializeField] Image _BlackImage;
    [SerializeField] float _FadeTimer = 0;
    float _FadeTime = 0;
    bool _Fading = false;
    bool _FadeDone = false;
    Color _CurrentColor = Color.clear;

    void Awake()
    {
        _BlackImage.enabled = false;
    }

    void Update()
    {
        _BlackImage.color = _CurrentColor;

        if (_Fading)
        {
            _CurrentColor.a = _FadeTime / _FadeTimer;

            if (_FadeTime > _FadeTimer)
            {
                _FadeDone = true;
            }

            _FadeTime += Time.deltaTime;
        }

        if (_FadeDone)
        {
            SceneManager.LoadScene("scn_test_level_2");
        }
    }

    public void PressPlay()
    {
        _Fading = true;
        _BlackImage.enabled = true;
    }

    public void PressOptions()
    {

    }

    public void PressExit()
    {
        Application.Quit();
        Debug.Break();
    }
}
