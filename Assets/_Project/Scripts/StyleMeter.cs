using UnityEngine;

//there should be a StyleMeter game object that this script is attached to

//make in Unity based off playtesting
[System.Serializable]  
public struct StyleRanking
{
    public string name; //current level
    public float points; //maximum points to be at this level
    public int multiplier; //score multiplier
}
public class StyleMeter : MonoBehaviour
{
    //variables, most to be set through playtesting
    [SerializeField] private StyleRanking[] levels; //stores all the style rankings we make in Unity
    [SerializeField] private int curLevel; //which level are we currently on
    [SerializeField] private float stylePoints; //how many points do we have right now
    [SerializeField] private float styleFactor; //what ratio of normal points should contribute to the style score?
    [SerializeField] private float resetTime; //total time on each level
    [SerializeField] private float drainRate; //rate that the style points decrease at -> do we need this?
    [SerializeField] private float time; //current time 
    //ui variables -> not sure yet
    void Start()
    {   
        //start with nothing
        stylePoints = 0;
        curLevel = 0;  
        time = resetTime; 
    }

    void Update()
    {
        //option 1: the player has a fixed amount of time before their score resets
        //decrease the timer
        time = Mathf.Max(time - Time.deltaTime, 0f);
        //once the player runs out of style points, reset
        if (time <= 0) {
            curLevel = 0;
            stylePoints = 0;
        }
        
        //option 2: the style points drain at a constant rate until it hits zero
        stylePoints = Mathf.Max(stylePoints - drainRate, 0f);
        for (int i = curLevel; i >= 0; i--) {
            if (stylePoints < levels[i].points) {
                curLevel = i;
                break;
            }
        }
        //once the player runs out of style points, reset
        if (stylePoints <= 0) {
            curLevel = 0;
            stylePoints = 0;
        }

        //somehow show the current styles
        UpdateUI();
    }

    //whatever script controls the user's overall score should call this before updating
    //and add this number to the total score
    public int ReceiveHit(int hitScore) {
        //calculate the score with the style bonus
        int score = hitScore * levels[curLevel].multiplier;
        //update the amount of style points the player has
        stylePoints += score / styleFactor;
        //upgrade them to the next level if they're over the maximum style points
        if (stylePoints > levels[curLevel].points) {
            curLevel++;
        }
        //reset the timer
        time = resetTime;
        //return the score with style bonus
        return score;
    }

    //not sure how to do this yet
    void UpdateUI() {
        return;
    }
}
