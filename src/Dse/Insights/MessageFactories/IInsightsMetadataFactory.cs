﻿// 
//       Copyright (C) 2019 DataStax, Inc.
// 
//     Please see the license for details:
//     http://www.datastax.com/terms/datastax-dse-driver-license-terms
// 

using Dse.Insights.Schema;

namespace Dse.Insights.MessageFactories
{
    internal interface IInsightsMetadataFactory
    {
        InsightsMetadata CreateInsightsMetadata(string messageName, string mappingId, InsightType insightType);
    }
}