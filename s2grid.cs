using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace langgen
{
    [Serializable]
        public class ns2grid : grids
        {
            public static string chars = "abcdefghijklmnopqrstuvwxyzåäö .,-'!?";
            static int le = (int)Math.Pow(chars.Length, 2);

            public int[][] map = new int[le][],
                cMap = new int[le][];
            public int[] sizemap = new int[le];
            Random r = new Random();
            public int[] biggest = new int[36 * 36];

            public int lang_id, n_id;

            public ns2grid(int a, int b)
            {
                lmap();
                lang_id = a;
                n_id = b;
            }
            public int getn()
            {
                return n_id;
            }
            public int getlang()
            {
                return lang_id;
            }
            private void lmap()
            {
                for (int i = 0; i < map.Length; i++)
                {
                    map[i] = new int[le];
                    cMap[i] = new int[le];
                }
            }

            public void appendMap(string s)
            {
                int h1, h2;
                s = clean(s);


                for (int i = 0; i < s.Length - 3; i++)
                {
                    h1 = iValue(s[i], s[i + 1]);
                    h2 = iValue(s[i + 2], s[i + 3]);

                    if (h1 >= 0 && h2 >= 0)// && h1 < chars.Length && h2 < chars.Length)
                    {
                        map[h1][h2]++;

                        if (map[h1][h2] > biggest[h1])
                            biggest[h1] = map[h1][h2];
                    }

                }
            }
            public static string clean(string s)
            {
                string s2 = "";
                foreach (char t in s)
                    if (s1grid.chars.IndexOf(t) != -1)
                        s2 += t;
                s = s2;

                return s2;
            }
            public int testIndexOf(char s)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i] == s)
                        return i;
                }
                return -1;
            }
            public int iValue(char s1, char s2)
            {
                return testIndexOf(s1) * (chars.Length) + testIndexOf(s2);
            }

            public void computeMap()
            {
                for (int i = 0; i < le; i++)
                {
                    int c = 0;

                    for (int j = 0; j < le; j++)
                    {
                        cMap[i][j] += c;
                        c += map[i][j];
                    }
                    sizemap[i] = c;
                }
            }
            public string getFirst()
            {
                return null;
            }

            public int getNextInt(int i)
            {
                if (sizemap[i] == 0)
                    return 0;

                int j = r.Next(1, sizemap[i]);

                for (int h = 0; h < le; h++)
                {
                    if (j <= cMap[i][h])
                    {
                        return Math.Max(0, h - 1);
                    }
                }
                return -1;
            }
            public string getNext(string i)
            {
                return null;
            }
        }
}
