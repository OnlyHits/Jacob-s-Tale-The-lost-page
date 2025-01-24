using UnityEngine;
using System.IO;

public class CanvasScreenshot : MonoBehaviour
{
    public Camera canvasCamera; // Assign the camera rendering the Canvas
    public int width = 1920;    // Set desired resolution width
    public int height = 1080;   // Set desired resolution height
    public string screenshotName = "CanvasScreenshot.png";

    public void TakeScreenshot()
    {
        // Create a RenderTexture
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        canvasCamera.targetTexture = renderTexture;

        // Render the camera's view to the RenderTexture
        RenderTexture.active = renderTexture;
        canvasCamera.Render();

        // Read pixels into a Texture2D
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Save the Texture2D to a file
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, screenshotName), bytes);
        Debug.Log($"Screenshot saved to {Path.Combine(Application.persistentDataPath, screenshotName)}");

        // Clean up
        canvasCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
    }
}
