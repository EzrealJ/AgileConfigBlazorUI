using System;
using System.Collections.Generic;

namespace AgileConfig.BlazorUI.Model
{
    public class AgileConfigServerSetting : IEquatable<AgileConfigServerSetting>
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsCurrent { get; set; }

        public override bool Equals(object obj) => Equals(obj as AgileConfigServerSetting);
        public bool Equals(AgileConfigServerSetting other) => other is not null && Name == other.Name && Url == other.Url && IsCurrent == other.IsCurrent;
        public override int GetHashCode() => HashCode.Combine(Name, Url, IsCurrent);

        public static bool operator ==(AgileConfigServerSetting left, AgileConfigServerSetting right) => EqualityComparer<AgileConfigServerSetting>.Default.Equals(left, right);
        public static bool operator !=(AgileConfigServerSetting left, AgileConfigServerSetting right) => !(left == right);
    }
}
