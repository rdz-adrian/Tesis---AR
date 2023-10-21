using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraCapture : MonoBehaviour
{
    [SerializeField] private Camera cameraToUse;

    public string CapturePhoto()
    {
        // Capture the camera image
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToUse.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraToUse.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        cameraToUse.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Generate a unique name for the image using date and time
        string fileName = "sreenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        // Convert the texture to an image
        byte[] bytes = screenshot.EncodeToPNG();

        string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        File.WriteAllBytes(Path.Combine(fullPath, fileName), bytes);

        return "Image captured and saved to: " + fullPath;
    }
}
