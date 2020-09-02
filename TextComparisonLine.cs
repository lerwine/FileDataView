using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDataView
{
    [DataObject]
    public class TextComparisonLine
    {
        [DataObjectField(true, false, false)]
        public int LineNumber { get; }

        [DataObjectField(false, false, false)]
        public int Length { get; }

        [DataObjectField(false, false, false)]
        public string Text { get; }

        private TextComparisonLine(int lineNumber, int length, String text)
        {
            LineNumber = lineNumber;
            Text = text;
            Length = length;
        }

        public static void FillSimple(FileInfo fileA, FileInfo fileB, BindingList<TextComparisonLine> list, out Encoding encodingA, out Encoding encodingB)
        {
            list.Clear();
            int lineNumber = 0;
            string lineB;
            if (null != fileA && fileA.Exists)
            {
                using (StreamReader readerA = fileA.OpenText())
                {
                    string lineA = readerA.ReadLine();
                    if (null != fileB && fileB.Exists)
                    {
                        using (StreamReader readerB = fileB.OpenText())
                        {
                            while (null != lineA)
                            {
                                lineB = readerB.ReadLine();
                                if (null == lineB)
                                {
                                    do
                                    {
                                        list.Add(new TextComparisonLine(++lineNumber, lineA.Length, lineA));
                                        list.Add(new TextComparisonLine(-1, -1, ""));
                                    } while (null != (lineA = readerA.ReadLine()));
                                    encodingA = readerA.CurrentEncoding;
                                    encodingB = readerB.CurrentEncoding;
                                    return;
                                }
                                list.Add(new TextComparisonLine(++lineNumber, lineA.Length, lineA));
                                list.Add(new TextComparisonLine(lineNumber, lineB.Length, lineB));
                            }
                            while (null != (lineB = readerB.ReadLine()))
                            {
                                list.Add(new TextComparisonLine(-1, -1, ""));
                                list.Add(new TextComparisonLine(++lineNumber, lineB.Length, lineB));
                            }
                            encodingB = readerB.CurrentEncoding;
                        }
                    }
                    else
                    {
                        encodingB = null;
                        if (null != lineA)
                        {
                            do
                            {
                                list.Add(new TextComparisonLine(++lineNumber, lineA.Length, lineA));
                                list.Add(new TextComparisonLine(-1, -1, ""));
                            } while (null != (lineA = readerA.ReadLine()));
                        }
                        else
                            encodingB = encodingA = Encoding.Default;
                    }
                    encodingA = readerA.CurrentEncoding;
                }
            }
            else
            {
                encodingA = null;
                if (null != fileB && fileB.Exists)
                {
                    using (StreamReader readerB = fileB.OpenText())
                    {
                        while (null != (lineB = readerB.ReadLine()))
                        {
                            list.Add(new TextComparisonLine(-1, -1, ""));
                            list.Add(new TextComparisonLine(++lineNumber, lineB.Length, lineB));
                        }
                        encodingB = readerB.CurrentEncoding;
                    }
                }
                else
                    encodingB = null;
            }
        }
    
        public static string ToEncodedText(string text)
        {
            if (text.Length == 0)
                return "\"\"";
            StringBuilder sb = new StringBuilder("\"");
            foreach (char c in text.ToCharArray())
            {
                switch (c)
                {
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\v':
                        sb.Append("«VT»");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\u0000':
                        sb.Append("«NUL»");
                        break;
                    case '\u0001':
                        sb.Append("«SOH»");
                        break;
                    case '\u0002':
                        sb.Append("«STX»");
                        break;
                    case '\u0003':
                        sb.Append("«ETX»");
                        break;
                    case '\u0004':
                        sb.Append("«EOT»");
                        break;
                    case '\u0005':
                        sb.Append("«ENQ»");
                        break;
                    case '\u0006':
                        sb.Append("«ACK»");
                        break;
                    case '\u000E':
                        sb.Append("«SO»");
                        break;
                    case '\u000F':
                        sb.Append("«SI»");
                        break;
                    case '\u0010':
                        sb.Append("«DLE»");
                        break;
                    case '\u0011':
                        sb.Append("«DC1»");
                        break;
                    case '\u0012':
                        sb.Append("«DC2»");
                        break;
                    case '\u0013':
                        sb.Append("«DC3»");
                        break;
                    case '\u0014':
                        sb.Append("«DC4»");
                        break;
                    case '\u0015':
                        sb.Append("«NAK»");
                        break;
                    case '\u0016':
                        sb.Append("«SYN»");
                        break;
                    case '\u0017':
                        sb.Append("«ETB»");
                        break;
                    case '\u0018':
                        sb.Append("«CAN»");
                        break;
                    case '\u0019':
                        sb.Append("«EM»");
                        break;
                    case '\u001A':
                        sb.Append("«SUB»");
                        break;
                    case '\u001B':
                        sb.Append("«ESC»");
                        break;
                    case '\u001C':
                        sb.Append("«FS»");
                        break;
                    case '\u001D':
                        sb.Append("«CS»");
                        break;
                    case '\u001E':
                        sb.Append("«RS»");
                        break;
                    case '\u001F':
                        sb.Append("«US»");
                        break;
                    case ' ':
                        sb.Append(" ");
                        break;
                    case '\u0082':
                        sb.Append("«BPH»");
                        break;
                    case '\u0083':
                        sb.Append("«NBH»");
                        break;
                    case '\u0084':
                        sb.Append("«IND»");
                        break;
                    case '\u0085':
                        sb.Append("«NEL»");
                        break;
                    case '\u0086':
                        sb.Append("«SSA»");
                        break;
                    case '\u0087':
                        sb.Append("«ESA»");
                        break;
                    case '\u0088':
                        sb.Append("«HTS»");
                        break;
                    case '\u0089':
                        sb.Append("«HTJ»");
                        break;
                    case '\u008A':
                        sb.Append("«VTS»");
                        break;
                    case '\u008B':
                        sb.Append("«PLD»");
                        break;
                    case '\u008C':
                        sb.Append("«PLU»");
                        break;
                    case '\u008D':
                        sb.Append("«RI»");
                        break;
                    case '\u008E':
                        sb.Append("«SS2»");
                        break;
                    case '\u008F':
                        sb.Append("«SS3»");
                        break;
                    case '\u0090':
                        sb.Append("«DCS»");
                        break;
                    case '\u0091':
                        sb.Append("«PU1»");
                        break;
                    case '\u0092':
                        sb.Append("«PU2»");
                        break;
                    case '\u0093':
                        sb.Append("«STS»");
                        break;
                    case '\u0094':
                        sb.Append("«CCH»");
                        break;
                    case '\u0095':
                        sb.Append("«MW»");
                        break;
                    case '\u0096':
                        sb.Append("«SPA»");
                        break;
                    case '\u0097':
                        sb.Append("«EPA»");
                        break;
                    case '\u0098':
                        sb.Append("«SOS»");
                        break;
                    case '\u009A':
                        sb.Append("«SCI»");
                        break;
                    case '\u009B':
                        sb.Append("«CSI»");
                        break;
                    case '\u009C':
                        sb.Append("«ST»");
                        break;
                    case '\u009D':
                        sb.Append("«OCS»");
                        break;
                    case '\u009E':
                        sb.Append("«PM»");
                        break;
                    case '\u009F':
                        sb.Append("«APC»");
                        break;
                    case '\u00A0':
                        sb.Append("«NBSP»");
                        break;
                    case '\u2028':
                        sb.Append("«LSEP»");
                        break;
                    case '\u2029':
                        sb.Append("«PSEP»");
                        break;
                    case '«':
                        sb.Append("««»");
                        break;
                    case '»':
                        sb.Append("«»»");
                        break;
                    default:
                        if (Char.IsControl(c) || Char.IsWhiteSpace(c))
                            sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else
                            sb.Append(c);
                        break;
                }
            }
            return sb.Append("\"").ToString();
        }

        public static void FillEncoded(FileInfo fileA, FileInfo fileB, BindingList<TextComparisonLine> list, out Encoding encodingA, out Encoding encodingB)
        {
            list.Clear();
            int lineNumber = 0;
            string lineB;
            if (null != fileA && fileA.Exists)
            {
                using (StreamReader readerA = fileA.OpenText())
                {
                    string lineA = readerA.ReadLine();
                    if (null != fileB && fileB.Exists)
                    {
                        using (StreamReader readerB = fileB.OpenText())
                        {
                            while (null != lineA)
                            {
                                lineB = readerB.ReadLine();
                                if (null == lineB)
                                {
                                    do
                                    {
                                        list.Add(new TextComparisonLine(++lineNumber, lineA.Length, ToEncodedText(lineA)));
                                        list.Add(new TextComparisonLine(-1, -1, ""));
                                    } while (null != (lineA = readerA.ReadLine()));
                                    encodingA = readerA.CurrentEncoding;
                                    encodingB = readerB.CurrentEncoding;
                                    return;
                                }
                                list.Add(new TextComparisonLine(++lineNumber, lineA.Length, ToEncodedText(lineA)));
                                list.Add(new TextComparisonLine(lineNumber, lineB.Length, ToEncodedText(lineB)));
                            }
                            while (null != (lineB = readerB.ReadLine()))
                            {
                                list.Add(new TextComparisonLine(-1, -1, ""));
                                list.Add(new TextComparisonLine(++lineNumber, lineB.Length, ToEncodedText(lineB)));
                            }
                            encodingB = readerB.CurrentEncoding;
                        }
                    }
                    else
                    {
                        encodingB = null;
                        if (null != lineA)
                        {
                            do
                            {
                                list.Add(new TextComparisonLine(++lineNumber, lineA.Length, ToEncodedText(lineA)));
                                list.Add(new TextComparisonLine(-1, -1, ""));
                            } while (null != (lineA = readerA.ReadLine()));
                        }
                        else
                            encodingB = encodingA = Encoding.Default;
                    }
                    encodingA = readerA.CurrentEncoding;
                }
            }
            else
            {
                encodingA = null;
                if (null != fileB && fileB.Exists)
                {
                    using (StreamReader readerB = fileB.OpenText())
                    {
                        while (null != (lineB = readerB.ReadLine()))
                        {
                            list.Add(new TextComparisonLine(-1, -1, ""));
                            list.Add(new TextComparisonLine(++lineNumber, lineB.Length, ToEncodedText(lineB)));
                        }
                        encodingB = readerB.CurrentEncoding;
                    }
                }
                else
                    encodingB = null;
            }
        }
    }
}