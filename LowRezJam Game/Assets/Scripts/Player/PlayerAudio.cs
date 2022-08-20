using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public List<AudioSource> audioSources;

    public AudioSource JumpSource;
    public AudioSource AttackSource;
    public AudioSource HurtSource;
    public AudioSource EnergySource;

    private void Start()
    {
        foreach(AudioSource i in audioSources)
        {
            i.mute = true;
        }
    }

    public IEnumerator PlayJump()
    {
        JumpSource.mute = false;
        JumpSource.Play();
        yield return new WaitForSeconds(JumpSource.clip.length);
        JumpSource.mute = true;

    }

    public IEnumerator PlayAttack()
    {
        AttackSource.mute = false;
        AttackSource.Play();
        yield return new WaitForSeconds(AttackSource.clip.length);
        AttackSource.mute = true;
    }

    public IEnumerator PlayHurt()
    {
        HurtSource.mute = false;
        HurtSource.Play();
        yield return new WaitForSeconds(HurtSource.clip.length);
        HurtSource.mute = true;
    }

    public IEnumerator PlayEnergy()
    {
        EnergySource.mute = false;
        EnergySource.Play();
        yield return new WaitForSeconds(EnergySource.clip.length);
        EnergySource.mute = true;
    }
}
