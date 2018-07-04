﻿//----------------------------------------------
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

        /// <summary>
        /// The new sprite.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Don't Use", tooltip = "The new sprite")]
        public ObjectVar newSprite;

        /// <summary>
        /// The new sprite.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Don't Use", tooltip = "The new sprite")]
        public ColorVar newColor;

        public override void Reset()
        {
            gameObject = this.self;
            newSprite = new ConcreteObjectVar();
            newColor = new ConcreteColorVar();
        }

        public override Status Update()
        {
            // Get the sprite renderer
            SpriteRenderer spriteRenderer = gameObject.Value != null ? gameObject.Value.GetComponent<SpriteRenderer>() : self.GetComponent<SpriteRenderer>();

            // Validate members
            if (spriteRenderer == null)
                return Status.Error;

            // Change the sprite
            if (!newSprite.isNone)
            {
                // Get the new sprite
                var sprite = newSprite.Value as Sprite;
                if (sprite != null)
                    spriteRenderer.sprite = sprite;
            }

            // Change the color
            if (!newColor.isNone)
                spriteRenderer.color = newColor.Value;

            return Status.Success;
        }

    }
}