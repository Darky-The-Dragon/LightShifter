using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] UnityEngine.Camera cam;
    [SerializeField] Transform followTarget;

    private Vector2 startingPosition;
    private Vector2 canMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    private float startingZ;
    private float zDistanceToTarget => transform.position.z - followTarget.transform.position.z;
    private float clippingPlane => (cam.transform.position.z + (zDistanceToTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    public float parallaxFactor => Mathf.Abs(zDistanceToTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + canMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, transform.position.y, startingZ);
    }
}
