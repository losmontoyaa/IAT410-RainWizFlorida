using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] EventReference BGAmbient;
    //[SerializeField] PlayerController p;
    [SerializeField] EventReference Filter;

    //FMOD.Studio.EventInstance Filter;

    private void Start()
    {
        PlayAmbience();
    }

    public void PlayAmbience()
    {
        RuntimeManager.PlayOneShot(BGAmbient);
    }

    public void AddFilter()
    {
        RuntimeManager.PlayOneShot(Filter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            print("guap");
            AddFilter();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Filter.release();
        }
    }

    private void OnDestroy()
    {
        Filter.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        RuntimeManager.
    }
}
