/*
A script to allow the logging of BCI selections in the Autumn scene 
The Log function included here will be called from the game object's own selection logger 
*/

using System;
using System.IO;
using UnityEngine;

public class ERPGameLoggerSummer : MonoBehaviour
{
    public static ERPGameLoggerSummer Instance { get; private set; }

    private string filePath;
    private string folderPath = @"C:\Users\student\Documents\Henriette\ERPLogsSummer";

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure the folder exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Create timestamped file
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
        string fileName = $"ERP_Selection_Log_{timestamp}.csv";
        filePath = Path.Combine(folderPath, fileName);

        // Write CSV header
        File.WriteAllText(filePath, "Timestamp,ObjectName\n");
        Debug.Log($"[ERP Logger] Logging to: {filePath}");
    }

    // Function for logging time and selected game object 
    public void Log(string sourceName)
    {
        string entry = $"{Time.time:F2},{sourceName}\n";
        File.AppendAllText(filePath, entry);
    }
}
