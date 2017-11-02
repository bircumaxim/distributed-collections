using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Models;
using Common.Models.Filters;

namespace Common
{
    public class DataRequestMessageBuilder
    {
        private long _requestTimeout;
        private readonly List<Filter<Employee>> _filters;

        public DataRequestMessageBuilder()
        {
            _filters = new List<Filter<Employee>>();
        }

        public DataRequestMessageBuilder Where(Expression<Func<Employee, bool>> predicate)
        {
            _filters.Add(new WhereFilter
            {
                Predicate = predicate
            });
            return this;
        }

        public DataRequestMessageBuilder WithTimeout(long requestTimeout)
        {
            _requestTimeout = requestTimeout;
            return this;
        }

        public DataRequestMessage Build()
        {
            return new DataRequestMessage
            {
                Filters = _filters,
                RequestTimeout = _requestTimeout
            };
        }
    }
}