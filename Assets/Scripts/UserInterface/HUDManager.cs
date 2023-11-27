using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

//Handles all HUD UI like health, objectives etc.
public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUD_Subtitles HUD_Subtitles;
    [SerializeField] private HUD_Notifications HUD_Notifications;
    [SerializeField] private HUD_Weapon HUDWeapon;
    [SerializeField] private HUDStats HUDStats;
    [SerializeField] private float instructionTimer = 4F;

    public static HUDManager instance;

    public HUD_Weapon HUD_Weapon { get { return HUDWeapon; } }

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        HUDStats.Start();
    }

    private void Update()
    {
        HUDStats.Update();
    }

    public void MakeVisible(CanvasGroup canvasGroup, bool value)
    {
        StartCoroutine(IMakeVisible(canvasGroup, value));
    }

    public IEnumerator IMakeVisible(CanvasGroup canvasGroup, bool value)
    {
        if (value)
        {
            while (canvasGroup.alpha != 1)
            {
                canvasGroup.alpha += Time.deltaTime * 1.5F;

                if (canvasGroup.alpha >= 0.95F)
                {
                    canvasGroup.alpha = Mathf.Ceil(canvasGroup.alpha);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        if (!value)
        {
            while (canvasGroup.alpha != 0)
            {
                canvasGroup.alpha -= Time.deltaTime * 1.5F;

                if (canvasGroup.alpha <= 0.01F)
                {
                    canvasGroup.alpha = Mathf.Ceil(canvasGroup.alpha);
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void ShowSubtitles(string text, float durationInSeconds)
    {
        PlayNotificationSound();
        StartCoroutine(IShowSubtitlesForADuration(text, durationInSeconds));
    }

    public void ShowNotification(string text)
    {
        PlayNotificationSound();
        StartCoroutine(IShowNotification(text));
    }

    private IEnumerator IShowSubtitlesForADuration(string text, float duration)
    {
        HUD_Subtitles.subtitle.text = string.Empty;

        while (HUD_Subtitles.cGroup.alpha != 1)
        {

            HUD_Subtitles.cGroup.alpha += Time.deltaTime * 1.5F;

            if (HUD_Notifications.cGroup.alpha >= 0.95F)
                HUD_Notifications.cGroup.alpha = Mathf.Ceil(HUD_Notifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }

        HUD_Subtitles.subtitle.text = text;

        yield return new WaitForSeconds(duration);

        while (HUD_Subtitles.cGroup.alpha != 0)
        {
            HUD_Subtitles.cGroup.alpha -= Time.deltaTime * 1.5F;

            if (HUD_Notifications.cGroup.alpha <= 0.01F)
                HUD_Notifications.cGroup.alpha = Mathf.Floor(HUD_Notifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator IShowNotification(string text)
    {
        while (HUD_Notifications.cGroup.alpha != 1)
        {
            HUD_Notifications.cGroup.alpha += Time.deltaTime * 1.5F;

            if (HUD_Notifications.cGroup.alpha >= 0.95F)
                HUD_Notifications.cGroup.alpha = Mathf.Ceil(HUD_Notifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }

        HUD_Notifications.notification.text = string.Empty;

        HUD_Notifications.notification.text = text;

        yield return new WaitForSeconds(instructionTimer);

        while (HUD_Notifications.cGroup.alpha != 0)
        {
            HUD_Notifications.cGroup.alpha -= Mathf.Floor(Time.deltaTime * 1.5F);

            if (HUD_Notifications.cGroup.alpha <= 0.01F)
                HUD_Notifications.cGroup.alpha = Mathf.Floor(HUD_Notifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }
    }

    private void PlayNotificationSound()
    {
        print("ui notification sound is meant to be played");
    }
}

[System.Serializable]
public struct HUD_Notifications
{
    public TMP_Text notification;
    public CanvasGroup cGroup;
}

[System.Serializable]
public struct HUD_Subtitles
{
    public TMP_Text subtitle;
    public CanvasGroup cGroup;
}


//MIGHT_ADD_BULLET_SPREAD
[System.Serializable]
public struct HUD_Weapon
{
    public TMP_Text ammo;
    public GameObject crosshair_Hit;
    public CanvasGroup cGroup;
}

[System.Serializable]
public struct HUDStats
{
    public TMP_Text timeOfDay;
    public TMP_Text daysPassed;
    public Slider health;
    public CanvasGroup cGroup;

    public void Start()
    {
        health.maxValue = 100;
    }

    public void Update()
    {
        timeOfDay.text = WorldManager.Instance.GetTimeOfDay();
        daysPassed.text = WorldManager.Instance.DaysPassed.ToString();
        health.value = PlayerCharacter.Instance.Health;
    }
}