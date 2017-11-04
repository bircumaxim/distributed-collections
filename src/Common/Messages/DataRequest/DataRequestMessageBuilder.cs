using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Models;
using Common.Models.Filters;

namespace Common.Messages.DataRequest
{
    public class DataRequestMessageBuilder
    {
        private int _requestTimeout;
        private DataType _dataType;
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

        public DataRequestMessageBuilder OrderBy(Expression<Func<Employee, string>> func)
        {
            _filters.Add(new StringOrderByFilter
            {
                Func = func
            });
            return this;
        }

        public DataRequestMessageBuilder OrderBy(Expression<Func<Employee, int>> func)
        {
            _filters.Add(new IntOrderByFilter
            {
                Func = func
            });
            return this;
        }

        public DataRequestMessageBuilder DataType(DataType dataType)
        {
            _dataType = dataType;
            return this;
        }

        public DataRequestMessageBuilder WithTimeout(int requestTimeout)
        {
            _requestTimeout = requestTimeout;
            return this;
        }

        public DataRequestMessage Build()
        {
            return new DataRequestMessage
            {
                Filters = _filters,
                RequestTimeout = _requestTimeout,
                DataType = Enum.GetName(typeof(DataType), _dataType)
            };
        }
    }
}