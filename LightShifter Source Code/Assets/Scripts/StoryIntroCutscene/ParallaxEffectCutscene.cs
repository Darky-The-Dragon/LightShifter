using System.Collections;
using System.Collections.Generic;
using StoryIntroCutscene;
using TarodevController;
using UnityEngine;
using UnityEngine.UIElements;
public class ParallaxEffectCutscene : MonoBehaviour
{
    [SerializeField] UnityEngine.Camera cam;
    [SerializeField] Transform followTarget;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] CutsceneManager cutsceneManager;

    private Vector2 startingPosition;
    private float _time;
    private Vector2 canMoveSinceStart => new Vector2(playerStats.BaseSpeed * _time, 0) - startingPosition;
    private float startingZ;
    private float zDistanceToTarget => transform.position.z - followTarget.transform.position.z;
    private float clippingPlane => (cam.transform.position.z + (zDistanceToTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    public float parallaxFactor => Mathf.Abs(zDistanceToTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(!cutsceneManager.GetEnabledParallax())
            return;
        _time += Time.deltaTime;
        Vector2 newPosition = startingPosition + canMoveSinceStart * parallaxFactor;
        // Debug.Log(gameObject.name);
        // Debug.Log("canMoveSinceStart"+ canMoveSinceStart);
        // Debug.Log("newPosition" + newPosition);
        transform.position = new Vector3(newPosition.x, transform.position.y, startingZ);
    }
}
