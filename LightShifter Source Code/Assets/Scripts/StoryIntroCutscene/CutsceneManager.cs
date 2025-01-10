using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

namespace StoryIntroCutscene {
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
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
        _enableParallax = false;
        _freezeMovement = false;
        StartCoroutine(CutsceneCoroutine());


        
    }

    public FrameInput GetFrameInput() {
        return _cutsceneframeInput;
    }

    public bool GetEnabledParallax() {
        return _enableParallax;
    }
    public bool GetFreezeMovement() {
        return _freezeMovement;
    }

    private IEnumerator CutsceneCoroutine() {
        yield return new WaitForSeconds(1.5f);

        _cutsceneframeInput.Move = new Vector2(1,0);

        while(player.transform.position.x <= -29)
            yield return 0;

        _cutsceneframeInput.Move = new Vector2(0,0);
        yield return new WaitForSeconds(1f);

        _freezeMovement = true;
        _cutsceneframeInput.Move = new Vector2(-0.001f, 0);
        yield return new WaitForSeconds(1f);

        _cutsceneframeInput.Move = new Vector2(+0.001f, 0);
        yield return new WaitForSeconds(1f);

        _enableParallax = true;
        _cutsceneframeInput.Move = new Vector2(1, 0);

        yield return null;
    }
    private static readonly int CutsceneKey = Animator.StringToHash("Cutscene");
    
}
}

