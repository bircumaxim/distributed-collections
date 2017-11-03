using System;
using System.Linq;
using System.Linq.Expressions;
using Serialize.Linq.Extensions;

namespace Common.Models.Filters
{
    public class WhereFilter : Filter<Employee>
    {
        public Expression<Func<Employee, bool>> Predicate { get; set; }

        protected override Expression GetExpression()
        {
            return Predicate;
        }

        protected override void SetExpression(Expression expression)
        {
            Predicate = expression.ToExpressionNode().ToBooleanExpression<Employee>();
        }

        public override Employee[] Execute(Employee[] listItems)
        {
            return listItems.Where(Predicate.Compile()).ToArray();
        }
    }
}