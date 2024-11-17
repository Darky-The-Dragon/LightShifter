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
        foreach (Platform platform in platforms)
        {
            platform.gameObject.SetActive(newState);
        }
    }

    public PlatformSide getSide()
    {
        return side;
    }
}
