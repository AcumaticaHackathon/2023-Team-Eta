using PX.Data;

namespace ACPROpenAI.Descriptor.Attributes
{
    public class ACPRAiModel
    {
        public const string TextDavinci003 = "text-davinci-003";
        public const string TextDavinci002 = "text-davinci-002";
        public const string TextDavinci001 = "text-davinci-001";

        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(
                new string[] 
                { 
                    TextDavinci003, 
                    TextDavinci002, 
                    TextDavinci001 
                },
                new string[] 
                {
                    "Text Davinci 003",
                    "Text Davinci 002",
                    "Text Davinci 001"
                })
            { }
        }

        public class textDavinci003 : PX.Data.BQL.BqlString.Constant<textDavinci003>
        {
            public textDavinci003() : base(TextDavinci003) { }
        }

        public class textDavinci002 : PX.Data.BQL.BqlString.Constant<textDavinci002>
        {
            public textDavinci002() : base(TextDavinci002) { }
        }

        public class textDavinci001 : PX.Data.BQL.BqlString.Constant<textDavinci001>
        {
            public textDavinci001() : base(TextDavinci001) { }
        }
    }
}
