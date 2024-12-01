using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats stats;
        private bool _cachedQueryStartInColliders;
        private BoxCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;

        private bool _grounded;
        private int _jumps;
        private Rigidbody2D _rb;

        private float _time;

        private bool _walled;
        /*
            defines the orientation of the player, 1 is right, -1 is left
        */


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<BoxCollider2D>();

            _jumps = 0;

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (stats == null)
                Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump"),
                JumpHeld = Input.GetButton("Jump"),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < stats.HorizontalDeadZoneThreshold
                    ? 0
                    : Mathf.Sign(_frameInput.Move.x);
                _orientation = _frameInput.Move.x != 0 ? Mathf.Sign(_frameInput.Move.x) : _orientation;
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < stats.VerticalDeadZoneThreshold
                    ? 0
                    : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        #region Collisions

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground, Ceiling and Wall checks
            bool groundHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0, Vector2.down,
                stats.GrounderDistance, ~stats.PlayerLayer);
            bool ceilingHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0, Vector2.up,
                stats.GrounderDistance, ~stats.PlayerLayer);
            bool wallHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0,
                Vector2.right * _frameInput.Move.x, stats.WallDistance, ~stats.PlayerLayer);
            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            if (!_walled && wallHit)
            {
                _jumps = stats.AvailableJumps - stats.AvailableJumpsFromWall;
                _walled = true;
                _disableGravity = true;
            }
            else if (_walled && !wallHit)
            {
                _walled = false;
                _disableGravity = false;
            }


            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion

        private void ApplyMovement()
        {
            _rb.velocity = _frameVelocity;
        }

        private IEnumerator DisableMovementForFixedTime(float time)
        {
            _disableMovement = true;
            yield return new WaitForSeconds(time);
            _disableMovement = false;
            yield return null;
        }

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + stats.JumpBuffer;

        private void HandleJump()
        {
            if (_grounded) _jumps = 0;

            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_walled) ExecuteWallJump();
            else if (_grounded || _jumps < stats.AvailableJumps) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _frameVelocity.y = stats.JumpPower;
            Jumped?.Invoke();
            _jumps++;
            // print("input: " + _frameInput.Move.x);
            // print("velocity: " + _frameVelocity.x);
        }


        private void ExecuteWallJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _disableGravity = false;
            StartCoroutine(DisableMovementForFixedTime(stats.WallJumpMovementDisableTime));
            _frameVelocity.x = -_orientation * stats.WallJumpPower_X;
            _frameVelocity.y = stats.WallJumpPower_Y;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private float _orientation;
        private bool _disableMovement;

        private void HandleDirection()
        {
            if (!_disableMovement)
            {
                if (_frameInput.Move.x == 0)
                {
                    var deceleration = _grounded ? stats.GroundDeceleration : stats.AirDeceleration;
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
                }
                else
                {
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * stats.MaxSpeed,
                        stats.Acceleration * Time.fixedDeltaTime);
                }
            }
            else
            {
                _frameInput.Move.x = 0;
            }
        }

        #endregion

        #region Gravity

        private bool _disableGravity;

        private void HandleGravity()
        {
            _disableGravity = stats.NoGravity || _disableGravity;
            if (!_disableGravity)
            {
                if (_grounded && _frameVelocity.y <= 0f)
                {
                    _frameVelocity.y = stats.GroundingForce;
                }
                else
                {
                    var inAirGravity = stats.FallAcceleration;
                    if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= stats.JumpEndEarlyGravityModifier;
                    _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -stats.MaxFallSpeed,
                        inAirGravity * Time.fixedDeltaTime);
                }
            }
            else
            {
                _frameVelocity.y = 0;
            }
        }

        #endregion
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public Vector2 FrameInput { get; }
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
    }
}