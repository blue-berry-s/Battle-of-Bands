using UnityEngine;
using UnityEngine.Events;

//takes care of the physics behind actions

//@author: Brackeys
public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;
	[SerializeField] private float m_JumpForce = 600f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing =  0f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching


	const float k_GroundedRadius = .17f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	private bool canGoForward = true;
	private bool canGoBackward = true;
	private float maxX = 0;
	private float minX = 0;
	

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

		//Debug.Log(m_Rigidbody2D.linearVelocityX);

		if (!canGoForward && m_Rigidbody2D.linearVelocityX > 0 && playerTransform.position.x > maxX)
		{
			playerTransform.position = new Vector3 (maxX, playerTransform.position.y, playerTransform.position.z);
			m_Rigidbody2D.linearVelocityX = 0;
		}
		else if (!canGoBackward && m_Rigidbody2D.linearVelocityX < 0 && playerTransform.position.x < maxX) {
			playerTransform.position = new Vector3(minX, playerTransform.position.y, playerTransform.position.z);
			m_Rigidbody2D.linearVelocityX = -m_Rigidbody2D.linearVelocityX;
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
		if (m_Grounded || m_AirControl)
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

			

			// If the input is moving the player right and the player is facing left...
			//if (move > 0 && !m_FacingRight)
			//{
				// ... flip the player.
			//	Flip();
			//}
			// Otherwise if the input is moving the player left and the player is facing right...
			//else if (move < 0 && m_FacingRight)
			//{
				// ... flip the player.
			//	Flip();
			//}
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
		//Debug.Log("m_grounded: " + m_Grounded);
		//Debug.Log(m_Rigidbody2D.linearVelocity);
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool isGrounded() {
		return m_Grounded;
	}

	public void forwardBlocked(float pos) {
		canGoForward = false;
		maxX = pos;
	}

	public void backwardBlocked(float pos) {
		canGoBackward = false;
		minX = pos;
	}

	public void forwardOpen() {
		canGoForward = true;
	}

	public void backwardOpen() {
		canGoBackward = true;
	}
}
