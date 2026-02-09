using UnityEngine;

public class PlayerController: MonoBehaviour
{
    private Vector3 _input;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _spd = 5;
    [SerializeField] private float _turnspd = 360;
    [SerializeField] private bool _squatPopped;

    public bool squatPopped => _squatPopped;

    private void Update()
    {
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GatherInput() {

        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // For poppin a squat
        _squatPopped = Input.GetKey(KeyCode.LeftControl);
    
    }

    void Look() {

        if (_squatPopped) { 
        
            float turnAmount = _input.x * _turnspd * Time.deltaTime;
            transform.Rotate(Vector3.up, turnAmount);
            return;
        }

        if(_input != Vector3.zero) {

            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
            var skewedInput = matrix.MultiplyPoint3x4(_input);

            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation,rot,_turnspd * Time.deltaTime);
        }


    
    }

    void Move() {

        // Dont move while squatting
        if (_squatPopped) return;

        _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _spd * Time.deltaTime);
    }

}
