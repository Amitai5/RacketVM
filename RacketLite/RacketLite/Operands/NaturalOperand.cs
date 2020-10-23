﻿using RacketLite.Parsing;
using System;

namespace RacketLite.Operands
{
    public class NaturalOperand : IOperable
    {
        public long OperandValue;
        public bool Inexact { get; private set; }

        public NaturalOperand(long value, bool inexact)
            : base(RacketOperandType.Natural)
        {
            Inexact = inexact;
            OperandValue = value;
        }

        #region IOperable Overrides
        public override bool Equals(object obj)
        {
            return OperandValue == ((NaturalOperand)obj).OperandValue;
        }

        public override int CompareTo(object obj)
        {
            double otherValue = ((NaturalOperand)obj).OperandValue;
            return ((double)OperandValue).CompareTo(otherValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, OperandValue);
        }

        public override string ToString()
        {
            string operandValueString = OperandValue.ToString().ToUpper();
            if (Inexact)
            {
                return $"{ParsingRules.InexactNumberPrefix}{operandValueString:0.0}";
            }
            return operandValueString;
        }
        #endregion IOperable Overrides
    }
}
