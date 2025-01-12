using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StoryIntroCutscene
{
    public class AnimatorTest : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Animator anim;

        [SerializeField] private GameObject effectsParent;
        [SerializeField] private Transform trailRenderer;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TrailRenderer trail;


        [Header("Particles")] [SerializeField] private ParticleSystem _jumpParticles;
        [SerializeField] private ParticleSystem _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles;
        [SerializeField] private ParticleSystem _landParticles;

        [SerializeField] private ParticleSystem _doubleJumpParticles;

        private float _originalTrailTime;
        //[SerializeField] private ParticleSystem _dashParticles;
        //[SerializeField] private ParticleSystem _dashRingParticles;
        //[SerializeField] private Transform _dashRingTransform;


        /*
        [Header("Audio Clips")] [SerializeField]
        private AudioClip _doubleJumpClip;

        [SerializeField] private AudioClip _dashClip;
        [SerializeField] private AudioClip[] _jumpClips;
        [SerializeField] private AudioClip[] _splats;
        [SerializeField] private AudioClip[] _slideClips;
        [SerializeField] private AudioClip _wallGrabClip;
        */


        //private AudioSource _source;
        private IPlayerController _player;

        //private Vector2 _defaultSpriteSize;
        //private GeneratedCharacterSize _character;
        private Vector3 _trailOffset;
        private Vector2 _trailVel;

        private void Awake()
        {
            //_source = GetComponent<AudioSource>();
            _player = GetComponentInParent<IPlayerController>();
            //_character = _player.Stats.CharacterSize.GenerateCharacterSize();
            //_defaultSpriteSize = new Vector2(1, _character.Height);

            _trailOffset = trailRenderer.localPosition;
            trailRenderer.SetParent(null);
            _originalTrailTime = trail.time;
        }

        private void Update()
        {
            if (_player == null)
            {
                Debug.Log("player is null");
                return;
            }

            var xInput = _player.Input.x;
            var yInput = _player.Input.y;
            //var velocity = _player.Velocity;

            SetParticleColor(-_player.Up, _moveParticles);

            HandleSpriteFlip(xInput);

            HandleMovementAnimation(xInput, yInput);

            HandleWallSlideEffects();
        }

        private void LateUpdate()
        {
            trailRenderer.position = Vector2.SmoothDamp(trailRenderer.position, transform.position + _trailOffset,
                ref _trailVel, 0.02f);
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;
            //_player.DashChanged += OnDashChanged;
            //_player.WallGrabChanged += OnWallGrabChanged;
            //_player.Repositioned += PlayerOnRepositioned;
            _player.ToggledPlayer += PlayerOnToggledPlayer;

            _moveParticles.Play();
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;
            //_player.DashChanged -= OnDashChanged;
            _player.WallGrabChanged -= OnWallGrabChanged;
            //_player.Repositioned -= PlayerOnRepositioned;
            _player.ToggledPlayer -= PlayerOnToggledPlayer;

            _moveParticles.Stop();
        }

        /*
        private void PlayerOnRepositioned(Vector2 newPosition)
        {
            StartCoroutine(ResetTrail());

            IEnumerator ResetTrail()
            {
                trail.time = 0;
                yield return new WaitForSeconds(0.1f);
                trail.time = _originalTrailTime;
            }
        }
        */

        private void PlayerOnToggledPlayer(bool on)
        {
            effectsParent.SetActive(on);
        }

        #region Walls & Ladders

        [Header("Walls & Ladders")] [SerializeField]
        private ParticleSystem _wallSlideParticles;

        //[SerializeField] private AudioSource _wallSlideSource;
        //[SerializeField] private AudioClip[] _wallClimbClips;
        //[SerializeField] private AudioClip[] _ladderClimbClips;
        //[SerializeField] private float _maxWallSlideVolume = 0.2f;
        [SerializeField] private float _wallSlideParticleOffset = 0.3f;
        [SerializeField] private float _distancePerClimbSound = 0.2f;

        private bool _isOnWall, _isSliding;
        private float _slidingVolumeGoal;
        private float _slideAudioVel;
        private bool _ascendingLadder;

        private float _lastClimbSoundY;

        //private int _wallClimbAudioIndex = 0;
        private int _ladderClimbAudioIndex;

        private void OnWallGrabChanged(bool onWall)
        {
            _isOnWall = onWall;
            //if(_isOnWall) PlaySound(_wallGrabClip, 0.5f);
        }

        private void HandleWallSlideEffects()
        {
            var slidingThisFrame = _isOnWall && !_grounded && _player.Velocity.y < 0;

            if (!_isSliding && slidingThisFrame)
            {
                _isSliding = true;
                _wallSlideParticles.Play();
            }
            else if (_isSliding && !slidingThisFrame)
            {
                _isSliding = false;
                _wallSlideParticles.Stop();
            }

            SetParticleColor(new Vector2(_player.WallDirection, 0), _wallSlideParticles);
            _wallSlideParticles.transform.localPosition =
                new Vector3(_wallSlideParticleOffset * _player.WallDirection, 0, 0);

            var requiredAudio = _isSliding || (_player.ClimbingLadder && _player.Velocity.y < 0);
            var point = requiredAudio ? Mathf.InverseLerp(0, -_player.Stats.LadderSlideSpeed, _player.Velocity.y) : 0;
            //_wallSlideSource.volume = Mathf.SmoothDamp(_wallSlideSource.volume, Mathf.Lerp(0, _maxWallSlideVolume, point), ref _slideAudioVel, 0.2f);

            if ((_player.ClimbingLadder || _isOnWall) && _player.Velocity.y > 0)
            {
                if (!_ascendingLadder)
                {
                    _ascendingLadder = true;
                    _lastClimbSoundY = transform.position.y;
                    //Play();
                }

                if (transform.position.y >= _lastClimbSoundY + _distancePerClimbSound)
                    _lastClimbSoundY = transform.position.y;
                //Play();
            }
            else
            {
                _ascendingLadder = false;
            }

            /*
            void Play()
            {
                if (_isOnWall) PlayWallClimbSound();
                else PlayLadderClimbSound();
            }
            */
        }

        /*
        private void PlayWallClimbSound()
        {
            _wallClimbAudioIndex = (_wallClimbAudioIndex + 1) % _wallClimbClips.Length;
            PlaySound(_wallClimbClips[_wallClimbAudioIndex], 0.1f);
        }

        private void PlayLadderClimbSound()
        {
            _ladderClimbAudioIndex = (_ladderClimbAudioIndex + 1) % _ladderClimbClips.Length;
            PlaySound(_ladderClimbClips[_ladderClimbAudioIndex], 0.07f);
        }
        */

        #endregion

        #region Animation

        [Header("AnimationSpeed")] [SerializeField] [Range(0f, 1f)]
        private float maxAnimationSpeed = 1;

        // Speed up idle while running
        private void HandleMovementAnimation(float xInput, float yInput)
        {
            var xInputStrength = Mathf.Abs(xInput);
            var yInputStrength = Mathf.Abs(yInput);

            anim.SetFloat(XVelocity, Mathf.Lerp(-Mathf.Abs(_player.Velocity.x), 1, xInputStrength));
            anim.SetFloat(YVelocity, Mathf.Lerp(_player.Velocity.y, 1, yInputStrength));

            anim.SetFloat(AnimationSpeed, Mathf.Lerp(1, maxAnimationSpeed, Mathf.Abs(xInput)));
            _moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale,
                Vector3.one * xInputStrength, 2 * Time.deltaTime);
        }

        private void HandleSpriteFlip(float xInput)
        {
            if (_player.Input.x != 0) sprite.flipX = xInput < 0;
        }

        #endregion

        /*
        #region Crouch & Slide

        private bool _crouching;
        private Vector2 _currentCrouchSizeVelocity;

        private void HandleCrouching()
        {
            if (!_crouching && _player.Crouching)
            {
                //_source.PlayOneShot(_slideClips[Random.Range(0, _slideClips.Length)], Mathf.InverseLerp(0, 5, Mathf.Abs(_player.Velocity.x)));
                _crouching = true;
                CancelSquish();
            }
            else if (_crouching && !_player.Crouching)
            {
                _crouching = false;
            }

            if (!_isSquishing)
            {
                var percentage = _character.CrouchingHeight / _character.Height;
                _sprite.size = Vector2.SmoothDamp(_sprite.size, new Vector2(1, _crouching ? _character.Height * percentage : _character.Height), ref _currentCrouchSizeVelocity, 0.03f);
            }
        }

        #endregion
        */

        #region Event Callbacks

        private void OnJumped(JumpType type)
        {
            if (type is JumpType.Jump or JumpType.Coyote or JumpType.WallJump)
            {
                anim.SetTrigger(JumpKey);
                anim.ResetTrigger(GroundedKey);
                //PlayRandomSound(_jumpClips, 0.2f, Random.Range(0.98f, 1.02f));

                // Only play particles when grounded (avoid coyote)
                if (type is JumpType.Jump)
                {
                    SetColor(_jumpParticles);
                    SetColor(_launchParticles);
                    _jumpParticles.Play();
                }
            }
            else if (type is JumpType.AirJump)
            {
                //_source.PlayOneShot(_doubleJumpClip);
                _doubleJumpParticles.Play();
            }
        }

        private bool _grounded;
        //private Coroutine _squishRoutine;

        private void OnGroundedChanged(bool grounded, float impact)
        {
            _grounded = grounded;

            if (grounded)
            {
                anim.SetBool(GroundedKey, true);
                //CancelSquish();
                //_squishRoutine = StartCoroutine(SquishPlayer(Mathf.Abs(impact)));
                //_source.PlayOneShot(_splats[Random.Range(0, _splats.Length)],0.5f);
                _moveParticles.Play();

                _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, Mathf.Abs(impact));
                SetColor(_landParticles);
                _landParticles.Play();
            }
            else
            {
                anim.SetBool(GroundedKey, false);
                _moveParticles.Stop();
            }
        }

        /*
        private void OnDashChanged(bool dashing, Vector2 dir)
        {
            if (dashing)
            {
                _dashParticles.Play();
                _dashRingTransform.up = dir;
                _dashRingParticles.Play();
                //_source.PlayOneShot(_dashClip,0.5f);
            }
            else
            {
                _dashParticles.Stop();
            }
        }
        */

        #endregion


        #region Helpers

        private ParticleSystem.MinMaxGradient _currentGradient;

        private void SetParticleColor(Vector2 detectionDir, ParticleSystem system)
        {
            // Perform raycast
            var ray = Physics2D.Raycast(transform.position, detectionDir, 2);
            if (ray.collider == null) return;

            // Attempt to handle Tilemap colors
            if (ray.transform.TryGetComponent(out Tilemap tilemap))
            {
                var gridPosition = tilemap.WorldToCell(ray.point);
                var tileBase = tilemap.GetTile(gridPosition);

                if (tileBase != null)
                {
                    var tileSprite = tilemap.GetSprite(gridPosition);
                    if (tileSprite != null)
                    {
                        var texture = tileSprite.texture;
                        var rect = tileSprite.textureRect;
                        var averageColor = GetAverageColor(texture, rect);

                        _currentGradient = new ParticleSystem.MinMaxGradient(averageColor * 0.9f, averageColor * 1.2f);
                    }
                    else
                    {
                        _currentGradient = new ParticleSystem.MinMaxGradient(Color.white);
                    }
                }
                else
                {
                    _currentGradient = new ParticleSystem.MinMaxGradient(Color.white);
                }
            }
            // Attempt to handle SpriteRenderer colors
            else if (ray.transform.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                _currentGradient =
                    new ParticleSystem.MinMaxGradient(spriteRenderer.color * 0.9f, spriteRenderer.color * 1.2f);
            }
            else
            {
                _currentGradient = new ParticleSystem.MinMaxGradient(Color.white);
            }

            // Apply the gradient to the particle system
            SetColor(system);
        }

        private Color GetAverageColor(Texture2D texture, Rect rect)
        {
            // Directly retrieve the pixels of the sprite's texture rect
            var pixels = texture.GetPixels(
                (int)rect.x,
                (int)rect.y,
                (int)rect.width,
                (int)rect.height
            );

            // Use LINQ to calculate the average color (optional for cleaner code)
            var averageColor = new Color(
                pixels.Average(p => p.r),
                pixels.Average(p => p.g),
                pixels.Average(p => p.b),
                pixels.Average(p => p.a)
            );

            return averageColor;
        }


        private void SetColor(ParticleSystem ps)
        {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        /*
        private void PlayRandomSound(IReadOnlyList<AudioClip> clips, float volume = 1, float pitch = 1)
        {
            PlaySound(clips[Random.Range(0, clips.Count)], volume, pitch);
        }

        private void PlaySound(AudioClip clip, float volume = 1, float pitch = 1)
        {
            _source.pitch = pitch;
            _source.PlayOneShot(clip, volume);
        }
        */

        #endregion

        #region Animation Keys

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
        private static readonly int JumpKey = Animator.StringToHash("Jump");

        #endregion
    }
}