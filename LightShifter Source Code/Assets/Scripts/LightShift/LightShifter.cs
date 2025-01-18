using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace LightShift
{
    public class LightShifter : MonoBehaviour
    {
        [SerializeField] private GameObject gridLight, gridDark;
        [SerializeField] private bool startWithLight = true;

        [SerializeField] private bool canChange = true, isShiftLoaded = true;
        [SerializeField] private float shiftCooldown = 0.5f;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip transitionClip;
        private LightShifter _instance;
        private Collider2D[] _lightColliders, _darkColliders;
        private SortingGroup _lightSortingGroup, _darkSortingGroup;
        private bool _start;
        private readonly float _volume = 0.1f;

        public bool IsLight { get; private set; }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                _lightSortingGroup = gridLight.GetComponent<SortingGroup>();
                _lightColliders = gridLight.GetComponentsInChildren<Collider2D>();

                _darkSortingGroup = gridDark.GetComponent<SortingGroup>();
                _darkColliders = gridDark.GetComponentsInChildren<Collider2D>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            _start = true;
            if (startWithLight)
            {
                IsLight = true;
                ShowLight();
                _start = false;
            }
            else
            {
                IsLight = false;
                ShowDark();
                _start = false;
            }

            canChange = true;
            isShiftLoaded = true;
        }

        public event Action<bool> OnWorldShifted; // passes 'true' if light, 'false' if dark

        public void BlockShift()
        {
            canChange = false;
        }

        public void EnableShift()
        {
            canChange = true;
        }

        private IEnumerator ShiftCooldown()
        {
            isShiftLoaded = false;
            yield return new WaitForSeconds(shiftCooldown);
            isShiftLoaded = true;
        }

        public bool GetShiftEnabled()
        {
            return canChange && isShiftLoaded;
        }

        public void ToggleEnvironment()
        {
            if (IsLight)
                ShowDark();
            else
                ShowLight();
            // Flip the toggle state
            IsLight = !IsLight;

            // Fire the event
            OnWorldShifted?.Invoke(IsLight);
        }

        private void ShowLight()
        {
            // Debug.Log("Showing Light");
            SetColliderTrigger(_lightColliders, false);
            SetColliderTrigger(_darkColliders, true);

            // move the dark world beneath the light world
            _darkSortingGroup.sortingOrder = _lightSortingGroup.sortingOrder - 1;

            if (!_start)
                audioSource.PlayOneShot(transitionClip, _volume);
        }

        private void ShowDark()
        {
            // Debug.Log("Showing Dark");

            SetColliderTrigger(_darkColliders, false);
            SetColliderTrigger(_lightColliders, true);

            // move the dark world on top of the light world
            _darkSortingGroup.sortingOrder = _lightSortingGroup.sortingOrder + 1;

            if (!_start)
                audioSource.PlayOneShot(transitionClip, _volume);
        }

        private void SetColliderTrigger(Collider2D[] colliders, bool isTrigger)
        {
            foreach (var collision in colliders) collision.isTrigger = isTrigger;
        }
    }
}