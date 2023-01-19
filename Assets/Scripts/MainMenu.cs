using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    // Exit button
    public void Exit() {
        Application.Quit();
    }

    // Volume Button
    public Sprite volume_up_sprite;
    public Sprite volume_off_sprite;
    public Button volume_button;
    private bool isVolumeOn = true;

    public void Volume() {
        if (isVolumeOn) {
            volume_button.image.sprite = volume_off_sprite;
            isVolumeOn = false;
        } else {
            volume_button.image.sprite = volume_up_sprite;
            isVolumeOn = true;
        }
    }
}
