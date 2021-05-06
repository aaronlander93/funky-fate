/*
Code By: Aaron Lander

This file keeps track of on-beat combos, and triggers a rhythm attack when
combos are hit.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmAttack : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject weakPrefab;
    public GameObject strongPrefab;
    public Renderer rend;
    public UnityEngine.Experimental.Rendering.Universal.Light2D light2D;

    public Material playerMat;
    public Material onBeatMat;
    public Material offBeatMat;

    public float shotDelay;
    private float shotDelayCounter;
    private Color origColor;
    private Color onBeatColor = new Color(.278f, 1.0f, .043f, 1.0f);
    private Color offBeatColor = new Color(.87f, 0f, .067f, 1.0f);

    private int consecOnBeats = 0;
    private int lastBeat = 0;
    public Animator animator;
    public string button = "specialAttack";
    // Start is called before the first frame update
    private string attack = "isWeak";
    void Start()
    {
        MusicManager.OnBeat += ComboCheck;
        MusicManager.OnBeat += ToggleOnBeatColor;
        
        MusicManager.OffBeat += ComboClear;
        MusicManager.OffBeat += ToggleOffBeatColor;

        origColor = light2D.color;
    }

    void Update(){
        ShootAnim(attack);
    }
    // This adds 1 to the combo counter and checks for a combo when an OnBeat
    // event happens
    private void ComboCheck()
    {
        int beat = BeatSystem.beatIndex;

        if (beat == lastBeat)
        {
            attack = "isStrong";
            Debug.Log("Weak");  
            // Player hit slightly before the beat, therefore the real beat is beat + 1
            lastBeat = beat + 1;
            consecOnBeats += 1;
        }
        else if (beat == lastBeat + 1)
        {
            attack = "isStrong";
            Debug.Log("Strong");
            // Player hit on or slightly after beat
            lastBeat = beat;
            consecOnBeats += 1;
        }
        else
        {
            attack = "isStrong";
            // Start of a new combo
            lastBeat = beat;
            consecOnBeats = 1;
        }
            
        if (consecOnBeats == 3)
        {
            DoRhythmAttack();

            // Clear combo counter
            consecOnBeats = 0;
        }
        
    }

    // This clears the combo counter when an OffBeat event happens
    private void ComboClear()
    {
        attack = "isWeak";
        consecOnBeats = 0;
    }

    // This gives a green glow around the player during an OnBeat event
    private void ToggleOnBeatColor()
    {
        rend.material = onBeatMat;
        light2D.color = onBeatColor;

        light2D.intensity = 3;

        StartCoroutine(ApplyOriginalMaterial());
    }

    // This gives a red glow around the player during an OffBeat event
    private void ToggleOffBeatColor()
    {
        rend.material = offBeatMat;
        light2D.color = offBeatColor;

        light2D.intensity = 3;

        StartCoroutine(ApplyOriginalMaterial());
    }

    // Puts back the original material after .4 seconds
    IEnumerator ApplyOriginalMaterial()
    {
        yield return new WaitForSeconds(.4f);

        rend.material = playerMat;
        light2D.color = origColor;
    }

    private void ShootAnim(string attack)
    {
        if (Input.GetButtonDown(button))
        {
            // Shoot();
            shotDelayCounter = shotDelay;
            animator.SetBool(attack, true);
            if(attack == "isStrong"){
                Strong();
            }
            else{
                Weak();
            }
        }
        else if (Input.GetButtonUp(button))
        {
            animator.SetBool(attack, false);
        }
        // if (Input.GetButton(button))
        // {
        //     shotDelayCounter -= Time.deltaTime;
        //     if (shotDelayCounter <= 0)
        //     {
        //         shotDelayCounter = shotDelay;
        //         // Shoot();

        //     }
        // }
    }

    void Weak()
    {
        Instantiate(weakPrefab, ShootPoint.position, ShootPoint.rotation);
    }

    void Strong(){
        Instantiate(strongPrefab, ShootPoint.position, ShootPoint.rotation);
    }
    private void DoRhythmAttack()
    {
        print("RHYTHM ATTACK");
    }
}
