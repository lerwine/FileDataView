using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDataView
{
    public class ByteBlock
    {
        public byte[] Data { get; }

        public string DWord0 { get; }

        public string DWord1 { get; }

        public string DWord2 { get; }

        public string DWord3 { get; }

        private ByteBlock(byte[] data)
        {
            Data = data;
            int e = data.Length;
            StringBuilder sb = new StringBuilder();
            sb.Append(data[0].ToString("x2"));
            if (e < 8)
            {
                for (int i = 1; i < e; i++)
                    sb.Append(" ").Append(data[i].ToString("x2"));
                DWord0 = sb.ToString();
                DWord1 = DWord2 = DWord3 = "";
                return;
            }
            
            for (int i = 1; i < 8; i++)
                sb.Append(" ").Append(data[i].ToString("x2"));
            DWord0 = sb.ToString();
            sb.Clear();
            sb.Append(data[8].ToString("x2"));
            if (e < 17)
            {
                for (int i = 9; i < e; i++)
                    sb.Append(" ").Append(data[i].ToString("x2"));
                DWord1 = sb.ToString();
                DWord2 = DWord3 = "";
                return;
            }
            for (int i = 9; i < 16; i++)
                sb.Append(" ").Append(data[i].ToString("x2"));
            DWord1 = sb.ToString();
            sb.Clear();
            sb.Append(data[16].ToString("x2"));
            if (e < 25)
            {
                for (int i = 17; i < e; i++)
                    sb.Append(" ").Append(data[i].ToString("x2"));
                DWord2 = sb.ToString();
                DWord3 = "";
                return;
            }
            for (int i = 17; i < 24; i++)
                sb.Append(" ").Append(data[i].ToString("x2"));
            DWord2 = sb.ToString();
            sb.Clear();
            sb.Append(data[24].ToString("x2"));
            for (int i = 25; i < e; i++)
                sb.Append(" ").Append(data[i].ToString("x2"));
            DWord3 = sb.ToString();
        }

        public static void Load(FileInfo file, BindingList<ByteBlock> list)
        {
            list.Clear();
            if (null == file || !file.Exists)
                return;
            using (Stream stream = file.OpenRead())
            {
                byte[] buffer = new byte[32];
                int count = stream.Read(buffer, 0, 32);
                while (count == 32)
                {
                    list.Add(new ByteBlock(buffer));
                    count = stream.Read(buffer, 0, 32);
                }
                if (count > 0)
                {
                    byte[] data = new byte[count];
                    for (int i = 0; i < count; i++)
                        data[i] = buffer[i];
                    list.Add(new ByteBlock(data));
                }
            }
        }
    }
}