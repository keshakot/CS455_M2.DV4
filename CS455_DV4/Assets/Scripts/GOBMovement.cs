using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal {
    public string name;
    public float value;

    public Goal( string n, float v ){
        name = n;
        value = v;
    }
}

public abstract class Action {
    public string action;
    public abstract float getGoalChange( Goal goal );
}

public class GOBMovement: Kinematic
{
    Sequence root;
    private List<Action> queue;
    public Action[] gActions;
    public Goal[] gGoals;
    public int timesteps = 0;

    private int delayFlag = 0;

    /* Implementation-specific tasks */
    class UseLoo: Action {
        public string action = "UseLoo";
        public UseLoo(){ base.action = action; }
        public override float getGoalChange( Goal goal ){
            switch (goal.name){
                case "Loo":
                    return -4f;
                case "Eat":
                    return 1f;
                case "Sleep":
                    return 1f;
                default:
                    return 0f;
            }
        }
    }

    class EatMeal: Action {
        public string action = "EatMeal";
        public EatMeal(){ base.action = action; }
        public override float getGoalChange( Goal goal ){
            switch (goal.name){
                case "Loo":
                    return 2f;
                case "Eat":
                    return -3f;
                case "Sleep":
                    return 1f;
                default:
                    return 0f;
            }
        }
    }

    class EatSnack: Action {
        public string action = "EatSnack";
        public EatSnack(){ base.action = action; }
        public override float getGoalChange( Goal goal ){
            switch (goal.name){
                case "Loo":
                    return 1f;
                case "Eat":
                    return -2f;
                case "Sleep":
                    return 1f;
                default:
                    return 0f;
            }
        }
    }

    class Sleep: Action {
        public string action = "Sleep";
        public Sleep(){ base.action = action; }
        public override float getGoalChange( Goal goal ){
            switch (goal.name){
                case "Loo":
                    return 1f;
                case "Eat":
                    return 2f;
                case "Sleep":
                    return -4f;
                default:
                    return 0f;
            }
        }
    }
 
    /* ----------------------------- */

    Action chooseActionSimple( Action[] actions, Goal[] goals ){
        Goal topGoal = goals[0];

        // Find the most 'valuable' goal
        for(int i=1; i<goals.Length; i++)
            if( goals[i].value > topGoal.value )
                topGoal = goals[i];

        // Find the best action to take
        Action bestAction = actions[0];
        float bestUtility = -actions[0].getGoalChange( topGoal );

        for(int i=1; i<actions.Length; i++){
            float utility = -actions[i].getGoalChange( topGoal );
            if( utility > bestUtility ){
                bestUtility = utility;
                bestAction = actions[i];
            }
        }

        return bestAction;
    }

    float discontentment( Action action, Goal[] goals ){
        float discontentment = 0;

        foreach( Goal goal in goals){
            float i = goal.value + action.getGoalChange( goal );
            discontentment += i*i;
        }

        return discontentment;
    }

    Action chooseActionUtility( Action[] actions, Goal[] goals ){
        Goal topGoal = goals[0];

        // Find the most 'valuable' goal
        for(int i=1; i<goals.Length; i++)
            if( goals[i].value > topGoal.value )
                topGoal = goals[i];

        // Find the action leading to the lowest discontentment
        Action bestAction = actions[0];
        float bestDsc = discontentment( bestAction, goals );

        for(int i=1; i<actions.Length; i++){
            float dsc = discontentment( actions[i], goals );
            if( dsc < bestDsc ){
                bestDsc = dsc;
                bestAction = actions[i];
            }
        }

        return bestAction;
    }

    void printWeights(){
        if( gGoals == null ) return; 
        
        string txt = "[T=" + timesteps + "] Weights: ";

        foreach( Goal g in gGoals )
            txt += string.Format(" [{0}:{1}]", g.name, g.value);

        Debug.Log( txt );
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1);

        // Define goals and actions
        gActions = new Action[]{};
        gGoals = new Goal[]{ new Goal("Eat", 4), new Goal("Sleep", 3), new Goal("Loo", 5) };
        gActions = new Action[]{ new UseLoo(), new EatMeal(), new EatSnack(), new Sleep() };

        queue = new List<Action>();
    }

    // Update is called once per frame
    //protected override void Update()
    void FixedUpdate()
    {
        if( queue == null ) return; 
        else if( delayFlag > 0 ){ delayFlag--; return; }

        // If no action to perform, choose action //
        if( queue.Count == 0 ){
            this.printWeights();
            Action a = this.chooseActionUtility( this.gActions, this.gGoals );
            queue.Add( a );
            Debug.Log("[T=" + timesteps++ + "] Performing action: " + a.action);
        }
        // If an action exists in the queue, perform that action //
        else {
            foreach( Goal g in gGoals ) {
                g.value += queue[0].getGoalChange( g );
                g.value = Mathf.Max( g.value, 0f ); // to avoid negative weights
            }
            delayFlag = 50;
            queue.RemoveAt(0);
        }
    }

    IEnumerator delay( float s ){
        yield return new WaitForSeconds( s );
        delayFlag = 0;
    }
}
