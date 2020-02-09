using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{

    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0.0f;
    public NavMeshAgent agent;

    public WorldState[] preConditions;
    public WorldState[] afterEffects;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;
    // State of the agent
    public WorldStates beliefs;
    // Are we currently performing an action?
    public bool running = false;

    // Constructor
    public GAction()
    {
        // Set up the preconditions and effects
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    private void Awake()
    {

        // Get hold of the agents NavMeshAgent
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        // Check if there are any preConditions in the Inspector
        // and add to the dictionary
        if (preConditions != null)
        {

            foreach (WorldState w in preConditions)
            {

                // Add each item to our Dictionary
                preconditions.Add(w.key, w.value);
            }
        }

        // Check if there are any afterEffects in the Inspector
        // and add to the dictionary
        if (afterEffects != null)
        {

            foreach (WorldState w in afterEffects)
            {

                // Add each item to our Dictionary
                effects.Add(w.key, w.value);
            }
        }
    }

    public bool IsAchievable()
    {

        return true;
    }

    //check if the action is achievable given the condition of the
    //world and trying to match with the actions preconditions
    public bool IsAhievableGiven(Dictionary<string, int> conditions)
    {

        foreach (KeyValuePair<string, int> p in preconditions)
        {

            if (!conditions.ContainsKey(p.Key))
            {

                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
