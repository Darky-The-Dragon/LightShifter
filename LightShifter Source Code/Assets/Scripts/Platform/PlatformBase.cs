using UnityEngine;

namespace TarodevController.Demo
{
    public abstract class PlatformBase : MonoBehaviour, IPhysicsObject, IPhysicsMover 
    {
        [SerializeField] private bool _requireGrounding;
        [SerializeField] private BoxCollider2D _boundingEffector;
        [SerializeField] private bool _useTakeOffVelocity;

        [HideInInspector] protected Rigidbody2D Rb;

        protected float Time;

        protected virtual void Awake()
        {
            _OriginalFreezeMovement = _freezeMovement;
            Rb = GetComponent<Rigidbody2D>();
            Rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            PhysicsSimulator.Instance.AddPlatform(this);
        }

        public void Start()
        {
            _startingPosition = transform.position;
        }

        protected virtual void OnDestroy()
        {
            PhysicsSimulator.Instance.RemovePlatform(this);
        }

        public virtual void OnValidate()
        {
            if (_boundingEffector) _boundingEffector.isTrigger = true;
        }

        public bool UsesBounding => _boundingEffector != null;
        public bool RequireGrounding => _requireGrounding;
        public Vector2 FramePositionDelta { get; private set; }
        public Vector2 FramePosition => Rb.position;
        public Vector2 Velocity => Rb.velocity;
        public Vector2 TakeOffVelocity => _useTakeOffVelocity ? Velocity : Vector2.zero;
        [SerializeField] public bool _freezeMovement = false;
        private bool _OriginalFreezeMovement = false;
        private Vector3 _startingPosition;
        public void EnableMovement(bool enable)
        {
            _freezeMovement = !enable;

        }

        public virtual void Reset()
        {
            _freezeMovement = _OriginalFreezeMovement;
            transform.position = _startingPosition;
        }


        public void TickFixedUpdate(float delta)
        {
            var newPos = Evaluate(delta);
            // if _freezeMovement is true, we don't want to update the position
            if(_freezeMovement) {
                Rb.gravityScale = 0;
                newPos = Rb.position;
            }
            // Position
            var positionDifference = newPos - Rb.position;

            if (positionDifference.sqrMagnitude > 0)
            {
                FramePositionDelta = positionDifference;
                Rb.velocity = FramePositionDelta / delta;
                //Debug.Log("Velocity: " + this.gameObject.name + " " + Rb.velocity);
            }
            else
            {
                FramePositionDelta = Vector3.zero;
                Rb.velocity = Vector3.zero;
            }
            

        }

        public void TickUpdate(float delta, float time)
        {
            Time = time;
        }

        protected abstract Vector2 Evaluate(float delta);
    }
}