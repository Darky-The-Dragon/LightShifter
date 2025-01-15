using System;
using System.Collections;
using System.Collections.Generic;
using LightShift;
using Newtonsoft.Json;
using TarodevController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StoryIntroCutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        private static readonly int CutsceneKey = Animator.StringToHash("Cutscene");
        [SerializeField] private GameObject player;
        [SerializeField] private LightShifter lightShifter;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float fadeInOutDuration = 1f;
        [SerializeField] private GameObject lightWorld, darkWorld;
        [SerializeField, Range(0,5)] private float floorSpeed = 1.5f;
        [SerializeField] private KeyCode skipKey = KeyCode.Space;
        [SerializeField] private Animator sceneTransitionAnim;

        private FrameInput _cutsceneframeInput;
        private bool _enableParallax, _freezeMovement, _moveEnvironment;
        private List<StoryText> _storyTexts;


        private void Start()
        {
            var jsonStoryTexts = Resources.Load<TextAsset>("StoryTexts");
            _storyTexts = JsonConvert.DeserializeObject<List<StoryText>>(jsonStoryTexts.text);

            _cutsceneframeInput = new FrameInput
            {
                JumpDown = false,
                JumpHeld = false,
                DashDown = false,
                Move = new Vector2(0, 0),
                LightShift = false
            };

            StartCoroutine(CutsceneCoroutine());
            StartCoroutine(StoryTextCouroutine());
        }

        private void Update() {
            MoveFloor();
        }

        private void MoveFloor()
        {
            if(_moveEnvironment) {
                lightWorld.transform.position -= new Vector3(floorSpeed * Time.deltaTime, 0, 0);
                darkWorld.transform.position -= new Vector3(floorSpeed * Time.deltaTime, 0, 0);
            }
        }

        private void GoToChapter1() {
            StartCoroutine(NextLevel());
        }




        /* start of CUTSCENE COROUTINES */

        private IEnumerator NextLevel()
        {
            sceneTransitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            sceneTransitionAnim.SetTrigger("Start");
        }

        private IEnumerator DisplayStoryTextCoroutine(StoryText x) {
            text.text = x.Text;
            text.CrossFadeAlpha(1, fadeInOutDuration, false);
            yield return new WaitForSeconds(fadeInOutDuration);
            yield return new WaitForSeconds((float)x.Duration);
            text.CrossFadeAlpha(0, fadeInOutDuration, false);
            yield return new WaitForSeconds(fadeInOutDuration);
        }

        private IEnumerator StoryTextCouroutine()
        {
            foreach (var x in _storyTexts)
            {
                Coroutine activeCoroutine = StartCoroutine(DisplayStoryTextCoroutine(x));
                float elapsedTime = 0;
                float timeToWait = x.Duration + 2 * fadeInOutDuration;
                while(elapsedTime < timeToWait) {
                    /* skip only if the DisplayStoryTextCoroutine isn't fading out the current text */
                    if (Input.GetKeyDown(skipKey) && elapsedTime < (timeToWait - fadeInOutDuration)) {
                        /* stop the current coroutine */
                        StopCoroutine(activeCoroutine);

                        /* gently skip the current text */
                        text.CrossFadeAlpha(0, fadeInOutDuration, false);
                        yield return new WaitForSeconds(fadeInOutDuration);

                        /* go to the next story text */
                        break;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            /* once all the story texts have been displayed the cutscene can be considered finished */
            GoToChapter1();            
        }

        private IEnumerator CutsceneCoroutine()
        {
            _moveEnvironment = false;
            _enableParallax = false;
            _freezeMovement = false;
            
            yield return new WaitForSeconds(1.5f);
            // block the shift after waiting, to ensure the LightShifter component has been loaded previously
            lightShifter.BlockShift();

            // move to the right until reached x = -29
            _cutsceneframeInput.Move = new Vector2(1, 0);
            while (player.transform.position.x <= -29.35)
                yield return 0;
            // stop moving and wait 1 second
            _cutsceneframeInput.Move = new Vector2(0, 0);
            yield return new WaitForSeconds(1f);

            // flip the player, wait 1 second, flip again and wait 1 second
            _freezeMovement = true;
            _cutsceneframeInput.Move = new Vector2(-0.001f, 0);
            yield return new WaitForSeconds(1f);
            _cutsceneframeInput.Move = new Vector2(+0.001f, 0);
            yield return new WaitForSeconds(1f);

            // start running towards right, activate the parallax effect and move the floor
            _enableParallax = true;
            _cutsceneframeInput.Move = new Vector2(1, 0);
            _moveEnvironment = true;

            yield return new WaitForSeconds(3f);
            lightShifter.ToggleEnvironment();

            yield return new WaitForSeconds(3f);
            lightShifter.ToggleEnvironment();

            yield return null;
        }

        /* end of CUTSCENE COROUTINES */

        /* start of PUBLIC GETTERS */
        public FrameInput GetFrameInput()
        {
            return _cutsceneframeInput;
        }

        public bool GetEnabledParallax()
        {
            return _enableParallax;
        }

        public bool GetFreezePlayerMovement()
        {
            return _freezeMovement;
        }

        /* end of PUBLIC GETTERS */
    }

    [Serializable]
    internal class StoryText
    {
        public double Index { get; set; }
        public string Text { get; set; }
        public float Duration { get; set; }
    }
}