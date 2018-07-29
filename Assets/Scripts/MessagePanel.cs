using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    RectTransform place;
    public Text messageToShow;

    void Awake()
    {
        place = GetComponent<RectTransform>();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(string message, float time)
    {
        gameObject.SetActive(true);
        Show(message, time, place.anchoredPosition);
    }

    public void Show(string message, float time, Vector2 position)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        place.anchoredPosition = position;
        messageToShow.text = message;
        StartCoroutine(Dissapear(time));
    }

    IEnumerator Dissapear(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}