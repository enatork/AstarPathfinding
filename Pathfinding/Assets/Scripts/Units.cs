using UnityEngine;
using System.Collections;

public class Units : MonoBehaviour
{

    public Transform target;
    float speed = 20;
    Vector3[] path;
    int targetIndex;
    Vector3 targetOldPosition;
    float timer = 0;

    void Start()
    {
        CalculatePath();
    }

    void Update()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 3)
        {
            timer = 0;
            CalculatePath();
            
        }
    }

    void CalculatePath()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        targetOldPosition = target.position;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path != null && path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {

                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
