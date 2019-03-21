//
//  Copyright (C) 2016 DataStax, Inc.
//
//  Please see the license for details:
//  http://www.datastax.com/terms/datastax-dse-driver-license-terms
//

using Dse.Insights;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Dse.Test.Integration.TestClusterManagement
{
    /// <summary>
    /// An attribute that filters the test to execute according to whether the current DSE version supports insights.
    /// </summary>
    public class TestInsightsVersion : NUnitAttribute, IApplyToTest
    {
        public void ApplyToTest(NUnit.Framework.Internal.Test test)
        {
            var executingVersion = TestClusterManager.DseVersion;
            var insightsSupportVerifier = new InsightsSupportVerifier();
            if (insightsSupportVerifier.DseVersionSupportsInsights(executingVersion))
            {
                return;
            }

            test.RunState = RunState.Ignored;
            var message = $"Test designed to run with DSE version that supports Insights (executing {executingVersion})";
            test.Properties.Set("_SKIPREASON", message);
        }
    }
}