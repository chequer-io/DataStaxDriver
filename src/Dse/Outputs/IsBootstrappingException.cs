//
//  Copyright (C) 2017 DataStax, Inc.
//
//  Please see the license for details:
//  http://www.datastax.com/terms/datastax-dse-driver-license-terms
//

namespace Dse
{
    public class IsBootstrappingException : QueryValidationException
    {
        public IsBootstrappingException(string message) : base(message)
        {
        }
    }
}