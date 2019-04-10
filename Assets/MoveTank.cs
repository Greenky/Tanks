using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTank : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _misslesNumber = 20;
    [SerializeField] private GameObject canon;
    [SerializeField] private GameObject misleEffect;
    [SerializeField] private GameObject gunEffect;
    [SerializeField] private AudioSource missleAudio;
    [SerializeField] private AudioSource gunleAudio;
    [SerializeField] private int boostValue;
    public AudioClip impact;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        boostValue = 1000;
        Cursor.visible = false;
        _rb = GetComponent<Rigidbody>();
        missleAudio.GetComponent<AudioSource>();
    }
    
    void Update()
    {
        Vector3 mouseRot = new Vector3(0, Input.GetAxis("Mouse X") * 2, 0);

        if (Input.GetKey(KeyCode.LeftShift) && boostValue > 0)
        {
            boostValue -= 2;
            _speed = 20;
        }
        else
        {
            boostValue++;
            _speed = 10;
        }

        canon.transform.localEulerAngles += mouseRot;
        Vector3 translateVector = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0) * 
        new Vector3(Input.GetAxis("Vertical"), 0, 0) * _speed * Time.deltaTime;
        transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal") * 2) ;
        _rb.MovePosition(_rb.position + translateVector);

        float h = Screen.currentResolution.height;
        float w = Screen.currentResolution.width;
        if (Input.GetMouseButtonDown(0) && _misslesNumber > 0)
        {
            missleAudio.PlayOneShot(impact, 1);
            if (missleAudio.isPlaying)
                Debug.Log("AAAAAA");
            else
            {
                Debug.Log("NE AAAAAA");
            }
            _misslesNumber--;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(w / 2, h / 2));
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 120))
            {
                Instantiate(misleEffect, hit.point, Quaternion.identity);
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(w / 2, h / 2));
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 90))
            {
                Instantiate(gunEffect, hit.point, Quaternion.identity);
            }

            gunleAudio.Play();
        }
    }
}
