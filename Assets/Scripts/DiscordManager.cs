using UnityEngine;
using Discord;
using System;
public class DiscordManager : MonoBehaviour
{
    [Header("Settings")]
    long applicationID = 1412706836632698889;

    string largeImage = "icon_snake";

    long time;

    [Header("Discord")]
    public static DiscordManager Instance;
    Discord.Discord discord;

    [Header("Scripts")]
    public GameManager gameManager;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        discord = new Discord.Discord(applicationID, (ulong)CreateFlags.NoRequireDiscord);
        time = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Debug.Log("Discord is not running");
        }
    }

    public void UpdateActivity()
    {
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Activity
            {
                Details = $"Score: {gameManager.score}",
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = $"{Application.productName} v{Application.version}"
                },
                Timestamps =
                {
                    Start = time
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                Debug.Log($"Discord UpdateActivity result: {res}");
            });
        }
        catch
        {
            Debug.LogError("Discord update activity error");
        }
    }

    void OnApplicationQuit()
    {
        discord.Dispose(); // корректно освобождаем
    }
}
