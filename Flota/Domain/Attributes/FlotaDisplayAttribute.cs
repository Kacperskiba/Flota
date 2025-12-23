using System;
namespace Flota.Domain.Attributes;
[AttributeUsage(AttributeTargets.Property)]
public class FlotaDisplayAttribute : Attribute {
    public string Header { get; }
    public int Order { get; }
    public FlotaDisplayAttribute(string header, int order) { Header = header; Order = order; }
}