//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//FlipSprite made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/07/04

using UnityEngine;

namespace BehaviourMachine
{
    /// <summary>
    /// Flip the sprite by x or by y.
    /// </summary>
    [NodeInfo(category = "Action/Sprite/",
        icon = "SpriteRenderer",
        description = "Flip the sprite by x or by y.")]
    public class FlipSprite : ActionNode
    {
        /// <summary>
        /// The game object that has a SpriteRenderer in it.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Use Self", tooltip = "The game object that has a SpriteRenderer in it")]
        public GameObjectVar gameObject;


        [VariableInfo(requiredField = false, nullLabel = "don't change", tooltip = "If set, will set value to Flip.X.")]
        public BoolVar flipX;
        [VariableInfo(requiredField = false, nullLabel = "don't change", tooltip = "If set, will set value to Flip.Y.")]
        public BoolVar flipY;

        public override void Reset()
        {
            gameObject = this.self;
            flipX = new ConcreteBoolVar();
            flipY = new ConcreteBoolVar();
        }

        public override Status Update()
        {
            // Get the sprite renderer
            SpriteRenderer spriteRenderer = gameObject.Value != null ? gameObject.Value.GetComponent<SpriteRenderer>() : self.GetComponent<SpriteRenderer>();

            // Validate members
            if (spriteRenderer == null)
                return Status.Error;

            // Flip X
            if (!flipX.isNone)
            {
                spriteRenderer.flipX = flipX.Value;
            }

            // Flip Y
            if (!flipY.isNone)
            {
                spriteRenderer.flipY = flipY.Value;
            }
            
            return Status.Success;
        }

    }
}