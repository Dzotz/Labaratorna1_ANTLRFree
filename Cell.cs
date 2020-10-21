using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labaratorna1_ANTLRFree
{
    public class Cell : DataGridViewTextBoxCell
    {
        string name;
        string val;
        string exp = "";
        List<string> dep = new List<string>();
        public string Value { get { return val; } set { val = value; } }
        public string Name { get { return name; } }
        public string Exp { get { return exp; } set { exp = value; } }
        public List<string> Depends { get { return dep; } set { dep = value; } }
    }
}
