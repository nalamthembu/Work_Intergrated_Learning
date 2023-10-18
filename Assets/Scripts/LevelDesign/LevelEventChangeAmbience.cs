public class LevelEventChangeAmbience : LevelEventTrigger
{
    public ChangeAmbienceEvent eventParameters;

    private void Awake()
    {
        Init(EVENT_TYPE.CHANGE_AMBIENCE);
    }
}