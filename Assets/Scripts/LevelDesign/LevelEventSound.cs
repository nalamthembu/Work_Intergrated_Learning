public class LevelEventSound : LevelEventTrigger
{
    public PlaySoundEvent eventParameters;

    private void Awake()
    {
        Init(EVENT_TYPE.PLAY_SOUND);
    }
}