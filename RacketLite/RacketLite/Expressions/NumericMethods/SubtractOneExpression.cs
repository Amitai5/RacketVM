﻿using RacketLite.ValueTypes;
using System.Collections.Generic;

namespace RacketLite.Expressions
{
    public sealed class SubtractOneExpression : NumericExpression
    {
        private SubtractOneExpression(List<IRacketObject> args)
            : base("SubtractOne")
        {
            parameters = args;
        }

        public static SubtractOneExpression? Parse(List<IRacketObject>? parameters)
        {
            RacketParsingHelper.ValidateParamTypes(typeof(RacketNumber), parameters);
            if (parameters?.Count == 1)
            {
                return new SubtractOneExpression(parameters);
            }
            return null;
        }

        public override RacketNumber Evaluate()
        {
            RacketNumber currentNumber = (RacketNumber)parameters[0].Evaluate();
            return RacketNumber.Parse(currentNumber.Value - 1, currentNumber.IsExact, currentNumber.IsRational);
        }
    }
}
