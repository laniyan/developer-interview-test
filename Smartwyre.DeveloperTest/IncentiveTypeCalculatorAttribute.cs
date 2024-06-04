using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest;

[AttributeUsage(AttributeTargets.Class)]
public class IncentiveTypeCalculatorAttribute(IncentiveType incentiveType) : Attribute
{
    public IncentiveType IncentiveType { get; } = incentiveType;
}
