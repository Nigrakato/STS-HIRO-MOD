using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Hiro.Scripts.Keywords
{
    public static class HiroCardKeywords
    {
        [CustomEnum("ZHENGYI")]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Zhengyi;

        [CustomEnum("ERROR")]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Error;

    }
}