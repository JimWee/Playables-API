using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioClip[] AudioClips;

    [Range(0, 1)]
    public float OutputWeight;
    [Range(0, 1)]
    public float Weight;
    [Range(0, 1)]
    public float Weight01;
    [Range(0, 1)]
    public float Weight02;

    PlayableGraph mPlayableGraph;
    AudioPlayableOutput mAudioOutput;
    AudioMixerPlayable mAudioMixerPlayable;
    AudioClipPlayable mAudioClipPlayable01;
    AudioClipPlayable mAudioClipPlayable02;
    

    bool mInited = false;

    void Start () {
        mPlayableGraph = PlayableGraph.Create("AudioManager");
        mAudioOutput = AudioPlayableOutput.Create(mPlayableGraph, "Audio", GetComponent<AudioSource>());

        mAudioClipPlayable01 = AudioClipPlayable.Create(mPlayableGraph, AudioClips[0], true);
        mAudioClipPlayable02 = AudioClipPlayable.Create(mPlayableGraph, AudioClips[1], true);

        mAudioMixerPlayable = AudioMixerPlayable.Create(mPlayableGraph, 2);

        mPlayableGraph.Connect(mAudioClipPlayable01, 0, mAudioMixerPlayable, 0);
        mPlayableGraph.Connect(mAudioClipPlayable02, 0, mAudioMixerPlayable, 1);

        mAudioOutput.SetSourcePlayable(mAudioMixerPlayable);

        mPlayableGraph.Play();

        mInited = true;
	}
	
	void Update ()
    {
        if (!mInited)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            mAudioClipPlayable01.SetClip(AudioClips[Random.Range(0, AudioClips.Length)]);
        }

        //mAudioMixerPlayable.SetInputWeight(0, Weight);
        //mAudioMixerPlayable.SetInputWeight(1, 1f - Weight);

        mAudioMixerPlayable.SetInputWeight(0, Weight01);
        mAudioMixerPlayable.SetInputWeight(1, Weight02);
        mAudioOutput.GetTarget().volume = OutputWeight;
    }

    private void OnEnable()
    {
        if (!mInited)
        {
            return;
        }

        if (!mPlayableGraph.IsPlaying())
        {
            mPlayableGraph.Play();
        }
    }

    private void OnDisable()
    {
        if (!mInited)
        {
            return;
        }

        mPlayableGraph.Stop();
    }

    void OnDestroy()
    {
        if (!mInited)
        {
            return;
        }

        mPlayableGraph.Destroy();    
    }
}
