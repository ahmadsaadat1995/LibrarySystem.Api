using System;
using System.Runtime.Serialization;

namespace LibrarySystem.Core.Entitties.Enums
{
    public enum Language
    {
        [EnumMember(Value = "English")]
        English,

        [EnumMember(Value = "Arabic")]
        Arabic,

        [EnumMember(Value = "French")]
        French,
    }
}
