using System.Collections.Generic;
using System.Text;

namespace TRTextProcessingClassLibrary.Tool
{
    public class StringDivideClass
    {
        public static string[] StringDivide(string sss)
        {
            // 双引号开始标记
            int qutationStart = 0;
            int qutationEnd = 0;
            char[] charStr = sss.ToCharArray();
            // 用于拼接字符 作为一个字段值
            StringBuilder stb = new StringBuilder();
            // 结果list
            List<string> list = new List<string>();
            int lastindex = sss.LastIndexOf('\"');
            // 逐个字符处理
            for (int i = 0; i < charStr.Length; i++)
            {
                // 在此之前还未遇到双引号并且当前的字符为"
                if (qutationStart == 0 && charStr[i] == '\"')
                {
                    qutationStart = 1;
                    continue;
                }

                if (qutationStart == 1 && charStr[i] == '\"')
                {
                    // 在此之前遇到了双引号并且当前的字符为" 说明字段拼接可能要结束了
                    qutationEnd = 1;
                    // 当最后一个字符是双引号时，由于下次循环不会执行，所以在此保存一下
                    if (i == charStr.Length - 1)
                    {
                        if (stb.Length != 0)
                        {
                            list.Add(stb.ToString());
                            stb.Clear();
                        } 
                        else
                        {
                            list.Add(string.Empty);
                            stb.Clear();
                        }

                    }
                    continue;
                }

                if (qutationEnd == 1 && charStr[i] != ',')
                {
                    // 在此之前遇到了第二个双引号并且当前的字符不为,说明字段拼接没有结束，是双重引号
                    qutationEnd = 0;
                    stb.Append('\"');
                    stb.Append(charStr[i]);

                    continue;
                }

                if (qutationEnd == 1 && charStr[i] == ',')
                {
                    // 在此之前遇到了第二个双引号并且当前的字符为,说明字段拼接该结束了
                    qutationStart = 0;
                    qutationEnd = 0;

                    list.Add(stb.ToString());
                    stb.Clear();

                    continue;
                }

                if (qutationStart == 1 && charStr[i] == ',')
                {
                    // 处理 \"中国,北京\"这种不规范的字符串
                    stb.Append(charStr[i]);
                    continue;
                }

                if (i == lastindex)
                {
                    continue;
                }

                if (charStr[i] == ',')
                {
                    // 字段结束，将拼接好的字段值存到list中
                    list.Add(stb.ToString());
                    stb.Clear();
                    continue;
                }

                // 不属于分隔符的就拼接
                stb.Append(charStr[i]);

                if (i == charStr.Length - 1 && stb.Length != 0)
                {
                    list.Add(stb.ToString());
                    stb.Clear();
                }
            }

            if (sss.EndsWith(","))
            {
                list.Add(string.Empty);
            }

            return list.ToArray();
        }
    }
}
