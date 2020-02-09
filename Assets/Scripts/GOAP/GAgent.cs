using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SubGoal 
{
    // Dictionary to store our goals
    // subGoal can contaim multiple goals
    // value presents that how it is important(like weight) 
    public Dictionary<string, int> sGoals;
    // Bool to store if goal should be removed after it has been achieved
    public bool remove;

    public SubGoal(string s, int i, bool r) 
    {
        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    public GAction currentAction;
    private SubGoal currentGoal;
    private GPlanner planner;
    Queue<GAction> actionQueue;

    public virtual void Start() 
    {
        //gets all actions attached from editor for this agent
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);
    }

    bool invoked = false;
    //an invoked method to allow an agent to be performing a task
    //for a set location
    void CompleteAction() 
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    private void LateUpdate()
    {
        //if there's a current action and it is still running
        if (currentAction != null && currentAction.running) 
        {
            if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 2f)
            {
                if (!invoked) 
                {
                    //if the action movement is complete wait
                    //a certain duration for it to be completed
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }


        // Check we have a planner and an actionQueue
        if (planner == null || actionQueue == null) 
        {
            planner = new GPlanner();

            // Sort the goals in descending order and store them in sortedGoals
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            //look through each goal to find one that has an achievable plan
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals) {
                actionQueue = planner.Plan(actions, sg.Key.sGoals, null);
                // If actionQueue is not = null then we must have a plan
                if (actionQueue != null) {
                    // Set the current goal
                    currentGoal = sg.Key;
                    break;
                }
            }
        }
    

        // Have we an actionQueue
        if (actionQueue != null && actionQueue.Count == 0) 
        {
            // Check if currentGoal is removable
            if (currentGoal.remove) {
                // Remove it
                goals.Remove(currentGoal);
            }
            // Set planner = null so it will trigger a new one
            planner = null;
        }
    
        // Do we still have actions
        if (actionQueue != null && actionQueue.Count > 0) 
        {
            // Remove the top action of the queue and put it in currentAction
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform()) 
            {
                // Get our current target object
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    // Activate the current action
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }
                    
                if (currentAction.target != null) 
                {
                    // Activate the current action
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            } 
            else 
            {
                // Force a new plan
                actionQueue = null;
            }

        }
    }
}
