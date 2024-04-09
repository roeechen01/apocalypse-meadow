using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AiEntity : Entity
{
    protected Transform target;
    protected Player player;
   

    virtual protected void SetTarget()
    {
        Vector3 oldpos = target.position;
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] allGameObjects = scene.GetRootGameObjects();
        List<Entity> allies = new List<Entity>();
        for (int i = 0; i < allGameObjects.Length; i++)
        {
            Entity entity = allGameObjects[i].GetComponent<Entity>();
            

            if (entity && goodEntity != entity.goodEntity)
                allies.Add(entity); 

        }

        float minDistance;
        Transform minTransform;
        
      
            minDistance = Vector2.Distance(transform.position, allies[0].transform.position);
            minTransform = allies[0].transform;
          
        

        for (int i = 0; i < allies.Count; i++)
        {
            Entity ally = allies[i];
            float distance = Vector2.Distance(transform.position, ally.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                minTransform = allies[i].transform;
            }
        }

        if (/*Random.Range(1, 4) != 3*/true)
        {
            target = minTransform;
        }

        if (oldpos != target.position)
            TargetChanged();
        Invoke("SetTarget", Random.Range(4.2f, 8.4f));

    }

    virtual protected void SetTarget(Transform newTarget)
    {
        this.target = newTarget;
    }

    protected float DistanceFromTarget(Vector3 position = default)
    {
        if(position != default)
            return Vector2.Distance(transform.position, position);
        else return Vector2.Distance(transform.position, target.position);
    }

    protected bool CloseEnough(float x1, float x2, float closeValue)
    {
        return Mathf.Abs(x1 - x2) < closeValue;
    }

    virtual protected void TargetChanged()
    {

    }




}
