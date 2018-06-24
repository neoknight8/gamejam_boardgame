using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public delegate void RunFinishedHandler();

    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
		private int m_horizontal = 0;
		private int m_vertical = 0;

		private RunFinishedHandler onRunFinishedListener;
        
        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = m_horizontal;
            float v = m_vertical;
            bool crouch = Input.GetKey(KeyCode.C);

            // we use world-relative directions in the case of no main camera
            m_Move = v*m_Character.transform.forward + h*Vector3.right;
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }

		public void Run(){
			StartCoroutine (runForSeconds ());	
		}

		private IEnumerator runForSeconds(){
			m_vertical = 1;
			yield return new WaitForSeconds (1.32f);
			onRunFinished ();
			m_vertical = 0;
		}

		public void onRunFinished (){
			if (onRunFinishedListener != null) {
				onRunFinishedListener ();
			}
		}

		public void setOnRunFinished(RunFinishedHandler handler){
			onRunFinishedListener = handler;
		}
    }
}
