using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	[SerializeField] private Text ammo;
	[SerializeField] private Image cross;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        boostValue = 500;
        Cursor.visible = false;
        _rb = GetComponent<Rigidbody>();
        missleAudio.GetComponent<AudioSource>();
    }
    
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
		    Application.Quit();
	    if (Input.GetKeyDown(KeyCode.R))
		    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    cross.color = Color.Lerp(cross.color, Color.white, 0.1f);
	    ammo.text = "X" + _misslesNumber;
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

        float h = Screen.height;
        float w = Screen.width;
        if (Input.GetMouseButtonDown(0) && _misslesNumber > 0)
        {
			missleAudio.Play();
			_misslesNumber--;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(w / 2, h / 2));
            RaycastHit hit;
			GameObject missle;
	        if (Physics.Raycast(canon.transform.position,
		        Quaternion.Euler(canon.transform.eulerAngles) * Vector3.forward, out hit, 120) 
	            && hit.transform.CompareTag("Tank"))
	        {
		        cross.color = Color.red;
		        hit.transform.GetComponent<HPcontroller>().Hit(10, gameObject);
		        missle = Instantiate(misleEffect, hit.point, Quaternion.identity);
		        StartCoroutine(Destroyer(missle));
	        }
            else if (Physics.Raycast(ray, out hit, 120))
            {
	            if (hit.transform.tag == "Tank")
	            {
		            cross.color = Color.red;
		            hit.transform.GetComponent<HPcontroller>().Hit(10, gameObject);
	            }
	            missle = Instantiate(misleEffect, hit.point, Quaternion.identity);
				StartCoroutine(Destroyer(missle));
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
			gunleAudio.Play();
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(w /2, h /2));
			RaycastHit hit;
			GameObject missle;
	        if (Physics.Raycast(canon.transform.position,
		            Quaternion.Euler(canon.transform.eulerAngles) * Vector3.forward, out hit, 90) 
	            && hit.transform.CompareTag("Tank"))
	        {
		        cross.color = Color.red;
		        hit.transform.GetComponent<HPcontroller>().Hit(5, gameObject);
		        missle = Instantiate(misleEffect, hit.point, Quaternion.identity);
		        StartCoroutine(Destroyer(missle));
	        }
			else if (Physics.Raycast(ray, out hit, 90))
            {
	            if (hit.transform.tag == "Tank")
	            {
		            cross.color = Color.red;
		            hit.transform.GetComponent<HPcontroller>().Hit(5, gameObject);
	            }

	            missle = Instantiate(gunEffect, hit.point, Quaternion.identity);
				StartCoroutine(Destroyer(missle));
			}
        }
    }

	private IEnumerator Destroyer(GameObject missle)
	{
		yield return new WaitForSeconds(1);
		Destroy(missle);
	}
}
