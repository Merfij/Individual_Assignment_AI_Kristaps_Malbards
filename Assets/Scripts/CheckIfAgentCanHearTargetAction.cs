using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check if agent can hear Target", story: "Check if agent [can] is true", category: "Action", id: "a0294649f376ebed19a27aaa15665b32")]
public partial class CheckIfAgentCanHearTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<DetectPlayer> Can;
    [SerializeReference] public BlackboardVariable<PlayerMovement> Player;
    [SerializeReference] public BlackboardVariable<bool> WithiHearingRange;
    [SerializeReference] public BlackboardVariable<bool> HeardPlayer;
    [SerializeReference] public BlackboardVariable<Vector3> LastHeard;
    bool samePOS;

    protected override Status OnUpdate()
    {
        if (Can.Value.IsWithinHearingRange() == true)
        {
            WithiHearingRange.Value = true;
            if (Player.Value.hasWhistled == true && WithiHearingRange.Value == true)
            {
                HeardPlayer.Value = true;
                LastHeard.Value = Player.Value.whistlePOS.transform.position;
            }
        }
        else
        {
            WithiHearingRange.Value = false;
        }
        return WithiHearingRange.Value ? Status.Success : Status.Failure;
    }

}

