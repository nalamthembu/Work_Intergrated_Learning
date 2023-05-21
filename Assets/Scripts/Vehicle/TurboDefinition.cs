using UnityEngine;

[CreateAssetMenu(fileName = "TurboDefinition", menuName = "Game/Vehicle/Turbo Definition")]
public class TurboDefinition : ScriptableObject
{
    [Header("0 means zero turbo lag (essentially a supercharger")]
    public float spoolTime;
    public float rpmToKickIn;
    public float boostNm;
}