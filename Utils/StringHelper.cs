using System.Globalization;
using System.Linq;
using System.Text;

namespace Utils
{
    public class StringHelper
    {
        public static string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.ToLower().Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }
    }
}
