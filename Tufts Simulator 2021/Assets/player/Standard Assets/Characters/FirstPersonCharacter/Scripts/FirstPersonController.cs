using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private Image m_punch1;
        [SerializeField] private Image m_punch2;
        [SerializeField] private Image bloodImage;
        [SerializeField] private AudioClip m_punch1sound;
        [SerializeField] private AudioClip m_punch2sound;
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;

        public int maxHealth = 100;
        public int initial_XP = 0;
        static int m_PlayerHealth;
        static int m_PlayerXP;
        private bool canShake;
        private Vector3 originalCameraPos;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private Text endgame_text;
        [SerializeField] private Text XPUI;

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        public float _shakeTimer;
        public float shakeAmount = 0.5f;
        private bool canattack;
        private bool isbloody;
        private AudioSource m_AudioSource;
        static int bosses_defeated = 0;
        static bool first_load = true;

        public HealthBar healthBar;

        // Use this for initialization
        private void Start()
        {
            if(m_PlayerXP == 0)
            {
              m_PlayerXP = initial_XP;
            }
            XPUI.text = "Player XP - " + m_PlayerXP;
            m_PlayerHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            canattack = true;
            isbloody = false;
            m_AudioSource = GetComponent<AudioSource>();
			      m_MouseLook.Init(transform , m_Camera.transform);
        }


        // Update is called once per frame
        private void Update()
        {
            //this rotates camera view with player rotation
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            //trigger for punch animation and sound
            //trigger for punch damage is in FistPunches.cs amd AIMover.cs
            if (canattack && Input.GetMouseButtonDown(0) && Time.timeScale == 1)
            {
                StartCoroutine(Punch());
            }
            //controls head bob for camera
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            //jump case for player is already in air
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }

        //call this function to add xp to the player (i.e. when they kill an enemy)
        public void addXP(int XP)
        {
          m_PlayerXP += XP;
          XPUI.text = "Player XP - " + m_PlayerXP;
        }

        //call this function to see how much XP the player has
        public int GetXP()
        {
          return m_PlayerXP;
        }

        //call this function every time a boss is defeated
        public void SetBossesDefeated()
        {
          bosses_defeated += 1;
          if (bosses_defeated >= 4)
          {
              SceneManager.LoadScene("Ending Scene");
          }
        }

        //call this function to see how many of the bosses are defeated (out of 4)
        public int GetBossesDefeated()
        {
          return bosses_defeated;
        }

        public void SetFirstLoad()
        {
          first_load = false;
        }

        public bool GetFirstLoad()
        {
          return first_load;
        }

        public void DamagePlayer(int Damage)
        {
          m_PlayerHealth -= Damage;
          healthBar.SetHealth(m_PlayerHealth);
          if (m_PlayerHealth <= 0)
          {
            //sends player to a game over screen when they die
            FindObjectOfType<GameManager>().EndGame();
          }

        }

        //increases the player's health (usually due to them collecting a smoothie)
        public void IncreaseHealth(int Health)
        {
          m_PlayerHealth += Health;
          if(m_PlayerHealth > 100)
          {
            m_PlayerHealth = 100;
          }
          //adjusts healthbar to reflect player health
          healthBar.SetHealth(m_PlayerHealth);

        }
        
        public int getHealth()
        {
            return m_PlayerHealth;
        }

        //this function starts a coroutine to display punch sound and annimation
        IEnumerator Punch()
        {
          //declares all variables to false to ensure we are not already punching,
          //  and can't punch again till after this function runs
          canattack = false;
          m_punch1.enabled = false;
          m_punch2.enabled = false;
          //this if / else check basically just randomly decides which of two
          //  punch images/sounds will play in this instance
          if (Random.Range(0, 2) == 1)
          {
            //setting m_punch.enabled = true displays the punch image on screen
            m_punch1.enabled = true;
            //this gets and plays the punch audio clip
            m_AudioSource.clip = m_punch1sound;
            m_AudioSource.Play();
            //this is why the function is a coroutine instead of a normal function -
            //  We must leave the punch image on screen for one second, and ensure the
            //  player doesn't spam their punches
            yield return new WaitForSeconds(1);
            m_punch1.enabled = false;
          }
          else
          {
            //everything here is a mirror of the above if check, but just with
            //  m_punch2 and m_punch2sound instead of m_punch1...
            m_punch2.enabled = true;
            m_AudioSource.clip = m_punch2sound;
            m_AudioSource.Play();
            yield return new WaitForSeconds(1);
            m_punch2.enabled = false;
          }
          // allows the player to punch again after the function has run
          canattack = true;
          m_punch1.enabled = false;
          m_punch2.enabled = false;
        }

        public void CameraShakeEffect()
        {
            //Debug.Log("in camera shake function");
            StartCoroutine(BloodFunction());
            //Debug.Log("after");
        }

       IEnumerator BloodFunction()
       {
            //Debug.Log("in blood function");
            isbloody = true;
            bloodImage.enabled = true;
            yield return new WaitForSeconds(1);
            bloodImage.enabled = false;
            isbloody = false;
        }


        ////////////////////////////////////////////////////////////////////////
        //  BELOW ARE SCRIPTS THAT CAME WITH THE DEFAULT CONTROLLER           //
        //    FUNCTIONS DEFINED ABOVE WERE MADE BY US                         //
        ////////////////////////////////////////////////////////////////////////


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();

            if (GameObject.Find("FPSController").transform.position.y < -1f)
            {
                FindObjectOfType<GameManager>().ResetPlayer();
            }
        }

        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0 && canattack && !isbloody)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
          if (Time.timeScale > 0)
          {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
          }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
