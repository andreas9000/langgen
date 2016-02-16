using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace langgen
{
    [Serializable]
    class s1grid : grids
    {
        public static string chars = "abcdefghijklmnopqrstuvwxyzåäö .,-'!?";

        public int[][] map = new int[chars.Length][],
            cMap = new int[chars.Length][];
        int[] sizemap = new int[chars.Length];
        Random r = new Random();
        public int[] biggest = new int[chars.Length];

        public int lang_id, n_id;

        public s1grid(int a, int b)
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
                map[i] = new int[chars.Length];
                cMap[i] = new int[chars.Length];
            }
        }

        public void appendMap(string s)
        {
            int h1, h2;

            for (int i = 0; i < s.Length - 1; i++)
            {
                h1 = testIndexOf(s[i]);
                h2 = testIndexOf(s[i + 1]);

                if (h1 >= 0 && h2 >= 0)// && h1 < chars.Length && h2 < chars.Length)
                {
                    map[h1][h2]++;

                    if (map[h1][h2] > biggest[h1])
                        biggest[h1] = map[h1][h2];
                }
            }
        }
        private int testIndexOf(char s)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == s)
                    return i;
            }
            return -1;
        }
        public string getFirst()
        {
            return null;
        }

        public void computeMap()
        {
            for (int i = 0; i < chars.Length; i++)
            {
                int c = 0;

                for (int j = 0; j < chars.Length; j++)
                {
                    cMap[i][j] += c;
                    c += map[i][j];
                }
                sizemap[i] = c;
            }
        }
        public string getNext(string i)
        {
            return null;
        }

        public int getNextInt(int i)
        {
            int j = r.Next(1, sizemap[i]);

            for (int h = 0; h < chars.Length; h++)
            {
                if (j <= cMap[i][h])
                {
                    return Math.Max(0, h - 1);
                }
            }
            return 0;
        }
    }
}
