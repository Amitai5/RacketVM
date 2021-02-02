﻿using RacketLite.ValueTypes;
using System.Collections.Generic;

namespace RacketLite.Expressions
{
    public sealed class BooleanToStringExpression : StringExpression
    {
        private BooleanToStringExpression(List<IRacketObject> args)
            : base("Boolean->String")
        {
            parameters = args;
        }

        public static BooleanToStringExpression? Parse(List<IRacketObject>? parameters)
        {
            RacketParsingHelper.ValidateParamTypes(typeof(RacketBoolean), parameters);
            if (parameters?.Count == 1)
            {
                return new BooleanToStringExpression(parameters);
            }
            return null;
        }

        public override RacketString Evaluate()
        {
            bool booleanValue = ((RacketBoolean)parameters[0].Evaluate()).Value;
            return new RacketString(booleanValue.ToString().ToLower());
        }
    }
}
