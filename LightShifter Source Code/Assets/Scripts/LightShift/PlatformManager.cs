using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{

    private List<Platform> platforms;
    [SerializeField] private PlatformSide side;
    void Awake()
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
        for (int i = 0; i < platforms.Count; i++)
        {
            Platform platform = platforms[i];
            if (platform == null)
            {
                Debug.LogError("Platform not found");
            }
            platform.gameObject.SetActive(newState);
        }

    }

    public PlatformSide GetSide()
    {
        return side;
    }
}
