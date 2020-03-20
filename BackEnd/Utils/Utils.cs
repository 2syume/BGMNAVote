using System;
using System.Linq;
using System.Linq.Expressions;

namespace BGMNANotebookGrab.Utils
{
    public static class Utils
    {
        public static LambdaExpression ObjectPropertyExpression<T>(string propertyName)
        {
            var objectType = typeof(T);
            var property = objectType.GetProperties().FirstOrDefault(t => t.Name == propertyName);
            if (property == null)
                throw new ArgumentException("Cannot find the property");
            var parameterExpression = Expression.Parameter(typeof(T), nameof(T));
            var propertyExpression = Expression.Property(parameterExpression, property);
            return Expression.Lambda(propertyExpression, parameterExpression);
        }
    }
}