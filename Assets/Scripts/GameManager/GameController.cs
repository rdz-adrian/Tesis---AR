using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.IO;

public class GameController : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] Transform initialPositionLeft;
    [SerializeField] Transform initialPositionRight;
    //De aquí salen las posiciones para salir de la pantalla
    [SerializeField] Transform picturePositionLeft;
    [SerializeField] Transform picturePositionRight;

    public bool moveToPicturePosition;

    [Header("Characters")]
    [SerializeField] GameObject[] characters;
    [SerializeField] private GameObject currentCharacter;

    [Header("Camera")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float captureTimerConstant = 5f;
    [SerializeField] private GameObject messagesContainer;

    private bool pictureCaptured = false;
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
        checkCharacterMovementParams();

        if (isCapturing)
        {
            timerText.text = ((int)captureTimer + 1).ToString();
            float elapsedTime = Time.time - startTime;
            captureTimer = Mathf.Max(0f, captureTimerConstant - elapsedTime);

            if (captureTimer <= 0f)
            {
                string text = cameraCap.CapturePhoto();
                pictureCaptured = true;
                ShowMessage(text, 3f);
                isCapturing = false;
            }
        }
        else timerText.gameObject.SetActive(false);
    }

    void checkCharacterMovementParams()
    {
        Character currentChScript = currentCharacter.GetComponent<Character>();

        if (pictureCaptured)
        {
            StartCoroutine(DelayedAction(() =>
            {
                currentChScript.moveToOutOfViewPosition = true;
            }, 1.5f));
        }

        //---------------TODO: Quitar lo de presionar la tecla 
        //if (moveToPicturePosition || Input.GetKeyDown(KeyCode.P))
        //{
        //    currentChScript.moveToPicturePosition = true;
        //}

        if (moveToPicturePosition && !pictureCaptured)
        {
            currentChScript.moveToPicturePosition = true;
            currentChScript.moveToInitialPosition = false;
        }

        if (!moveToPicturePosition && !pictureCaptured)
        {
            currentChScript.moveToInitialPosition = true;
            currentChScript.moveToPicturePosition = false;
        }


    }

    public void onReachedOutScreenPosition()
    {
        if (characters.Length == 1) currentCharacter.SetActive(false);

        if (characters.Length > 0)
        {
            float randomFloat = UnityEngine.Random.Range(-1f, 1f);
            int randomIndex = -1;
            while (randomIndex < 0)
            {
                randomIndex = UnityEngine.Random.Range(0, characters.Length);
                if (characters[randomIndex].activeSelf) randomIndex = -1;
            }

            //--------------TODO------------------
            /*
             setear las posiciones para ver si va para la posicion inicial o no
            y los otros parametros para que no ande mareado, tmb cambiar los nombres de 
            las animaciones y para los puntos que va a caminar
             */

            currentCharacter.SetActive(false);
            pictureCaptured = false;

            currentCharacter = characters[randomIndex];
            currentCharacter.SetActive(true);
            Character currentChScript = currentCharacter.GetComponent<Character>();

            if (randomFloat < 0f)
            {
                currentChScript.picturePosition = picturePositionLeft;
                currentCharacter.transform.position = picturePositionLeft.GetChild(0).position;
                currentChScript.initialPosition = initialPositionLeft;
            }
            else
            {
                currentChScript.picturePosition = picturePositionRight;
                currentCharacter.transform.position = picturePositionRight.GetChild(0).position;
                currentChScript.initialPosition = initialPositionRight;
            }


            if (moveToPicturePosition) currentChScript.moveToPicturePosition = true;
            else currentChScript.moveToInitialPosition = true;

        }

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

    IEnumerator DelayedAction(Action callback, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        callback.Invoke(); // Ejecutar la acción callback después del retraso
    }

}
