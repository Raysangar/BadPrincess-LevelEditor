using UnityEngine;
using System.Collections;

public class CEnemyDetectionManager : MonoBehaviour {

    public float detection_range = 20;
    public int detector_collision_group = 12;

    public bool moving_detection = false;
    public bool is_aggressive = false;
    public bool can_defend_position = true;
    public bool can_keep_attacking = true;
    public float defending_distance = 40;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
