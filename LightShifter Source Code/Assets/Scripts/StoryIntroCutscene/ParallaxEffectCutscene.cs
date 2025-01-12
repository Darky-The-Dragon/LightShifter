using StoryIntroCutscene;
using TarodevController;
using UnityEngine;

public class ParallaxEffectCutscene : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform followTarget;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CutsceneManager cutsceneManager;
    private float _time;

    private Vector2 startingPosition;
    private float startingZ;
    private Vector2 canMoveSinceStart => new Vector2(playerStats.BaseSpeed * _time, 0) - startingPosition;
    private float zDistanceToTarget => transform.position.z - followTarget.transform.position.z;

    private float clippingPlane =>
        cam.transform.position.z + (zDistanceToTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);

    public float parallaxFactor => Mathf.Abs(zDistanceToTarget) / clippingPlane;

    // Start is called before the first frame update
    private void Start()
    {
        _time = 0;
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!cutsceneManager.GetEnabledParallax())
            return;
        _time += Time.deltaTime;
        var newPosition = startingPosition + canMoveSinceStart * parallaxFactor;
        // Debug.Log(gameObject.name);
        // Debug.Log("canMoveSinceStart"+ canMoveSinceStart);
        // Debug.Log("newPosition" + newPosition);
        transform.position = new Vector3(newPosition.x, transform.position.y, startingZ);
    }
}