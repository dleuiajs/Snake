using UnityEngine;
using Discord;
using System;
using System.Collections;
public class DiscordManager : MonoBehaviour
{
    [Header("AppSettings")]
    long applicationID = 1412706836632698889;

    string largeImage = "icon_snake";

    long time;

    [Header("Discord")]
    public static DiscordManager Instance;
    Discord.Discord discord;
    float timeoutReconnect = 10f;
    bool connected = false;

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
        Connect();
        time = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    void Update()
    {
        if (connected)
        {
            try
            {
                discord.RunCallbacks();
            }
            catch
            {
                Debug.LogWarning("RunCallbacks error");
                Connect();
            }
        }
    }

    void Connect()
    {
        try
        {
            discord = new Discord.Discord(applicationID, (ulong)CreateFlags.NoRequireDiscord);
            connected = true;
            Debug.Log("Discord successfully connected");
        }
        catch
        {
            connected = false;
            Debug.LogWarning($"Discord is not running. Attempting to connect in {timeoutReconnect} seconds.");
            StartCoroutine(TryConnect(timeoutReconnect));
        }
    }

    IEnumerator TryConnect(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        Connect();
    }

    public void UpdateActivity()
    {
        if (connected)
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
                Debug.LogWarning("Discord update activity error");
            }
        }
    }

    void OnApplicationQuit()
    {
        if (connected)
            discord.Dispose(); // корректно освобождаем
    }
}
