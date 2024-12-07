
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private float _velocityZ,_velocityX,_acc,_maxRun,_maxWalk,_maxVelocity; // float
    private bool _forward,_left,_right,_run,_hasGun,_aim,_jump,_crouch,_doubleJump,_backWard,_flashlightButton,_coverDetected,_inCover,_nearEnemy,_knifeTime,_grenade,_reload; // bool
    [SerializeField] private GameObject[] flashlights; // game objects arrays
    private bool _grenadeAim;
    private RigBuilder _rigBuilder; 
    private Rigidbody _rb; // rb
    private GameObject _target,_ladderObject,_crossHair,_magazine; // gameObject
    private Transform _coverTransform,_coverSide,_nearEnemyTransform;
    public GameObject currentGun;
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
    [SerializeField] private LayerMask layerMask;
    private GameObject _lastHitObject; // Önceki çarpılan nesne
    private Color _originalColor;
    [SerializeField]private GameObject[] weapons,weaponsV,weaponsGrenade;
    [SerializeField] private GameObject secondHand;
    public GameObject childMagazine;
    private static readonly int Knife1 = Animator.StringToHash("knife");
    private static readonly int Grenade1 = Animator.StringToHash("grenade");
    private static readonly int Reload = Animator.StringToHash("Reload");
    private int _currentGunIndex;
    public GameObject tutor;
    private static readonly int StopReload1 = Animator.StringToHash("StopReload");
    [SerializeField] private GrenadeThrow _grenadeThrow;

    
    public bool GetCrouch()
    {
        return _crouch;
    }
    public bool GetGranade()
    {
        return _grenade;
    }
    
    void Start()
    {
        currentGun = weapons[0];
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
        _crossHair = GameObject.FindWithTag("CrossHair");
    }
    
    void Update()
    {
        GetInputsFunction();
        CheckGunStatus();
        JumpFunction();
        CheckCrouch();
        CheckFlashlight();
        CheckCover();
        ChangeWeapons();
    }
    private void FixedUpdate()
    {
        if (_knifeTime == false && (_reload && _crouch) == false)
        {
            animation_movement();
        }
        CheckCross();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ChangeWeapons()
    {
        if (!_hasGun || _reload) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject w in weapons) // to active or de_active gun_aim objects
            {
                w.SetActive(false);
            }
            currentGun = weapons[0];
            weaponsV[0].SetActive(false);
            weaponsV[1].SetActive(true);
            currentGun.SetActive(true);
            secondHand.transform.localPosition = new Vector3(-0.09f, 0.3391f, -0.0388f);
            secondHand.transform.localRotation = Quaternion.Euler(0,-186.2f,0);
            currentGun.GetComponent<AudioSource>().clip = currentGun.GetComponent<Gun>()._audioClips[3];
            currentGun.GetComponent<AudioSource>().Play();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (GameObject w in weapons) // to active or de_active gun_aim objects
                {
                    w.SetActive(false);
                }
                currentGun = weapons[1];
                weaponsV[1].SetActive(false);
                weaponsV[0].SetActive(true);
                currentGun.SetActive(true);
                secondHand.transform.localPosition = new Vector3(0, 0, 0);
                secondHand.transform.localRotation = Quaternion.Euler(0,-186.2f,0);
                currentGun.GetComponent<AudioSource>().clip = currentGun.GetComponent<Gun>()._audioClips[3];
                currentGun.GetComponent<AudioSource>().Play();
            }
            else
            {
                if (!Input.GetKeyDown(KeyCode.Alpha3)) return;
                foreach (GameObject w in weapons) // to active or de_active gun_aim objects
                {
                    w.SetActive(false);
                }
                currentGun = weapons[2];
                currentGun.SetActive(true);
                secondHand.transform.localPosition = new Vector3(0.01131009f, 0.2369997f, 0.0585504f);
                secondHand.transform.localRotation = Quaternion.Euler(62.834f,-208.042f,-35.346f);
            }
        }
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
            if (child.CompareTag("coverSide"))
            {
                nearestChild = child;
            }
        }
        if (nearestChild != null)
        {
            _coverSide = nearestChild;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckCross()
    {
        if (_hasGun && _aim && _reload == false)
        {
            _target.SetActive(true);
            _crossHair.SetActive(true);
            Cross();
            if (_lastHitObject != null)
            {
                _lastHitObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        else
        {
            _target.SetActive(false);
            _crossHair.SetActive(false);
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
    

    // ReSharper disable Unity.PerformanceAnalysis
    private void ChangeGunPartsStatus()
    {
        currentGun.SetActive(_hasGun);
        if (currentGun == weapons[0])
        {
            weaponsV[0].SetActive(false);
        }
        else
        {
            weaponsV[1].SetActive(false);
        }
        currentGun.GetComponent<AudioSource>().clip = currentGun.GetComponent<Gun>()._audioClips[3];
        currentGun.GetComponent<AudioSource>().Play();

        if (_hasGun == false)
        {
            foreach (var wv in weaponsV)
            {
                wv.SetActive(true);
            }
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

    private void Knife()
    {
        //test it with jump 
        _aim = false;
        _velocityX = 0;
        _velocityZ = 0;
        _hasGun = false;
        _jump = false;
        _crouch = false;
        _crossHair.SetActive(false);
        _target.SetActive(false);
        ChangeGunPartsStatus();
        weaponsV[2].SetActive(false);
        weaponsV[0].SetActive(true);
        weaponsV[1].SetActive(true);
        weapons[3].SetActive(true);
        SendVarToAnimator();
        var transform1 = transform;
        transform1.eulerAngles = new Vector3(0,90,0);
        var position = _nearEnemyTransform.position;
        transform1.position = new Vector3(position.x - 0.5f, transform1.position.y, position.z);
        _animator.SetTrigger(Knife1);
    }
    public void StopKnife()
    {
        _knifeTime = false;
        weapons[3].SetActive(false);
        weaponsV[2].SetActive(true);
    }

    public void StartThrowGrenade()
    {
        // animation throw  end call will's code 
        Debug.Log("Start throw granade");
        _grenadeThrow.Throw();
    }

    private IEnumerator Grenade(int val)
    {
        yield return new WaitForSeconds(2.23f);
        _grenade = false;
        currentGun.SetActive(true);
        weaponsGrenade[val].SetActive(false);
    }

    private void PositionCorrector()
    {
        if (Input.GetKeyUp(KeyCode.G) && _hasGun == false)
        {
           GunStatusChange();
           if(_crouch) ChangeCrouchStatus();
        }
        
    }
    
    private void GetInputsFunction()
    {
        PositionCorrector();
        if (Input.GetKey(KeyCode.G)&& _hasGun && _reload == false && _crouch == false)
        {
            //grenade aim
            _grenadeAim = true;
        }
        else
        {
            _grenadeAim = false;
        }

        if (Input.GetKeyUp(KeyCode.G)&& _hasGun  && _reload == false && _crouch == false)
        {
            _grenade = true;
            _aim = false;
            var val = 0;
            _animator.SetTrigger(Grenade1);
            currentGun.SetActive(false);
            if (currentGun == weapons[0])
            {
                // set active 
                weaponsGrenade[0].SetActive(true);
            }
            else
            {
                if (currentGun == weapons[1])
                {
                    weaponsGrenade[1].SetActive(true);
                    val = 1;
                }
            }

            StartCoroutine(Grenade(val));
        }

        if (Input.GetKeyDown(KeyCode.E) && _grenade == false)
        {
            if (_nearEnemy && _knifeTime == false)
            {
                _knifeTime = true;
                Knife();
            }
            else
            {
                if (_coverDetected)
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
                else
                {
                    if (ladder)
                    {
                        CheckLadderStatus();
                    }
                }
            }
        }
        
        if (_hasGun) {
            if (_grenade == false && _reload == false)
            {
                _aim = Input.GetMouseButton(1);
            }
            // if you carry weapon, you can aim
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
        if (Input.GetKeyDown(KeyCode.C)&& _jump == false&& ladderStart == false && _reload == false) // crouch 
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
        if (_grenadeAim == false)
        {
            _rigBuilder.enabled = _aim;
        }
        else
        {
            _rigBuilder.enabled = false;
        } // if you aim, enable rigBuilder
        if (Input.GetKeyDown(KeyCode.R)&& _aim== false && _jump == false&& ladderStart == false && _reload == false)
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
        if (_velocityZ < 0)
        {
            _animator.SetLayerWeight(1,0.9f);
        }
        else
        {
            _animator.SetLayerWeight(1,0);
        }
        _animator.SetBool(Crouch,_crouch);
        if (_grenadeAim)
        {
            _animator.SetBool(Aim,false);
        }
        else
        {
            _animator.SetBool(Aim,_aim);
        }
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

        if (other.gameObject.CompareTag("cover"))
        {
            _coverDetected = true;
            _coverTransform = other.transform;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            _nearEnemy = true;
            _nearEnemyTransform = other.transform;
        }

        if (other.gameObject.CompareTag("checkPoint"))
        {
            Destroy(other.gameObject);
            _velocityX = 0;
            _velocityZ = 0;
            SendVarToAnimator();
            tutor.SetActive(true);
            tutor.GetComponent<DialogManager>().ContDialog();
        }


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
        if (other.gameObject.CompareTag("Enemy"))
        {
            _nearEnemy = false;
            _nearEnemyTransform = null;
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
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {

            if (raycastHit.transform.gameObject.layer == 6 || raycastHit.transform.gameObject.layer == 7)
            {
                GameObject hitObject = raycastHit.collider.gameObject;
                if (_lastHitObject != hitObject)
                {
                    ResetLastObjectColor(); 

            
                    _lastHitObject = hitObject;
                    Renderer rendererLocal = hitObject.GetComponent<Renderer>();

                    if (rendererLocal != null)
                    {
                        var material = rendererLocal.material;
                        _originalColor = material.color; 
                        material.color = Color.red; 
                    }
                }
            }

            _target.transform.position = raycastHit.point;
            if ( raycastHit.transform.gameObject.layer == 6)
            {
                var position = _target.transform.position;
                position = new Vector3(position.x,position.y,raycastHit.transform.position.z );
                _target.transform.position = position;
            }
            //code complex and not clear change it later :)
        }
    }
    public void StartReload()
    {
        _reload = true;
        _aim = false;
        _animator.SetBool(Aim,_aim);
        _animator.SetTrigger(Reload);
        _magazine = currentGun.transform.Find("Magazine").gameObject;
        GameObject child = Instantiate(_magazine, currentGun.transform, true);
        child.GetComponent<Rigidbody>().isKinematic = true;
        child.name = "Magazine";
        child.SetActive(false);
        _magazine.GetComponent<Rigidbody>().isKinematic = false;
        _magazine.GetComponent<BoxCollider>().enabled = true;
        _magazine.transform.SetParent(null);
        childMagazine = child;
    }
    
    public void StopReload()
    {
        _reload = false;
        _animator.SetTrigger(StopReload1);
        childMagazine.SetActive(true);
    }

    public bool IsReload()
    {
        return _reload;
    }
    private void ResetLastObjectColor()
    {
        if (_lastHitObject != null)
        {
            Renderer rendererLocal = _lastHitObject.GetComponent<Renderer>();

            if (rendererLocal != null)
            {
                rendererLocal.material.color = _originalColor; // Eski rengi geri yükle
            }
            
        }
        _lastHitObject = null;
    }

    public void SetAim(bool set)
    {
        _aim = set;
    }
}
