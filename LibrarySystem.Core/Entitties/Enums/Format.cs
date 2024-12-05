using System;
using System.Runtime.Serialization;

namespace LibrarySystem.Core.Entitties.Enums
{
    public enum Format
    {
        [EnumMember(Value = "Paperback")]
        Paperback,

        [EnumMember(Value = "Flashcards")]
        Flashcards,

        [EnumMember(Value = "Flex-Bind")]
        FlexBind,

        [EnumMember(Value = "Hardcover")]
        Hardcover,

        [EnumMember(Value = "Hardcover-spiral")]
        HardcoverSpiral,

        [EnumMember(Value = "International Paperback")]
        InternationalPaperback,

        [EnumMember(Value = "Mass Market Paperback")]
        MassMarketPaperback,

        [EnumMember(Value = "Paperback Trade")]
        PaperbackTrade
    }
}
