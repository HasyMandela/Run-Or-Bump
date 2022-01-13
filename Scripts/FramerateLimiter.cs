using UnityEngine;

public class FramerateLimiter : MonoBehaviour
{
    public int TargetFramerate
    {
        get => _targetFramerate;
        set => _targetFramerate = value;
    } 
    
    protected void Start()
    {
        Application.targetFrameRate = _targetFramerate;
        QualitySettings.vSyncCount = _vsync ? 1 : 0;
    }

    protected void LateUpdate()
    {
        if (Application.targetFrameRate != _targetFramerate)
        {
            Application.targetFrameRate = _targetFramerate;
        }
        
        var vsync = _vsync ? 1 : 0;

        if (QualitySettings.vSyncCount != vsync)
        {
            QualitySettings.vSyncCount = _vsync ? 1 : 0;
        }
    }

    [SerializeField]
    private int _targetFramerate = 60;
    
    [SerializeField]
    private bool _vsync = true;
}
