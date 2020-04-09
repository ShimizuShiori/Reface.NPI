﻿using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public string Name { get; private set; }

        public ParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
