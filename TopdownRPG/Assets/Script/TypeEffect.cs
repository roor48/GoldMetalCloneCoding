using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public int CharPerSeconds;
    public GameObject endCursor;
    public bool isAnim;

    Text msgText;
    AudioSource audioSource;

    int index;
    string targetMsg;

    private void Start()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg)
    {
        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
            return;
        }
        targetMsg = msg;
        EffectStart();
    }

    void EffectStart()
    {
        msgText.text = null;
        index = 0;
        endCursor.SetActive(false);
        isAnim = true;
        
        Effecting();
    }
    void Effecting()
    {
        // End Animation
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index++;
        
        // Sound
        if (targetMsg[index-1] != ' ' && targetMsg[index-1] != '.')
        {
            audioSource.Play();
        }
        else
        {
            Effecting();
            return;
        }

        //Recursive
        Invoke(nameof(Effecting), 1.0f / CharPerSeconds);
    }
    void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }
}
