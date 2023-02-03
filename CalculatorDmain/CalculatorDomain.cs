using AngouriMath;
using System.Data;
using System.Text;

namespace CalculatorDmain
{
    public class CalculatorDomain : ICalculatorDomain
    {
        public async Task<string> Calculate(string expression)
        {
            double result = 0;
            if (!string.IsNullOrWhiteSpace(expression))
            {
                Entity e = expression;
                var result1 = (double)e.EvalNumerical();
                result = result1;
                //result = Evaluate(expression);

            }
            return await Task.FromResult(result.ToString());
        }
        public double Evaluate(string expr)
        {
            Stack<String> stack = new Stack<String>();
            expr = PreProcessExpression(expr);
            string value = "";
            for (int i = 0; i < expr.Length; i++)
            {
                String s = expr.Substring(i, 1);
                char chr = s.ToCharArray()[0];

                if (!char.IsDigit(chr) && chr != '.' && value != "")
                {
                    stack.Push(value);
                    value = "";
                }

                if (s.Equals("("))
                {

                    string innerExp = "";
                    i++; //Fetch Next Character
                    int bracketCount = 0;
                    for (; i < expr.Length; i++)
                    {
                        s = expr.Substring(i, 1);

                        if (s.Equals("("))
                            bracketCount++;

                        if (s.Equals(")"))
                            if (bracketCount == 0)
                                break;
                            else
                                bracketCount--;


                        innerExp += s;
                    }

                    stack.Push(Evaluate(innerExp).ToString());

                }
                else if (s.Equals("+")) stack.Push(s);
                else if (s.Equals("-")) stack.Push(s);
                else if (s.Equals("*")) stack.Push(s);
                else if (s.Equals("/")) stack.Push(s);
                else if (s.Equals("sqrt")) stack.Push(s);
                else if (s.Equals(")"))
                {
                }
                else if (char.IsDigit(chr) || chr == '.')
                {
                    value += s;

                    if (value.Split('.').Length > 2)
                        throw new Exception("Invalid decimal.");

                    if (i == (expr.Length - 1))
                        stack.Push(value);

                }
                else
                    throw new Exception("Invalid character.");

            }


            double result = 0;
            while (stack.Count >= 3)
            {

                double right = Convert.ToDouble(stack.Pop());
                string op = stack.Pop();
                double left = Convert.ToDouble(stack.Pop());

                if (op == "+") result = left + right;
                else if (op == "+") result = left + right;
                else if (op == "-") result = left - right;
                else if (op == "*") result = left * right;
                else if (op == "/") result = left / right;

                stack.Push(result.ToString());
            }


            return Convert.ToDouble(stack.Pop());
        }

        private string PreProcessExpression(string query)
        {
            string expr = "  ";
            List<string> digits = new List<string> { "0", "1","2", "3", "4", "5", "6", "7", "8", "9" };
            List<string> symbols = new List<string> { "+", "-", "*", "/" };
            for (int i = 0; i < query.Length; i++)
            {
                String s = query.Substring(i, 1);
                char chr = s.ToCharArray()[0];
                var lastChar = expr.ToString().Substring(expr.Length - 1, 1);
                if (s.Equals("(") && !symbols.Contains(lastChar))
                {
                    expr = expr + "*";
                }
                if(lastChar==")" && digits.Contains(s))
                {
                    expr = expr + "*";
                }
                expr = expr + s;

            }
            return expr.Trim();
        }
    }
}
