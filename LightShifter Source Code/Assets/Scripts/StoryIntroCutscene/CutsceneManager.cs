using System.Collections;
using System.Collections.Generic;
using LightShift;
using TarodevController;
using UnityEngine;

namespace StoryIntroCutscene {
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private LightShifter lightShifter;
    private FrameInput _cutsceneframeInput;
    private bool _enableParallax, _freezeMovement;

    void Start()
    {
        // _playerObjectAnim.Play(CutsceneKey);
        _cutsceneframeInput = new FrameInput
        {
            JumpDown = false,
            JumpHeld = false,
            DashDown = false,
            Move = new Vector2(0,0),
            LightShift = false
        };
        
        StartCoroutine(CutsceneCoroutine());
    }

    public FrameInput GetFrameInput() {
        return _cutsceneframeInput;
    }

    public bool GetEnabledParallax() {
        return _enableParallax;
    }
    public bool GetFreezePlayerMovement() {
        return _freezeMovement;
    }

    private IEnumerator CutsceneCoroutine() {
        _enableParallax = false;
        _freezeMovement = false;
        // wait 1.5 seconds
        yield return new WaitForSeconds(1.5f);
        // block the shift after waiting, to ensure the LightShifter component has been loaded previously
        lightShifter.BlockShift();

        // move to the right until reached x = -29
        _cutsceneframeInput.Move = new Vector2(1,0);
        while(player.transform.position.x <= -29.35)
            yield return 0;
        // stop moving and wait 1 second
        _cutsceneframeInput.Move = new Vector2(0,0);
        yield return new WaitForSeconds(1f);

        // flip the player, wait 1 second, flip again and wait 1 second
        _freezeMovement = true;
        _cutsceneframeInput.Move = new Vector2(-0.001f, 0);
        yield return new WaitForSeconds(1f);
        _cutsceneframeInput.Move = new Vector2(+0.001f, 0);
        yield return new WaitForSeconds(1f);

        // start running on the spot towards the right
        _enableParallax = true;
        _cutsceneframeInput.Move = new Vector2(1, 0);

        yield return new WaitForSeconds(3f);
        lightShifter.ToggleEnvironment();

        yield return new WaitForSeconds(3f);
        lightShifter.ToggleEnvironment();

        yield return null;
    }
    private static readonly int CutsceneKey = Animator.StringToHash("Cutscene");
    
}
}

