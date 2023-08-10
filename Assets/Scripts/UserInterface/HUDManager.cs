using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

//Handles all HUD UI like health, objectives etc.
public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtSubtitles;
    [SerializeField] private TMP_Text txtNotification;

    public static HUDManager instance;

    //canvas group naming convention "cg_nameOfGroup"
    private CanvasGroup cg_subtitleGroup;
    private CanvasGroup cg_NotificationGroup;

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

        cg_subtitleGroup = txtSubtitles.transform.parent.GetComponent<CanvasGroup>();
        cg_NotificationGroup = txtNotification.transform.parent.GetComponent<CanvasGroup>();
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
        while (cg_subtitleGroup.alpha != 1)
        {
            cg_subtitleGroup.alpha += Time.deltaTime * 1.5F;

            if (System.MathF.Round(cg_subtitleGroup.alpha, 2) >= 0.999F)
                cg_subtitleGroup.alpha = 1;

            yield return new WaitForEndOfFrame();
        }

        txtSubtitles.text = text;

        yield return new WaitForSeconds(duration);

        while (cg_subtitleGroup.alpha != 0)
        {
            cg_subtitleGroup.alpha -= Time.deltaTime * 1.5F;

            if (System.MathF.Round(cg_subtitleGroup.alpha, 2) <= 0.001F)
                cg_subtitleGroup.alpha = 0;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator IShowNotification(string text)
    {
        while (cg_NotificationGroup.alpha != 1)
        {
            cg_NotificationGroup.alpha += Time.deltaTime * 1.5F;

            if (System.MathF.Round(cg_NotificationGroup.alpha, 2) >= 0.999F)
                cg_NotificationGroup.alpha = 1;

            yield return new WaitForEndOfFrame();
        }

        txtNotification.text = text;

        yield return new WaitForSeconds(4F);

        while (cg_NotificationGroup.alpha != 0)
        {
            cg_NotificationGroup.alpha -= Time.deltaTime * 1.5F;

            if (System.MathF.Round(cg_NotificationGroup.alpha, 2) <= 0.001F)
                cg_NotificationGroup.alpha = 0;

            yield return new WaitForEndOfFrame();
        }
    }

    private void PlayNotificationSound()
    {
        print("ui notification sound is meant to be played");
    }
}