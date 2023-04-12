using System;
using System.Linq.Expressions;

namespace Benraz.Infrastructure.EF
{
    /// <summary>
    /// Expression extensions.
    /// </summary>
    public static class ExpressionExtensions
    {
        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }

                return base.Visit(node);
            }
        }

        /// <summary>
        /// Creates predicate that represents a conditional AND operation.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>Predicate that represents a conditional AND operation.</returns>
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var leftExpression = leftVisitor.Visit(left.Body);

            var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var rightExpression = rightVisitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(leftExpression, rightExpression), parameter);
        }

        /// <summary>
        /// Creates predicate that represents a conditional OR operation.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>Predicate that represents a conditional OR operation.</returns>
        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var leftExpression = leftVisitor.Visit(left.Body);

            var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var rightExpression = rightVisitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(leftExpression, rightExpression), parameter);
        }
    }
}




