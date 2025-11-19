using System;
using JetBrains.Annotations;

namespace NouAssist.SharedKernel.Extension // Use your root namespace
{
    public static class Ensure
    {
        public static decimal NotNegative(
            decimal value, 
            [InvokerParameterName] string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentException($"{parameterName} cannot be negative!", parameterName);
            }

            return value;
        }

        public static int NotNegative(
            int value, 
            [InvokerParameterName] string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentException($"{parameterName} cannot be negative!", parameterName);
            }

            return value;
        }
        
        public static double NotNegative(
            double value, 
            [InvokerParameterName] string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentException($"{parameterName} cannot be negative!", parameterName);
            }

            return value;
        }
    }
}