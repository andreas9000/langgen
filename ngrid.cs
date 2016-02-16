using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace langgen
{
    [Serializable]
    class ngrid : grids
    {
        public List<List<int>> map = new List<List<int>>(),
            map2 = new List<List<int>>();
        public List<int> cmap = new List<int>();
        public List<string> map_id = new List<string>();
        public List<List<string>> nestedMap = new List<List<string>>();
        Random r = new Random();
        public int n;

        public int lang_id, n_id;

        public ngrid(int a, int b)
        {
            lang_id = a;
            n_id = b;
            n = b;
        }
        public int getn()
        {
            return n_id;
        }
        public int getlang()
        {
            return lang_id;
        }
        public string getFirst()
        {
            return nestedMap[0][0];
        }
        public void appendMap(string s)
        {

            s = ns2grid.clean(s);

            for (int i = 0; i < s.Length - 2 * n_id; i++)
            {
                int i1 = getIndexOrAdd(s.Substring(i, n_id)),
                    i2 = getIndexOrAddInner(s.Substring(i+n_id,n_id), i1);

                map[i1][i2]++;

                //map[getIndexOrAdd(s.Substring(i, n))][getIndexOrAdd(s.Substring(i + n, n))]++;
            }
        }
        int getIndexOrAdd(string s)
        {
            int r = map_id.IndexOf(s);

            if (r != -1)
                return r;

            map_id.Add(s);
            map.Add(new List<int>());
            nestedMap.Add(new List<string>());
            return map_id.Count - 1;
        }

        public void computeMap()
        {
            for (int i = 0; i < map.Count; i++)
            {
                int h = 0;

                for (int k = 0; k < map[i].Count; k++)
                {
                    h += map[i][k];
                    map[i][k] = h;
                }
                cmap.Add(h);
            }
        }

        int getIndexOrAddInner(string s, int i)
        {
            int r = nestedMap[i].IndexOf(s);

            if (r != -1)
                return r;

            nestedMap[i].Add(s);
            map[i].Add(0);
            return nestedMap[i].Count - 1;
        }

        public double prediction(string s1, string s2)
        {
            int id1 = map_id.IndexOf(s1);

            if (id1 == -1)
                return 0;

               int size = cmap[id1],
                id2 = nestedMap[id1].IndexOf(s2);

            if (id2 == -1)
                return 0;

                int part = id2 == 0 ? map[id1][id2] : 
                    map[id1][id2] - map[id1][id2-1];

                double r = (double)((double)part / (double)size);

                return r;
        }

        public string getNext(string i)
        {
            int h1 = map_id.IndexOf(i),
                max = r.Next(1, cmap[h1]);
            
            
            for (int j = 0; j < map[h1].Count; j++)
            {
                if (max <= map[h1][j])
                {
                    return nestedMap[h1][j];
                }
            }
            return "err";
        }

        public int getNextInt(int i)
        {
            return -1;
        }

        public int iValue(char[] inp)
        {
            int r = 0;

            for (int i = 0; i < inp.Length; i++)
            {
                r += s1grid.chars.IndexOf(inp[i]) * (int)Math.Pow(s1grid.chars.Length, i);
            }

            return r;
        }
    }
}
