using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators;
public class RebateCalculatorTypeCollection : IEnumerable<Type>
{
    private readonly IList<Type> rebateCalculators;

    public RebateCalculatorTypeCollection()
    {
        var assembly = Assembly.GetExecutingAssembly();
        rebateCalculators = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IRebateCalculator)))
            .Where(t => Attribute.IsDefined(t, typeof(IncentiveTypeCalculatorAttribute)))
            .ToList();
    }

    public Type GetFor(IncentiveType incentiveType)
        => rebateCalculators
            .Where(t => t.GetCustomAttribute<IncentiveTypeCalculatorAttribute>()?.IncentiveType == incentiveType)
            .LastOrDefault();

    public IEnumerator<Type> GetEnumerator() => rebateCalculators.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
