using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeout : MonoBehaviour
{
    private TMP_Text tmp;
    public static bool fadeOut = true;
    // Start is called before the first frame update
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        StartCoroutine(FadeOut());
    }
    private void OnEnable()
    {
        if (fadeOut)
            StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        tmp.CrossFadeAlpha(0.0f, 3.0f, false);
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }
}
