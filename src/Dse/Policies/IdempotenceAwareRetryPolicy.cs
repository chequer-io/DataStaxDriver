//
//  Copyright (C) 2017 DataStax, Inc.
//
//  Please see the license for details:
//  http://www.datastax.com/terms/datastax-dse-driver-license-terms
//

using System;

// ReSharper disable once CheckNamespace : All policies are on the root namespace
namespace Dse
{
    /// <summary>
    /// A retry policy that avoids retrying non-idempotent statements.
    /// <para>
    /// In case of write timeouts this policy will always return <see cref="RetryDecision.Rethrow()"/>
    /// if the statement is considered non-idempotent (see <see cref="IStatement.IsIdempotent"/>).
    /// For all other cases, this policy delegates the decision to the child policy.
    /// </para>
    /// </summary>
    public class IdempotenceAwareRetryPolicy : IExtendedRetryPolicy
    {
        private readonly IExtendedRetryPolicy _extendedChildPolicy;

        /// <summary>
        /// Creates a new instance of <see cref="IdempotenceAwareRetryPolicy"/>.
        /// </summary>
        /// <param name="childPolicy">The retry policy to wrap.</param>
        public IdempotenceAwareRetryPolicy(IRetryPolicy childPolicy)
        {
            ChildPolicy = childPolicy ?? throw new ArgumentNullException("childPolicy");
            _extendedChildPolicy = childPolicy as IExtendedRetryPolicy;
        }

        public IRetryPolicy ChildPolicy { get; }

        /// <inheritdoc />
        public RetryDecision OnReadTimeout(IStatement stmt, ConsistencyLevel cl, int requiredResponses, int receivedResponses, bool dataRetrieved, int nbRetry)
        {
            return ChildPolicy.OnReadTimeout(stmt, cl, requiredResponses, receivedResponses, dataRetrieved, nbRetry);
        }

        /// <inheritdoc />
        public RetryDecision OnWriteTimeout(IStatement stmt, ConsistencyLevel cl, string writeType, int requiredAcks, int receivedAcks, int nbRetry)
        {
            if (stmt != null && stmt.IsIdempotent == true)
            {
                return ChildPolicy.OnWriteTimeout(stmt, cl, writeType, requiredAcks, receivedAcks, nbRetry);
            }
            return RetryDecision.Rethrow();
        }

        /// <inheritdoc />
        public RetryDecision OnUnavailable(IStatement stmt, ConsistencyLevel cl, int requiredReplica, int aliveReplica, int nbRetry)
        {
            return ChildPolicy.OnUnavailable(stmt, cl, requiredReplica, aliveReplica, nbRetry);
        }

        /// <inheritdoc />
        public RetryDecision OnRequestError(IStatement stmt, Configuration config, Exception ex, int nbRetry)
        {
            if (stmt != null && stmt.IsIdempotent == true)
            {
                if (_extendedChildPolicy != null)
                {
                    return _extendedChildPolicy.OnRequestError(stmt, config, ex, nbRetry);
                }
                return RetryDecision.Retry(null, false);
            }
            return RetryDecision.Rethrow();
        }
    }
}
