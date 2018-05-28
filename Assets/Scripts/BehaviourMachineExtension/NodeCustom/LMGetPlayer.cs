//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//LMGetPlayer made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/28

using UnityEngine;
using BehaviourMachine;

/// <summary>
/// Get player from Level Manager.
/// </summary>
[NodeInfo(category = "Action/Blackboard/",
            icon = "Blackboard",
            description = "Get player from Level Manager.")]
public class LMGetPlayer : ActionNode
{

    [VariableInfo(canBeConstant = false, tooltip = "The (blackboard) var to store player 1.")]
    public GameObjectVar player1;
    [VariableInfo(canBeConstant = false, tooltip = "The (blackboard) var to store player 2.")]
    public GameObjectVar player2;

    public override void Reset()
    {
        player1 = new ConcreteGameObjectVar();
        player2 = new ConcreteGameObjectVar();
    }


    public override Status Update()
    {
        if (LevelManager.exist == null)
        {
            Print.LogError("Cannot find Level Manager.");
            return Status.Error;
        }
        Player[] playerList = LevelManager.exist.players;
        player1.Value = playerList[0].gameObject;
        player2.Value = playerList[1].gameObject;
        
        return Status.Success;
    }

}
