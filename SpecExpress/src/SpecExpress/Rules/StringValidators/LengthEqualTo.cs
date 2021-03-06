﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SpecExpress.Rules.StringValidators
{   
    public class LengthEqualTo<T> : RuleValidator<T, string>
    {
        private int _length;

        public LengthEqualTo(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length should be greater than 0");
            }
            Params.Add(new RuleParameter("length", length));
        }

        public LengthEqualTo(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("length", expression));
        }

        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {

            int length = String.IsNullOrEmpty(context.PropertyValue) ? 0 : context.PropertyValue.Length;

            var contextWithLength = new RuleValidatorContext<T, string>(context.Instance, context.PropertyName, length.ToString(),
                                                                           context.PropertyInfo, context.Level, null);

            var lengthParamVal = (int)Params[0].GetParamValue(context);

            return Evaluate(length == lengthParamVal, contextWithLength, notification);
        }
    }
}
