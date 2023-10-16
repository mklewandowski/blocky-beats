using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterUI : MonoBehaviour
{
    AudioManager audioManager;

    TextMeshProUGUI HUDText;

    float typeTimer = 0;
    float typeTimerMax = .02f;
    string textHeader = "";
    string textToType = "";
    int textLength = 0;
    float clickTimer = 0;
    float clickTimerMax = .075f;

    void Start()
    {
        GameObject am = GameObject.Find("GameManager");
        audioManager = am.GetComponent<AudioManager>();
        HUDText = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (typeTimer > 0)
        {
            typeTimer -= Time.deltaTime;
            if (typeTimer <= 0)
            {
                textLength++;
                HUDText.text = textHeader + textToType.Substring(0, textLength);
                if (textLength < textToType.Length)
                {
                    typeTimer = typeTimerMax;
                }
            }
        }
        if (clickTimer > 0 && typeTimer > 0)
        {
            clickTimer -= Time.deltaTime;
            if (clickTimer <= 0)
            {
                audioManager.PlayClickSound();
                clickTimer = clickTimerMax;
            }
        }
    }

    public void StartEffect(string staticText, string text)
    {
        textHeader = staticText;
        textToType = text;
        textLength = 0;
        typeTimer = typeTimerMax;
        clickTimer = clickTimerMax;
    }
}
