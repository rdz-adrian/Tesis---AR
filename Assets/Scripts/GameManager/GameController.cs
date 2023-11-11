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


    [Header("Camera")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float captureTimerConstant = 5f;
    [SerializeField] private GameObject messagesContainer;

    private GameObject currentCharacter;
    private bool pictureCaptured = false;
    private float captureTimer;
    private float startTime = 0;
    private bool isCapturing;
    private CameraCapture cameraCap;

    List<int> usedIndices = new List<int>();

    void Start()
    {
        cameraCap = GetComponent<CameraCapture>();
        isCapturing = false;
        timerText.text = captureTimerConstant.ToString();
        captureTimer = captureTimerConstant;

        onReachedOutScreenPosition();
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
            if (!currentChScript.moveToOutOfViewPosition)
            {
                StartCoroutine(DelayedAction(() =>
                {
                    currentChScript.moveToOutOfViewPosition = true;
                    currentChScript.moveToPicturePosition = false;
                    currentChScript.moveToInitialPosition = false;

                }, 1.5f));
            }
            else
            {
                currentChScript.moveToPicturePosition = false;
                currentChScript.moveToInitialPosition = false;
            }

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
            // Este float es para que vaya para la derecha o izquierda
            float randomFloat = UnityEngine.Random.Range(-1f, 1f);
            int randomIndex = GetRandomIndex();

            if (currentCharacter != null) currentCharacter.SetActive(false);
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

    int GetRandomIndex()
    {

        int randomIndex = -1;

        while (randomIndex < 0 || usedIndices.Contains(randomIndex))
        {
            randomIndex = UnityEngine.Random.Range(0, characters.Length);
        }

        usedIndices.Add(randomIndex);

        if (usedIndices.Count == characters.Length)
        {
            usedIndices.Clear();
        }

        return randomIndex;

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
