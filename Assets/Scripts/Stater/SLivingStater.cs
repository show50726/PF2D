//S(Stater) Living Stater by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/05/31
//Usage:            State the info of living unit, such as move speed. Assign it to any GO that needs these info.

using System;
using UnityEngine;
namespace CMSR
{
    public class SLivingStater : SUnitStater
    {
        public float moveSpeed = 0;
        public float jumpSpeed = 0;

        private float iniMoveSpeed = 0;
        private float iniJumpSpeed = 0;

        protected override void Reset()
        {
            base.Reset();
            moveSpeed = 0;
            jumpSpeed = 0;
            iniMoveSpeed = 0;
            iniJumpSpeed = 0;
        }


    }
}