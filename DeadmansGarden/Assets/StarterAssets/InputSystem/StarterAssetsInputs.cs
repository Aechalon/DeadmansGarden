using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviourPunCallbacks
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool crouch;
		public bool aim;
		public bool tab;
		public bool trigger;
		public float stamina = 1;
		public float staminaCos = 0.008f;
		public float staminaRec = 0.01f;
		[Header("Movement Settings")]
		public bool analogMovement;
		
		private PlayerControllerScript player;

		[Header("UI Components")]
		[SerializeField] private Image imgStamina;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private void Awake()
        {
            if(photonView.IsMine)
            {
				player = GetComponent<PlayerControllerScript>();
		
            }
        }
        private void Update()
        {
		
			if(!sprint && stamina < 1)
            {
				stamina += staminaRec;
            }
			else if(stamina > .3
				&& sprint)
            {
				stamina -= staminaCos ;
            }
			if(jump)
			{
				stamina -= (staminaCos * 10);
			}
			imgStamina.fillAmount = stamina;

			if(aim && crouch)
			{ move = new Vector2(0, 0); }
		}


#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			
				MoveInput(value.Get<Vector2>());
			
           
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (!aim)
			{
				JumpInput(value.isPressed);
			}
		}

		public void OnSprint(InputValue value)
		{
			if (!crouch)
			{
				SprintInput(value.isPressed);

			}
		
		}
		public void OnCrouch(InputValue value)
		{
			CrouchInput(value.isPressed);
		}
		public void OnTab(InputValue value)
		{
			TabInput(value.isPressed);
		}
		public void OnAim(InputValue value)
		{
		
		    AimInput(value.isPressed);

			

		}
		public void OnTrigger(InputValue value)
		{
			
	     	TriggerInput(value.isPressed);

	
		}

#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
	
				move = newMoveDirection;
				player._PickUpAnimationEnd();
				player._AnimatorNormalRoot();
			
		
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
			player._PickUpAnimationEnd();
			player._AnimatorNormalRoot();
		}
		public void CrouchInput(bool newCrouchState)
		{
			crouch = newCrouchState;
			player._PickUpAnimationEnd();
			player._AnimatorNormalRoot();
		}
		public void TabInput(bool newTabState)
		{
			tab = newTabState;
		
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void AimInput(bool newAimState)
		{

			aim = newAimState;
		}
		public void TriggerInput(bool newTriggerState)
		{

			trigger = newTriggerState;
		}


#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}