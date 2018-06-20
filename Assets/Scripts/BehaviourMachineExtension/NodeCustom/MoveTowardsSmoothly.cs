//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
// Improved by STC 2018 / stc.ntu@gmail.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

namespace BehaviourMachine
{

    /// <summary>
    /// The "Game Object" will smooth follows the "Target".
    /// </summary>
    [NodeInfo(category = "Action/Transform/",
                icon = "Transform",
                description = "The \"Game Object\" will move towards the \"Target\" smoothly.")]
    public class MoveTowardsSmoothly : ActionNode
    {
        /// <summary>
        /// The object that will be moved.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Use Self", tooltip = "The object that will be moved")]
        public GameObjectVar objectToMove;

        /// <summary>
        /// The "Object To Move" will move towards this game object.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Use Self", tooltip = "The \"Object To Move\" will move towards this game object")]
        public GameObjectVar targetPosition;

        /// <summary>
        /// The speed to move the "Object To Move".
        /// </summary>
        [VariableInfo(tooltip = "The speed to move the \"Object To Move\"")]
        public FloatVar moveSpeed;

        /// <summary>
        /// If set to True ignores the x axis.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Don't Ignore", tooltip = "If set to True ignores the x axis")]
        public BoolVar ignoreXAxis;

        /// <summary>
        /// If set to True ignores the y axis.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Don't Ignore", tooltip = "If set to True ignores the y axis")]
        public BoolVar ignoreYAxis;

        /// <summary>
        /// If set to True ignores the z axis.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Don't Ignore", tooltip = "If set to True ignores the z axis")]
        public BoolVar ignoreZAxis;
        [VariableInfo(requiredField = false, nullLabel = "Use position", tooltip = "If assigned, will use it as movement direction(ignore beyond sets)")]
        public Vector3Var direction;

        [System.NonSerialized]
        Transform m_TransformToMove = null;
        [System.NonSerialized]
        Transform m_Position = null;

        public override void Reset()
        {
            objectToMove = new ConcreteGameObjectVar(this.self);
            targetPosition = new ConcreteGameObjectVar(this.self);
            moveSpeed = 1f;
            ignoreXAxis = new ConcreteBoolVar();
            ignoreYAxis = new ConcreteBoolVar();
            ignoreZAxis = new ConcreteBoolVar();
            direction = new ConcreteVector3Var();
        }

        public override Status Update()
        {
            // Get the transform1
            if (m_TransformToMove == null || m_TransformToMove.gameObject != objectToMove.Value)
                m_TransformToMove = objectToMove.Value != null ? objectToMove.Value.transform : null;

            // Get the transform2
            if (m_Position == null || m_Position.gameObject != targetPosition.Value)
                m_Position = targetPosition.Value != null ? targetPosition.Value.transform : null;

            // Check for errors
            if (moveSpeed.isNone || m_TransformToMove == null || m_Position == null)
                return Status.Error;

            // Get the position to move
            var positionToMove = m_Position.position;

            // Ignore axis?
            if (!ignoreXAxis.isNone && ignoreXAxis.Value) positionToMove.x = m_TransformToMove.position.x;
            if (!ignoreYAxis.isNone && ignoreYAxis.Value) positionToMove.y = m_TransformToMove.position.y;
            if (!ignoreZAxis.isNone && ignoreZAxis.Value) positionToMove.z = m_TransformToMove.position.z;

            // direction?
            if (!direction.isNone) positionToMove = m_TransformToMove.position + direction;

            m_TransformToMove.position = Vector3.Lerp(m_TransformToMove.position, positionToMove, moveSpeed.Value * owner.deltaTime);
            return Status.Success;
        }
    }
}