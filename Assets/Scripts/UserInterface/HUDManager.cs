using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

//Handles all HUD UI like health, objectives etc.
public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUD_Subtitles HUD_Subtitles;
    [SerializeField] private HUD_Notifications HUDNotifications;
    [SerializeField] private HUD_Weapon HUDWeapon;
    [SerializeField] private HUDStats HUDStats;
    [SerializeField] private HUD_MiniMap HUDMiniMap;
    [SerializeField] private float instructionTimer = 4F;

    [SerializeField] private GameObject DEMO_END_SCREEN_GROUP;

    public void SetObjectivePrefabVisibilityAndLocation(Vector3 location, bool isVisible)
    {
        HUDMiniMap.PlaceObjectiveSprite(location, isVisible);
    }

    public void SHOW_DEMO_END_SCREEN()
    {
        DEMO_END_SCREEN_GROUP.SetActive(true);
    }

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
        HUDMiniMap.Start();
    }

    private void Update()
    {
        HUDStats.Update();
        HUDMiniMap.Update();
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

            if (HUDNotifications.cGroup.alpha >= 0.95F)
                HUDNotifications.cGroup.alpha = Mathf.Ceil(HUDNotifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }

        HUD_Subtitles.subtitle.text = text;

        yield return new WaitForSeconds(duration);

        while (HUD_Subtitles.cGroup.alpha != 0)
        {
            HUD_Subtitles.cGroup.alpha -= Time.deltaTime * 1.5F;

            if (HUDNotifications.cGroup.alpha <= 0.01F)
                HUDNotifications.cGroup.alpha = Mathf.Floor(HUDNotifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator IShowNotification(string text)
    {
        while (HUDNotifications.cGroup.alpha != 1)
        {
            HUDNotifications.cGroup.alpha += Time.deltaTime * 1.5F;

            if (HUDNotifications.cGroup.alpha >= 0.95F)
                HUDNotifications.cGroup.alpha = Mathf.Ceil(HUDNotifications.cGroup.alpha);

            yield return new WaitForEndOfFrame();
        }

        HUDNotifications.notification.text = string.Empty;

        HUDNotifications.notification.text = text;

        yield return new WaitForSeconds(instructionTimer);

        while (HUDNotifications.cGroup.alpha != 0)
        {
            HUDNotifications.cGroup.alpha -= Mathf.Floor(Time.deltaTime * 1.5F);

            if (HUDNotifications.cGroup.alpha <= 0.01F)
                HUDNotifications.cGroup.alpha = Mathf.Floor(HUDNotifications.cGroup.alpha);

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

[System.Serializable]
public struct HUD_MiniMap
{
    [SerializeField] Camera miniMapCamera;

    [SerializeField] GameObject playerSpritePrefab, objectiveSpritePrefab;

    private Transform playerSpriteInstance, objectiveSpriteInstance;

    private PlayerCharacter playerCharacter;

    [SerializeField][Range(1, 500)] private float size;
    [SerializeField][Range(1, 500)] private float height;

    public void Start()
    {
        playerCharacter = PlayerCharacter.Instance;
        miniMapCamera.transform.eulerAngles = Vector3.right * 90;
        playerSpriteInstance = Object.Instantiate(playerSpritePrefab, playerCharacter.transform.position, Quaternion.Euler(Vector3.right * 90)).transform;
        objectiveSpriteInstance = Object.Instantiate(objectiveSpritePrefab).transform;
        objectiveSpriteInstance.gameObject.SetActive(false);
    }

    public void Update()
    {
        UpdateCamera();
        UpdateUI();
    }

    public void PlaceObjectiveSprite(Vector3 position, bool isVisible)
    {
        objectiveSpriteInstance.position = position;
        objectiveSpriteInstance.gameObject.SetActive(isVisible);
    }
    
    private void UpdateCamera()
    {
        if (playerCharacter.IsInVehicle && playerCharacter.CurrentVehicle != null)
        {
            miniMapCamera.transform.eulerAngles = Vector3.right * 90 + Vector3.forward * playerCharacter.CurrentVehicle.transform.eulerAngles.y;
            miniMapCamera.transform.position = playerCharacter.CurrentVehicle.transform.position + Vector3.up * height;
            miniMapCamera.orthographicSize = Mathf.Lerp(miniMapCamera.orthographicSize, size, Time.deltaTime);
        }
        else
        {
            miniMapCamera.transform.eulerAngles = Vector3.right * 90 + Vector3.forward * playerCharacter.transform.eulerAngles.y;
            miniMapCamera.transform.position = playerCharacter.transform.position + Vector3.up * height;
            miniMapCamera.orthographicSize = Mathf.Lerp(miniMapCamera.orthographicSize, size, Time.deltaTime);
        }
    }

    private void UpdateUI()
    {
        if (playerCharacter.IsInVehicle && playerCharacter.CurrentVehicle != null)
        {
            playerSpriteInstance.transform.position = playerCharacter.CurrentVehicle.transform.position + Vector3.up * (height - 50.0F);
            playerSpriteInstance.eulerAngles = miniMapCamera.transform.eulerAngles;
        }
        else
        {
            playerSpriteInstance.transform.position = playerCharacter.transform.position + Vector3.up * (height - 50.0F); 
            playerSpriteInstance.eulerAngles = Vector3.right * 90 + Vector3.forward * playerCharacter.transform.eulerAngles.y;
        }
    }
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