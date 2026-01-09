using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check if player is seen by any AI agent", story: "Check if [CanSeePlayer] is true", category: "Action", id: "e39f8f529640b6ddd9ddd8fe503272b7")]
public partial class CheckIfPlayerIsSeenByAnyAiAgentAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> CanSeePlayer;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (CanSeePlayer.Value == false)
        {
            Debug.Log("CheckIfPlayerIsSeenByAnyAiAgentAction: Can See Player = " + CanSeePlayer.Value);
        } else {
            Debug.Log("CheckIfPlayerIsSeenByAnyAiAgentAction: Can See Player = " + CanSeePlayer.Value);
        }
        return CanSeePlayer.Value ? Status.Success : Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

