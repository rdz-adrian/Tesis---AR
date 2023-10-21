using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float captureTimerConstant = 5f;
    [SerializeField] private GameObject messagesContainer;

    private float captureTimer;
    private float startTime = 0;
    private bool isCapturing;
    private CameraCapture cameraCap;

    void Start()
    {
        cameraCap = GetComponent<CameraCapture>();
        isCapturing = false;
        timerText.text = captureTimerConstant.ToString();
        captureTimer = captureTimerConstant;
    }

    void Update()
    {
        if (isCapturing)
        {
            timerText.text = ((int)captureTimer + 1).ToString();
            float elapsedTime = Time.time - startTime;
            captureTimer = Mathf.Max(0f, captureTimerConstant - elapsedTime);

            if (captureTimer <= 0f)
            {
                string text = cameraCap.CapturePhoto();
                ShowMessage(text, 3f);
                isCapturing = false;
            }
        }
        else timerText.gameObject.SetActive(false);
    }

    public void TakePicture()
    {
        if (!isCapturing)
        {
            isCapturing = true;
            startTime = Time.time;
            timerText.gameObject.SetActive(true);
        }
    }

    public void ShowMessage(string text, float time)
    {
        messagesContainer.GetComponentInChildren<TextMeshProUGUI>().text = text;
        messagesContainer.SetActive(true);
        StartCoroutine(HideMessage(time));
    }

    IEnumerator HideMessage(float time)
    {
        yield return new WaitForSeconds(time);

        messagesContainer.SetActive(false);
    }
}
