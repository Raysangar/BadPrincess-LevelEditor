using UnityEngine;
using System.Collections;

public class CPrincessManager : MonoBehaviour {

    public float aura_texture_width = 5;
    public float aura_texture_height = 5;
    public float aura_projection_height = 1;

    public int wrath_per_casualty = 10;
    public int wrath_per_damage = 2;
    public int wrath_per_attack = 3;

    public float time_to_decrease = 1000;
    public float time_to_awake = 5000;

    public float aoe_stun_time = 6000;
    public float aoe_stun_distance = 20;
    public int aoe_stun_cost = 20;
    public string aoe_stun_particle = "";

    public float aoe_buff_percentage = 20;
    public int aoe_buff_cost = 40;
    public int aoe_buff_duration = 5;
    public string aoe_buff_particle = "";

    public float aoe_heal_quantity = 50;
    public int aoe_heal_cost = 60;
    public string aoe_heal_particle = "";

    public int berserkerModeDuration = 6000;
    public float berserkerModeAttackModifier = 50;
    public int berserkerModeCost = 80;
    public string berserker_particle = "";

    public string aura_material = "";

    public int limitTimeOnDangerousWrathLevel;
    public int wrathOverflowDamage;
    public string wrathOverflowParticleName;
    public string wrathOverflowSoundName;

    public bool wrathOverflowActive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
