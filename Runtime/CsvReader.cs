using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gilzoide.SqliteAsset.Csv
{
    public static class CsvReader
    {
        public enum SeparatorChar
        {
            Comma,
            Semicolon,
            Tabs,
        }

        public static IEnumerable<string> ParseStream(StreamReader stream, SeparatorChar separator = SeparatorChar.Comma, int maxFieldSize = int.MaxValue)
        {
            SkipEmptyLines(stream);
            if (stream.Peek() < 0)
            {
                yield break;
            }

            bool insideQuotes = false;
            var stringBuilder = new StringBuilder();
            while (true)
            {
                int c = stream.Read();
                switch (c)
                {
                    case '\r':
                        if (!insideQuotes && stream.Peek() == '\n')
                        {
                            stream.Read();
                            goto case '\n';
                        }
                        else
                        {
                            goto default;
                        }

                    case '\n':
                        if (!insideQuotes)
                        {
                            yield return stringBuilder.ToString();
                            stringBuilder.Clear();
                            yield return null;

                            SkipEmptyLines(stream);
                            if (stream.Peek() < 0)
                            {
                                yield break;
                            }
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    case ',':
                        if (!insideQuotes && separator == SeparatorChar.Comma)
                        {
                            yield return stringBuilder.ToString();
                            stringBuilder.Clear();
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    case ';':
                        if (!insideQuotes && separator == SeparatorChar.Semicolon)
                        {
                            yield return stringBuilder.ToString();
                            stringBuilder.Clear();
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    case '\t':
                        if (!insideQuotes && separator == SeparatorChar.Tabs)
                        {
                            yield return stringBuilder.ToString();
                            stringBuilder.Clear();
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    case '"':
                        if (insideQuotes && stream.Peek() == '"')
                        {
                            stream.Read();
                            goto default;
                        }
                        else
                        {
                            insideQuotes = !insideQuotes;
                        }
                        break;

                    case < 0:
                        yield return stringBuilder.ToString();
                        yield return null;
                        yield break;

                    default:
                        if (stringBuilder.Length >= maxFieldSize)
                        {
                            throw new CsvException("Field size is greater than maximum allowed size.");
                        }
                        stringBuilder.Append((char) c);
                        break;
                }
            }
        }

        private static void SkipEmptyLines(StreamReader streamReader)
        {
            while (true)
            {
                int c = streamReader.Peek();
                switch (c)
                {
                    case '\n':
                    case '\r':
                        streamReader.Read();
                        continue;

                    default:
                        return;
                }
            }
        }
    }
}
