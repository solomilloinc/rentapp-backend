using System;
using System.Reflection;

namespace rentap.backend.Core.Helper
{
    public class PropertyPathItem
    {
        #region Public Properties

        public Type ConvertionType { get; set; }

        public MemberInfo FieldOrProperty { get; set; }

        #endregion
    }
}