/*using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WCA.Views;

namespace WCA
{
    public class GenericFontResolver : IFontResolver
    {
        public string DefaultFontName => "OpenSans";

        public byte[] GetFont(string faceName)
        {
            if (faceName.Contains(DefaultFontName))
            {
                var assembly = typeof(JobDetailPage).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream($"PDFDemo.Fonts.{faceName}.ttf");

                using (var reader = new StreamReader(stream))
                {
                    var bytes = default(byte[]);

                    using (var ms = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    return bytes;
                }
            }
            else
                return GetFont(DefaultFontName);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var fontName = string.Empty;

            switch (familyName)
            {
                case "Open Sans":
                case "OpenSans":
                    fontName = "OpenSans";

                    if (isBold && isItalic)
                        fontName = $"{fontName}-BoldItalic";
                    else if (isBold)
                        fontName = $"{fontName}-Bold";
                    else if (isItalic)
                        fontName = $"{fontName}-Italic";
                    else
                        fontName = $"{fontName}-Regular";

                    return new FontResolverInfo(fontName);
                default:
                    break;
            }

            return null;
        }
    }
}
*/