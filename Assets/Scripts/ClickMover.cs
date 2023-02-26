using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickMover : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wayPointPrefab;
    public GameObject pointer;
    NavMeshAgent agent;
    private Interactable prevInteractable;
    public Animator anim;
    public Animator cameraAnim;
    public GameObject cutDialog;
    public GameObject[] responses;
    public int cuts = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Interactable>() == prevInteractable)
        {
            Debug.Log("Mine?");
            cutDialog.SetActive(true);
        }
    }

    public void Mine()
    {
        StartCoroutine(MiningRoutine(3f));
        cutDialog.SetActive(false);
    }

    public IEnumerator MiningRoutine(float delay)
    {
        anim.Play("mining");
        yield return new WaitForSeconds(delay);
        Destroy(prevInteractable.gameObject);
        anim.Play("idle");
        yield return new WaitForSeconds(1f);
        cameraAnim.Play("earthquake");
        yield return new WaitForSeconds(2f);
        responses[cuts].SetActive(true);
        cuts++;
    }



    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Interactable interactable;
            if (hit.transform.gameObject.GetComponentInParent<Interactable>())
            {
                interactable = hit.transform.gameObject.GetComponentInParent<Interactable>();
                if (interactable != prevInteractable)
                {
                    if (prevInteractable != null)
                    prevInteractable.DisableHighlight();
                }
                interactable.EnableHighlight();
                prevInteractable = interactable;
            } else
            {
                if (prevInteractable != null)
                prevInteractable.DisableHighlight();
            }
            if (hit.transform.gameObject.tag == "Navigation")
                pointer.transform.position = hit.point;
        } else
        {

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (cutDialog.activeInHierarchy)
            {
                cutDialog.SetActive(false);
            } foreach (GameObject r in responses)
            {
                r.SetActive(false);
            }
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.gameObject.tag == "Navigation")
                {
                    GameObject wp = Instantiate(wayPointPrefab, null);
                    wp.transform.position = hit.point;
                    agent.destination = hit.point;
                }
                else
                {
                    Interactable interactable;
                    if (hit.transform.gameObject.GetComponentInParent<Interactable>())
                    {
                        interactable = hit.transform.gameObject.GetComponentInParent<Interactable>();
                        agent.destination = interactable.interactPos.position;
                    }
                }
            }
        }
        
        
           
        
    }
}
