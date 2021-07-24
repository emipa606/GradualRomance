using System.Collections.Generic;
using System.Text;
using Verse;

namespace Gradual_Romance
{
    public static class GRGrammarUtility
    {
        public static string SayList(List<string> items, string conjoiner = "AND", string seperator = "COMMA",
            bool oxfordComma = true)
        {
            if (items.Count == 1)
            {
                return items[0].UncapitalizeFirst();
            }

            if (items.Count == 2)
            {
                return items[0].UncapitalizeFirst() + " " + conjoiner.Translate() + " " + items[1].UncapitalizeFirst();
            }

            var listSize = items.Count;
            var listToSay = new StringBuilder();
            for (var i = 0; i < listSize; i++)
            {
                if (i == 0)
                {
                    listToSay.Append(items[0].UncapitalizeFirst());
                    continue;
                }

                if (i == listSize - 1)
                {
                    listToSay.AppendWithSeparator(items[i].UncapitalizeFirst() + " ", conjoiner.Translate());
                    continue;
                }

                listToSay.AppendWithComma(items[i].UncapitalizeFirst() + " ");
            }

            return listToSay.ToString();
        }
    }
}