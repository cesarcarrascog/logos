using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StudentBehaviour : MonoBehaviour
{
    float averageWaitTime;
    float interestedWaitTime;
    float science;
    float technology;  // Corrected spelling

    int university;
    public Texture[] uniTextures;
    public GameObject tarjetita;

    int lastVisited = -1;

    NavMeshAgent agent;

    StandObject desirablestand = null;

    public GameObject body;

    void Start()
    {
        averageWaitTime = Random.Range(15f, 35f);
        interestedWaitTime = averageWaitTime + Random.Range(10f, 20f);

        science = Random.Range(0f, 1f);
        technology = Random.Range(0f, 1f);

        agent = GetComponent<NavMeshAgent>();

        body.GetComponent<Renderer>().materials[0].color = new Color(Random.Range(0f, 1f), // Red
                                      Random.Range(0f, 1f), // Green
                                      Random.Range(0f, 1f), // Blue
                                      1f);

        if (uniTextures != null && uniTextures.Length > 0)
        {
            university = Random.Range(0, uniTextures.Length);
            Material uniMat = tarjetita.GetComponent<Renderer>().material;
            uniMat.SetTexture("_BaseMap", uniTextures[university]);
        }
        StartCoroutine(GotoLocation()); // Start coroutine here
    }

    void Update()
    {
        if(!desirablestand)
            return;
        Vector3 direction = desirablestand.transform.position - transform.position;
        direction.y = 0; // Keep the y-component unchanged

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;

    }

    IEnumerator GotoLocation()
    {
        
            desirablestand = null;
            while (!desirablestand)
            {
                if(GameManager.instance == null)
                    yield return null;
                for (int i = 0; i < GameManager.instance.stands.Length; i++)
                {
                    if(i == lastVisited){
                        continue;
                    }
                    StandObject stand = GameManager.instance.stands[i];
                    if (isInteresting(stand))
                    {
                        desirablestand = stand;
                        lastVisited = i;
                        break;
                    }
                }
            }

            if (desirablestand != null)
            {
                Transform trns = desirablestand.userLoc.transform;
                trns.localPosition = new Vector3(trns.localPosition.x, trns.localPosition.y, trns.localPosition.z+Random.Range(-1.0f, 1.0f));
                agent.SetDestination(trns.position);
                yield return new WaitUntil(() => !agent.pathPending || agent.remainingDistance < .5f);  // Wait until agent reaches the destination

                yield return new WaitForSeconds(CalculateStayDuration(desirablestand));  // Stay at the stand

                yield return StartCoroutine(GotoLocation());
            }
            else
            {
                yield return null;  // Yield control back to Unity and continue next frame
            }
    }

    bool isInteresting(StandObject stand)
    {
        float matchScore = (Mathf.Abs(stand.stand.science - science) +
                            Mathf.Abs(stand.stand.technology - technology)) / 2.0f;

        matchScore = 1 - matchScore;
        return Random.Range(0.0f, 1.0f) < matchScore;
    }

    public float CalculateStayDuration(StandObject stand)
    {
        float matchScore = (Mathf.Abs(stand.stand.science - science) +
                            Mathf.Abs(stand.stand.technology - technology)) / 2.0f;

        return Mathf.Lerp(averageWaitTime, interestedWaitTime, 1 - matchScore);
    }
}
