﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RacketVM.Operands
{
    public class StringOperand : IOperable
    {
        public string OperandValue;

        public StringOperand(string value)
            : base(OperandType.String)
        {
            OperandValue = value;
        }

        #region Object Overrides

        public override bool Equals(object obj)
        {
            return OperandValue == ((StringOperand)obj).OperandValue;
        }

        public override int CompareTo(object obj)
        {
            string otherValue = ((StringOperand)obj).OperandValue;
            return OperandValue.CompareTo(otherValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, OperandValue);
        }

        public override string ToString()
        {
            return $"\"{OperandValue}\"";
        }

        #endregion Object Overrides
    }
}
