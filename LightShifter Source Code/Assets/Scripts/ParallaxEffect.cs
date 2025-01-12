using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform followTarget;

    private Vector2 startingPosition;
    private float startingZ;

    private Vector2 canMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    private float zDistanceToTarget => transform.position.z - followTarget.transform.position.z;

    private float clippingPlane =>
        cam.transform.position.z + (zDistanceToTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);

    public float parallaxFactor => Mathf.Abs(zDistanceToTarget) / clippingPlane;

    // Start is called before the first frame update
    private void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    private void Update()
    {
        var newPosition = startingPosition + canMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}