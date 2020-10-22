using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Labaratorna1_ANTLRFree
{
    public class Parser
    {
        enum Types
        {
            NONE,
            DELIMITER,
            NUMBER
        }

        string s = "";
        string exp;
        public string Expr { set { exp = value; } get { return exp; } }
        int expID;
        string token;
        public string Token { get { return token; } }
        Types tokenType;
        public string err = "";
        public Parser()
        {

        }

        bool LastLexChecker(int len)
        {
            return (exp[len - 1].Equals('+') || exp[len - 1].Equals('-') || exp[len - 1].Equals('/') || exp[len - 1].Equals('*') || exp[len - 1].Equals('^') || exp[len - 1].Equals('('));
        }

        public double ExpressionStart(string expression)
        {
            err = "";
            double result;
            exp = expression;
            int l = exp.Length;
            if(exp == "")
            {
                err = "no expression";
                return 0.0;

            }
            if (LastLexChecker(l))
            {
                MessageBox.Show("Wrong last lex: Last lexema should be an expression");
                err = "Last lexem should be an expression";
            }
            expID = 0;
            try
            {
                GetToken();
                if (token == "")
                {
                    err = "no expression";
                    return 0.0;
                }
                ExpSravnit(out result);
                if (token != ""&&token != "]")
                {
                    err = "last lexema should be 0";
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0.0;
            }
        }

        void ExpSravnit(out double result)
        {
            string op;
            double partRes;

            ExpPlusMinus(out result);
            while ((op = token) == ">" || op == "<" || op == "<=" || op == "=" || op == ">=" || op == "<>")
            {
                GetToken();
                ExpPlusMinus(out partRes);
                switch (op)
                {
                    case ">":
                        if (result > partRes) { result = 1.0; }
                        else result = 0.0;
                        break;
                    case "<":
                        if (result < partRes) { result = 1.0; }
                        else result = 0.0;
                        break;
                    case "=":
                        if (result == partRes) { result = 1.0; }
                        else result = 0.0;
                        break;
                    case ">=":
                        if (result >= partRes) { result = 1.0; }
                        else result = 0.0;
                        break;
                    case "<=":
                        if (result <= partRes) { result = 1.0; }
                        else result = 0.0;
                        break;
                    case "<>":
                        if (result != partRes) { result = 1.0; }
                        else result = 0.0;
                        break;
                }
            }
        }

        void ExpPlusMinus(out double result)
        {
            string op;
            double partRes;

            ExpMultiplyDivision(out result);
            
            while ((op = token) == "+" || op == "-")
            {
                GetToken();
                ExpMultiplyDivision(out partRes);
                switch (op)
                {
                    case "-":
                        result = result - partRes;
                        break;
                    case "+":
                        result = result + partRes;
                        break;
                }
            }
        }

        void ExpMultiplyDivision(out double result)
        {
            string op;
            double partRes = 0.0;
            ExpStepin(out result);
            while ((op = token) == "*" || op == "/")
            {
                GetToken();
                ExpStepin(out partRes);
                switch (op)
                {
                    case "*":
                        result = result * partRes;
                        break;
                    case "/":
                        if (partRes == 0.0)
                        {
                            MessageBox.Show("divide by zero");
                            err = "invalid expression (divide by zero)";
                        }
                        else result = result / partRes;
                        break;
                }
            }
        }

        void ExpStepin(out double result)
        {
            double partRes, karman;
            int iter;
            ExpMaxMin(out result);
            if (token == "^")
            {
                GetToken();
                ExpMaxMin(out partRes);
                karman = result;
                if (partRes == 0.0)
                {
                    result = 1.0;
                    return;
                }
                for (iter = (int)partRes - 1; iter > 0; iter--)
                {
                    result = result * karman;
                }
            }
        }

        void ExpMaxMin(out double result)
        {
            double partRes = 0.0;
            string op;
            //GetToken();
            ExpDugka(out result);
            while ((op=token) == "max[" || op == "min[")
            {
                GetToken();
                ExpDugka(out result);
                if (token == ",")
                {
                    GetToken();
                    ExpDugka(out partRes);
                }
                else
                {
                    MessageBox.Show("expected ,");
                    err = "invalid expression (expected ,)";
                }
                if (token != "]")
                {
                    MessageBox.Show("expected ]");
                    err = "invalid expression (expected ])";
                }
                switch (op)
                {
                    case "max[":
                        if (result < partRes)
                        {
                            result = partRes;
                        }
                        break;
                    case "min[":
                        if (result > partRes)
                        {
                            result = partRes;
                        }
                        break;
                }
            }
        }


        void ExpDugka(out double result)
        {
            if (token == "(")
            {
                GetToken();
                ExpSravnit(out result);
                if (token == "]")
                {
                    GetToken();
                }
                if (token != ")")
                {
                        MessageBox.Show("Unbalansed parens");
                        err = "invalid expression (unbalanced parens)";
                }
                GetToken();
            }
            else Atom(out result);
        }

        void Atom(out double result)
        {
            switch (tokenType)
            {
                case Types.NUMBER:
                    try
                    {
                        result = Double.Parse(token);
                    }
                    catch (FormatException)
                    {
                        result = 0.0;
                        MessageBox.Show("Syntax error");
                        err = "syntax error";
                    }
                    GetToken();
                    return;
                default:
                    result = 0.0;
                    break;
            }
        }

        public void GetToken()
        {
            tokenType = Types.NONE;
            token = "";
            if (expID == exp.Length) return;
            while (expID < exp.Length && Char.IsWhiteSpace(exp[expID])) expID++;
            if (expID == exp.Length) return;
            if (IsDelim(exp[expID]))
            {
                token += exp[expID];
                tokenType = Types.DELIMITER;
                expID++;
               
                if (token == "m")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        token += exp[expID];
                        expID++;
                    }
                }
                if (expID < exp.Length)
                {
                    if ((token == "<"||token==">" )&&(exp[expID] == '=' || exp[expID] == '<' || exp[expID] == '>'))
                    {
                        token += exp[expID];
                        expID++;
                    }
                }

            }
            else if (Char.IsDigit(exp[expID]))
            {
                while (!IsDelim(exp[expID]))
                {
                    token += exp[expID];
                    expID++;
                    if (expID >= exp.Length) break;
                }
                tokenType = Types.NUMBER;
            }
        }

        bool IsDelim(char c)
        {
            if ("+-*/^<>()=m,]".IndexOf(c) != -1)
            {
                return true;
            }
            return false;
        }
    }
}
