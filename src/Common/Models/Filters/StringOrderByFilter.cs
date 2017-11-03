using System;
using System.Linq;
using System.Linq.Expressions;
using Serialize.Linq.Extensions;

namespace Common.Models.Filters
{
    public class StringOrderByFilter : Filter<Employee>
    {
        public Expression<Func<Employee, string>> Func { get; set; }
        
        protected override Expression GetExpression()
        {
            return Func;
        }

        protected override void SetExpression(Expression expression)
        {
            Func = expression.ToExpressionNode().ToExpression<Func<Employee, string>>();
        }

        public override Employee[] Execute(Employee[] items)
        {
            return items.OrderBy(Func.Compile()).ToArray();
        }
    }
}