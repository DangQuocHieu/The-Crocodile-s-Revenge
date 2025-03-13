using UnityEngine;

public enum AudioName
{
    MenuMusic, 
    GameMusic,
    CoinCollectedSFX,
    PowerupSFX,
    PowerdownSFX,
    ExplosionSFX,
    HurtSFX,
    FirstJumpSFX, 
    MultipleJumpSFX,
    GameOverSFX,
    GameStartSFX,
    SpikeBrokenSFX,
    None
}

[System.Serializable]
public class Audio
{
    public AudioName audioName;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    [Range(0f, 256f)] public int priority = 128;
}
