﻿using RacketLite.ValueTypes;
using System;
using System.Collections.Generic;

namespace RacketLite.Expressions
{
    public sealed class CosineExpression : NumericExpression
    {
        private CosineExpression(List<IRacketObject> args)
            : base("Cosine")
        {
            arguments = args;
        }

        public static new CosineExpression? Parse(string str)
        {
            List<IRacketObject>? arguments = RacketParsingHelper.ParseRacketNumbers(str);
            if (arguments?.Count == 1)
            {
                return new CosineExpression(arguments);
            }
            return null;
        }

        public override RacketNumber Evaluate()
        {
            RacketNumber currentNumber = (RacketNumber)arguments[0].Evaluate();
            return RacketNumber.Parse(MathF.Cos(currentNumber.Value), false, currentNumber.IsRational);
        }
    }
}
