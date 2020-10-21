using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labaratorna1_ANTLRFree
{
    class Converter26
    {
        public Converter26() { }

        public static string To26(int i)
        {
            int k = 0;
            int[] Arr = new int[100];
            while (i > 25)
            {
                Arr[k] = i / 26 - 1;
                k++;
                i = i % 26;
            }
            Arr[k] = i;

            string res = "";
            for (int j = 0; j <= k; j++)
            {
                res = res + ((char)('A' + Arr[j])).ToString();
            }
            return res;
        }

        public static int From26(string header)
        {
            char[] chArr = header.ToCharArray();
            int l = chArr.Length;
            int res = 0;
            for (int i = l - 2; i >= 0; i--)
            {
                res = res + (((int)chArr[i] - (int)'A' + 1) * Convert.ToInt32(Math.Pow(26, l - i - 1)));
            }
            res = res + ((int)chArr[l - 1] - (int)'A');
            return res;
        }

    }
}
