using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private PlatformSide side;

    private List<Platform> platforms;

    private void Awake()
    {
        // the true parameter is to include inactive objects in the search
        platforms = new List<Platform>(GetComponentsInChildren<Platform>(true));
    }

    public void SetPlatformsState(bool newState)
    {
        // foreach (Platform platform in platforms)
        // {
        //     if (platform == null)
        //     {
        //         Debug.LogError("Platform not found");
        //     }
        //     platform.gameObject.SetActive(newState);
        // }
        for (var i = 0; i < platforms.Count; i++)
        {
            var platform = platforms[i];
            if (platform == null) Debug.LogError("Platform not found");
            platform.gameObject.SetActive(newState);
        }
    }

    public PlatformSide GetSide()
    {
        return side;
    }
}