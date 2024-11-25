
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private float _velocityZ,_velocityX,_acc,_maxRun,_maxWalk,_maxVelocity; // float
    private bool _forward,_left,_right,_run,_hasGun,_aim,_jump,_crouch,_doubleJump,_backWard,_flashlightButton,_coverDetected,_inCover; // bool
    [SerializeField] private GameObject[] gun,flashlights; // game objects arrays
    private RigBuilder _rigBuilder; 
    private Rigidbody _rb; // rb
    private GameObject _target,_ladderObject; // gameObject
    private Transform _coverTransform,_coverSide;
    private Camera _mainCamera; // camera
    public bool ladderEnd;
    public bool ladder, ladderStart;
    private BoxCollider _boxCollider;
    private CapsuleCollider _capsuleCollider;
    private static readonly int Crouch = Animator.StringToHash("crouch");
    private static readonly int Aim = Animator.StringToHash("aim");
    private static readonly int HasGun = Animator.StringToHash("hasGun");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Ladder = Animator.StringToHash("ladder");
    private static readonly int Property1 = Animator.StringToHash("velocity x");
    private static readonly int Property2 = Animator.StringToHash("velocity z");
    [SerializeField] private LayerMask _layerMask;
    private GameObject _lastHitObject; // Önceki çarpılan nesne
    private Color _originalColor;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigBuilder = GetComponent<RigBuilder>();
        _rb = GetComponent<Rigidbody>();
        _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _target = GameObject.FindWithTag("target");
        _maxWalk = 0.7f;
        _maxRun = 2f;
        _acc = 2f;
        _boxCollider = GetComponent<BoxCollider>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }
    
    void Update()
    {
        GetInputsFunction();
        CheckGunStatus();
        JumpFunction();
        CheckCrouch();
        CheckFlashlight();
        CheckCover();
    }

    
    private void FixedUpdate()
    {
        animation_movement();
        CheckCross();
    }

    private void CheckCover()
    {
        if (!_coverDetected) return;
        if (_coverTransform.childCount <= 0) return;
        Transform nearestChild = null;
        var shortestDistance = Mathf.Infinity;
        Vector3 triggerCenter = transform.position;
        foreach (Transform child in _coverTransform)
        {
            var distance = Vector3.Distance(triggerCenter, child.position);

            if (!(distance < shortestDistance)) continue;
            shortestDistance = distance;
            nearestChild = child;
        }
        if (nearestChild != null)
        {
            _coverSide = nearestChild;
        }
    }

    private void CheckCross()
    {
        if (_hasGun && _aim)
        {
            _target.SetActive(true);
            Cross();
            if (_lastHitObject != null)
            {
                _lastHitObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        else
        {
            _target.SetActive(false);
            ResetLastObjectColor();
        }
    }

    private void CheckFlashlight()
    {
        if (!_flashlightButton) return;
        foreach (var flashlight in flashlights)
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }
    }
    
    private void ChangeLadderStatus(bool status)
    {
        ladderStart = !status;
        _rb.useGravity = status;
        _capsuleCollider.enabled = status;
        _boxCollider.enabled = !status;
    }
    

    private void ChangeGunPartsStatus()
    {
        foreach (GameObject gunParts in gun) // to active or de_active gun_aim objects
        {
            gunParts.SetActive(_hasGun);
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckLadderStatus()
    {
        if (ladderStart)
        {
            ChangeLadderStatus(true);
            //relase
        }
        else
        {
            ChangeLadderStatus(false);
            //grab
            var transform1 = transform;
            transform1.eulerAngles = new Vector3(0,-90,0);
            var position = _ladderObject.transform.position;
            transform1.position = new Vector3(position.x + 0.5f, transform1.position.y, position.z);
            _aim = false;
            _hasGun = false;
           ChangeGunPartsStatus();

            if (ladderEnd == false)
            {
                transform.position = _ladderObject.transform.Find("ladderStartDown").transform.position;
            }
        }
    }
    
    private void GetInputsFunction()
    {
        if (Input.GetKeyDown(KeyCode.E)&& _coverDetected)
        {
                transform.position = _coverSide.position;
                if (!_hasGun)
                {
                    GunStatusChange();
                }

                if (!_crouch)
                {
                    ChangeCrouchStatus();
                }
        }
        
        if (_hasGun) {
            _aim = Input.GetMouseButton(1); // if you carry weapon, you can aim
        }
        
        if (ladder)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CheckLadderStatus();
            }
        }

        _flashlightButton = Input.GetKeyDown(KeyCode.F);
        _forward = Input.GetKey(KeyCode.W);
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _backWard = Input.GetKey(KeyCode.S);

        if (_backWard) // if you are going backward your controls are reversed
        {
            _right = Input.GetKey(KeyCode.A);
            _left = Input.GetKey(KeyCode.D);
        } else {
            _left = Input.GetKey(KeyCode.A);
            _right = Input.GetKey(KeyCode.D);
        }

        if (_crouch == false && _jump == false && _hasGun == false&& ladderStart == false) // run 
        {
            _run = Input.GetKey(KeyCode.LeftShift);
        } else {
            _run = false;
        }
    }

    private void CheckCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C)&& _jump == false&& ladderStart == false) // crouch 
        {
            ChangeCrouchStatus();
        }
    }
    private void ChangeCrouchStatus()
    {
        _crouch = _crouch == false; // if you crouch and press c, stand else start crouching
    }
    private void CheckGunStatus()
    {
        _rigBuilder.enabled = _aim; // if you aim, enable rigBuilder
        if (Input.GetKeyDown(KeyCode.R)&& _aim== false && _jump == false&& ladderStart == false)
        {
           GunStatusChange();
        }
    }

    private void GunStatusChange()
    {
        if (!_hasGun)
        {
            _hasGun = true;
        }
        else
        {
            _hasGun = false;
        }
        ChangeGunPartsStatus();
    }

    private void JumpFunction()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_hasGun && !_crouch && !_doubleJump && ladderStart == false)
        {
            if (_jump)
            {
                _doubleJump = true;
                _rb.AddForce(Vector3.up * 200); // second jump
            }
            else
            {
                _jump = true;
                StartCoroutine(AddJumpForceAfterDelay());
            }
        }
    }

    IEnumerator AddJumpForceAfterDelay()
    {
        yield return new WaitForSeconds(0.18f);
        AddJumpForce();
    }

    private void AddJumpForce()
    {
        _rb.AddForce(Vector3.up * 300);
    }
    private void animation_movement()
    {
        
        _maxWalk = _hasGun ? 0.5f : 0.7f;
        _maxVelocity = _run ? _maxRun : _maxWalk;
        
            if (_forward && _velocityZ < _maxVelocity) // pressed w and speed is lower then max
            {
                _velocityZ += Time.deltaTime * _acc;
            }
        
            if (_velocityZ > _maxVelocity) 
            {
                _velocityZ -= Time.deltaTime * _acc; // to reduce
            }
        
            if (_forward == false && _backWard && _velocityZ > -0.5f) // if going backward and not going forward until abs(-0.5) velocity
            {
                _velocityZ -= Time.deltaTime * _acc;
            }
        
            if (!_forward && !_backWard)
            {
                _velocityZ += (_velocityZ > 0 ? -1f : 1f) * Time.deltaTime * _acc;
            }
            if (_backWard == false && _forward == false && _velocityZ>-0.1f && _velocityZ< 0.1f) 
            {// prob. its not important but be safe is better
                _velocityZ = 0;
            }
        
            if (_right && _velocityX<0.5f && -0.5f<_velocityX)
            {
                _velocityX += Time.deltaTime * _acc;
            } // make turn velocity positive
        
            if (_left && _velocityX<0.5f && -0.5f<_velocityX)
            {
                _velocityX -= Time.deltaTime * _acc;
            } // make turn velocity negative

            if (!_right && !_left)
            {
                if (Mathf.Abs(_velocityX) < 0.1f)
                {
                    _velocityX = 0;
                }
                else
                {
                    _velocityX += (_velocityX > 0f ? -1f : 1f) * Time.deltaTime * _acc;
                }
            }
            else
            {
                if (0.5 < _velocityX && !_right)
                {
                    _velocityX = 0.49f;
                }
                if (-0.5 > _velocityX && !_left)
                {
                    _velocityX = -0.49f;
                }
            }

            if (ladderStart==false)
            {
                float speedValue;
                if (_aim)
                {
                    Vector3 targetPosition = _target.transform.position;
                    targetPosition.y = transform.position.y;
                    transform.LookAt(targetPosition);
                    speedValue = 0.03f;
                }
                else
                {
                    var rotationInput = _velocityX * 400 * Time.deltaTime;
                    transform.Rotate(0, rotationInput, 0);
                    speedValue = 0.05f;
                }

                var transform1 = transform;
                var movement = transform1.forward * _velocityZ;
                _rb.MovePosition(transform1.position + movement*speedValue);
            }
            else
            {
                var movement = new Vector3(0, _velocityZ, 0);
                _rb.MovePosition(transform.position + movement * 0.05f);
                if (_velocityZ < 0 && ladderEnd)
                {
                    ladderStart = false;
                    _rb.useGravity = true;
                    _capsuleCollider.enabled = true;
                    _boxCollider.enabled = false;
                }
            }

            SendVarToAnimator();
    }
    
    private void SendVarToAnimator() 
    {
        _animator.SetBool(Crouch,_crouch);
        _animator.SetBool(Aim,_aim);
        _animator.SetFloat(Property1,_velocityX);
        _animator.SetFloat(Property2,_velocityZ);
        _animator.SetBool(HasGun,_hasGun);
        _animator.SetBool(Jump,_jump);
        _animator.SetBool(Ladder,ladderStart);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            StartCoroutine(WaitAfterTouchedGround());
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ladder"))
        {
            ladder = true;
            _ladderObject = other.gameObject;
        }
        if (other.gameObject.CompareTag("ladderEnd")&& ladderStart)
        {
            ladderStart = false;
            StartCoroutine(WaitBeforeSetGravityTrue());
            _capsuleCollider.enabled = true;
            _boxCollider.enabled = false;
            transform.position = other.gameObject.transform.position;
        }

        if (other.gameObject.CompareTag("ladderDown"))
        {
            ladderEnd = true;
        }

        if (!other.gameObject.CompareTag("cover")) return;
        _coverDetected = true;
        _coverTransform = other.transform;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ladder"))
        {
            ladder = false;
            ladderStart = false;
            StartCoroutine(WaitBeforeSetGravityTrue());
        }
        if (other.gameObject.CompareTag("ladderDown"))
        {
            ladderEnd = false;
        }

        if (other.gameObject.CompareTag("cover"))
        {
            _coverDetected = false;
        }
    }

    IEnumerator WaitBeforeSetGravityTrue()
    {
        yield return new WaitForSeconds(0.1f);
        _rb.useGravity = true;
    }

    IEnumerator WaitAfterTouchedGround()
    {
        yield return new WaitForSeconds(0.1f);
        _jump = false;
        _doubleJump = false;
    }

    private void Cross()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _layerMask))
        {
           
            GameObject hitObject = raycastHit.collider.gameObject;

            
            if (_lastHitObject != hitObject)
            {
                ResetLastObjectColor(); 

            
                _lastHitObject = hitObject;
                Renderer renderer = hitObject.GetComponent<Renderer>();

                if (renderer != null)
                {
                    _originalColor = renderer.material.color; 
                    renderer.material.color = Color.red; 
                }
            }

            
            _target.transform.position = new Vector3(
                raycastHit.collider.gameObject.transform.position.x,
                raycastHit.point.y,
                raycastHit.point.z);
        }
        else
        {
           
            if (_lastHitObject != null)
            {
               
            }
        }
    }
    
    private void ResetLastObjectColor()
    {
        if (_lastHitObject != null)
        {
            Renderer renderer = _lastHitObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = _originalColor; // Eski rengi geri yükle
            }
            
        }
    }
}
