using rentap.backend.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace rentap.backend.Core.Helpers
{
    internal class Reflection
    {
        public static PropertyPathItem[] GetPropertyPath<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            var properties = new Stack<PropertyPathItem>();
            var exp = expression.Body;
            while (exp != null)
            {
                var ppi = new PropertyPathItem();
                if (exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked)
                {
                    var unaryExpression = (UnaryExpression)exp;
                    ppi.ConvertionType = unaryExpression.Type;
                    exp = unaryExpression.Operand;
                }

                if (!(exp is MemberExpression memberExpression))
                {
                    exp = null;
                }
                else
                {
                    ppi.FieldOrProperty = memberExpression.Member;
                    exp = memberExpression.Expression;
                    properties.Push(ppi);
                }
            }

            return properties.ToArray();
        }
    }
}