using UnityEngine;
using UnityEngine.Events;

//takes care of the physics behind actions

//@author: Brackeys
public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;
	[SerializeField] private float m_JumpForce = 800f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing =  0f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	private float fallMultiplier = 3f;


	private Animator animator;


	const float k_GroundedRadius = .17f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;

	private bool canGoForward = true;
	private bool canGoBackward = true;
	private float maxX = 0;
	private float minX = 0;

	private bool canAir = true;
	

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

    private void Start()
    {
		animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		if (m_Rigidbody2D.linearVelocityY < 1) {
			m_Rigidbody2D.linearVelocityY += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}

		//Debug.Log(m_Rigidbody2D.linearVelocityX);
		//Debug.Log(canGoForward);

		if (!canGoForward && m_Rigidbody2D.linearVelocityX > 0 && playerTransform.position.x > maxX)
		{
			playerTransform.position = new Vector3 (maxX, playerTransform.position.y, playerTransform.position.z);
			m_Rigidbody2D.linearVelocityX = 0;
		}
		else if (!canGoBackward && m_Rigidbody2D.linearVelocityX < 0 && playerTransform.position.x < maxX) {
			playerTransform.position = new Vector3(minX, playerTransform.position.y, playerTransform.position.z);
			m_Rigidbody2D.linearVelocityX = -m_Rigidbody2D.linearVelocityX;
		}

		if (!canAir) {
			m_Rigidbody2D.linearVelocityY = 0;
		}

		//Debug.Log(m_Rigidbody2D.linearVelocity);
	}

	//prevent sliding when attacking
	public void stopSlide()
	{
		m_Rigidbody2D.linearVelocityX = 0;
		
	}


	public void Move(float move, bool crouch, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded && !animator.GetBool("isHurt"))
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.linearVelocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(m_Rigidbody2D.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			if (move > 0 || move < 0) {
				m_Rigidbody2D.linearVelocity = new Vector2 (move > 0 ?3:-3, 0);
			}
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public bool isGrounded() {
		return m_Grounded;
	}

	public void forwardBlocked(float pos) {
		canGoForward = false;
		maxX = pos;
		//Debug.Log("FORWAD BLOCKED: " + maxX);
	}

	public void backwardBlocked(float pos) {
		canGoBackward = false;
		minX = pos;
		//Debug.Log("BB");
	}

	public void forwardOpen() {
		canGoForward = true;
		//Debug.Log("FORWARD OPEN");
	}

	public void backwardOpen() {
		canGoBackward = true;
		//Debug.Log("BO");
	}

	public void airBlocked() {
		canAir = false;
	}

	public void airOpen() {
		canAir = true;
	}
}
