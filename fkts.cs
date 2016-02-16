using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace langgen
{
    class fkts
    {
        public static int chiffer(string s, ns2grid n)
        {
            s = s.ToLower();
            double l = 0;
            int r = 0;

            for (int i = 0; i < 29; i++)
            {
                double h = (avg_p2(nshift(s, i), n));
                if (h > l)
                { l = h; r = i; }
            }
            return r;
        }

        public static string nshift(string s, int n)
        {
            string r = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s1grid.chars.IndexOf(s[i]) < 29)
                    r += s1grid.chars[(s1grid.chars.IndexOf(s[i]) - n + 29) % 29];
                else r += s[i];
            }
            return r;
        }

        public static double pred_c(string s, grids n)
        {
            if (n.getn() == 2)
                return avg_p2(s, (ns2grid)n);
            else if (n.getn() > 2)
                return avg_pn(s, (ngrid)n);
            return 0;
        }
        public static string blanc(string s)
        {
            string s2 = "";
            foreach (char t in s)
                if (s1grid.chars.IndexOf(t) <= 28)
                    s2 += t;
            return s2;
        }


        public static double avg_p2(string s, ns2grid n, bool rblanc = false)
        {
            double r = 0, rr;
            s = s.ToLower();
            if (rblanc)
            {
                s = blanc(s);
            }

            // int c = s.Length - 2;
            int c = 0;

            for (int i = 0, j, k; i < s.Length - 3; i++)
            {
                j = n.iValue(s[i], s[i + 1]);
                k = n.iValue(s[i + 2], s[i + 3]);

                if (j < 0 || k < 0 || s1grid.chars.IndexOf(s[i]) >= 30)
                {
                    //c--;
                    continue;
                }
                rr = (double)(n.map[j][k]) / (double)(n.sizemap[j]);
                if (!Double.IsNaN(rr))
                {
                    r += rr;
                    c++;
                }
            }

            return (r / c) * 100;
        }

        public static double avg_pn(string s, ngrid n, bool rblanc = false)
        {
            double r = 0, rr;
            s = s.ToLower();
            if (rblanc)
            {
                s = blanc(s);
            }

            int c = s.Length;

            for (int i = 0; i < s.Length - (2 * n.n); i++)
            {
                rr = n.prediction(s.Substring(i, n.n), (s.Substring(i + n.n, n.n)));
                if (!Double.IsNaN(rr))
                    r += rr;
                else
                    c--;
            }
            double ret = (double)((double)r / (double)s.Length * 100);

            return ret;
        }
    }
}
