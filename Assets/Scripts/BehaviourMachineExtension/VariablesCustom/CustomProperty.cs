//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
// Customized Property by STC 2018 stc.ntu@gmail.com
//----------------------------------------------

using UnityEngine;
using System.Reflection;

namespace BehaviourMachine
{
    /// <summary>
    /// Calculate the weight. Would be useful when use AddForce.
    /// </summary>
    [System.Serializable]
    [CustomVariable("Reflection_10")]
    public class GetWeight : FloatVar
    {

        [VariableInfo(tooltip = "The target object")]
        public UnityEngine.Object target;

        [HideInInspector]
        public Component component;

        [HideInInspector]
        public bool isStatic;

        //[ReflectedProperty("target", "component", "isStatic", typeof(float))]
        //[Tooltip("The name of the property")]
        //public string property = string.Empty;


        #region Private Members
        [System.NonSerialized]
        MemberInfo m_MemberInfo;
        //[System.NonSerialized]
        //PropertyInfo m_PropertyInfo;
        //[System.NonSerialized]
        //FieldInfo m_FieldInfo;
        //[System.NonSerialized]
        //object m_Target;
        #endregion Private Members


        /// <summary>
        /// Init the private data.
        /// </summary>
        void Initialize()
        {
            if (target != null)
            {
                //System.Type targetType = null;
                //if (component == null)
                //{
                //    targetType = target.GetType();
                //    m_Target = isStatic ? null : target;
                //}
                //else
                //{
                //    targetType = component.GetType();
                //    m_Target = isStatic ? null : component;
                //}

                //// Try to get the property info
                //m_PropertyInfo = targetType.GetProperty(property);

                //if (m_PropertyInfo == null)
                //{
                //    // Try to get the field info
                //    m_FieldInfo = targetType.GetField(property);
                //    m_MemberInfo = m_FieldInfo;
                //}
                //else
                //    m_MemberInfo = m_PropertyInfo;
            }
        }

        /// <summary>
        /// Variable value.
        /// </summary>
        public override float Value
        {
            get
            {
                // Needs to load the member info?
                //if (m_MemberInfo == null)
                //    Initialize();

                // Get value
                GameObject obj = (GameObject)target;
                if (obj != null)
                {
                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        return rb.mass * Physics.gravity.magnitude;
                    }
                    Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D>();
                    if (rb2d != null)
                    {
                        //Debug.Log("rb2d value: (mass)" + rb2d.mass + "; (gScale)" + rb2d.gravityScale + ";(gravity)" + Physics2D.gravity);
                        return rb2d.mass * rb2d.gravityScale * Physics2D.gravity.magnitude;
                    }

                }
                else
                {
                    Print.Log("cannot get rigidbody, thus cannot sure weight. Will use 1 as value instead.");
                    return 1;
                }

                //if (m_PropertyInfo != null)
                //{
                //    return (GameObject)m_PropertyInfo.GetValue(m_Target, null);
                //}
                //else if (m_FieldInfo != null)
                //{
                //    return (GameObject)m_FieldInfo.GetValue(m_Target);
                //}
                //else
                //{
                //    if (component == null)
                //        Print.LogError("No property with name \'" + property + "\' in the object \'" + target + "\'.");
                //    else
                //        Print.LogError("No property with name \'" + property + "\' in the component \'" + target + "." + component + "\'.");
                //    return null;
                //}
                return 1;

            }

            set
            {
                // Needs to load the member info?
                //if (m_MemberInfo == null)
                //    Initialize();

                // Set value
                //if (m_PropertyInfo != null)
                //{
                //    m_PropertyInfo.SetValue(m_Target, value, null);
                //}
                //else if (m_FieldInfo != null)
                //{
                //    m_FieldInfo.SetValue(m_Target, value);
                //}
                //else
                //{
                //    if (component == null)
                //        Print.LogError("(GameObjectProperty) No property with name \'" + property + "\' in the object \'" + target + "\'.");
                //    else
                //        Print.LogError("No property with name \'" + property + "\' in the component \'" + target + "." + component + "\'.");
                //}
            }
        }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public GetWeight() : base() { }

        /// <summary>
        /// Constructor for constants.
        /// <param name="self">The GameObject that owns the variable.</param>
        /// </summary>
        public GetWeight(GameObject self) : base()
        {
            this.target = self;
        }

        /// <summary>
        /// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
        /// Validate members.
        /// </summary>
        public override void OnValidate()
        {
            base.OnValidate();

            if (target == null || (component != null && component.gameObject != target))
            {
                component = null;
                isStatic = false;
                //property = string.Empty;
            }
        }
    }
}