using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountLessThan<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        private int _countLessThan;

        public CountLessThan(int countLessThan)
        {
            _countLessThan = countLessThan;
        }

        public CountLessThan(Expression<Func<T, int>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() {{"", _countLessThan}}; }
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.Any())
            {
                _countLessThan = (int)GetExpressionValue(context);
            }

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount < _countLessThan, context, notification);
        }
    }
}