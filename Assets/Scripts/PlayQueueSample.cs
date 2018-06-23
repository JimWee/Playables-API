using UnityEngine;

using UnityEngine.Animations;

using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]

public class PlayQueueSample : MonoBehaviour

{

    public AnimationClip[] clipsToPlay;

    PlayableGraph playableGraph;

    void Start()

    {

        playableGraph = PlayableGraph.Create();

        var playQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playableGraph);

        var playQueue = playQueuePlayable.GetBehaviour();

        playQueue.Initialize(clipsToPlay, playQueuePlayable, playableGraph);

        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());

        playableOutput.SetSourcePlayable(playQueuePlayable);
        playableOutput.SetSourceInputPort(0);

        playableGraph.Play();

    }

    void OnDisable()

    {

        // Destroys all Playables and Outputs created by the graph.

        playableGraph.Destroy();

    }

}
