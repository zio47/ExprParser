using System;
using System.Collections.Generic;

namespace ExpressionParser
{
    public class SumParser
    {
        static void Main(string[] args)
        {
            SumParser sumParser = new SumParser();
            int sumResult = 0;
            string sum = "4 + (12 /(1 * 2))";
            bool parseResult = sumParser.EvaluateExpression(sum, out sumResult);
            Console.WriteLine(parseResult + " " + sumResult);
        }

        public bool EvaluateExpression(string expression, out int result)
        {
            bool valid = true;
            result = 0;
            bool done = false;

            foreach (char c in expression)
            {
                // If there are any letters or an odd number of matching brackets the expression is not valid
                if (Char.IsLetter(c) || expression.Split('(').Length != expression.Split(')').Length)
                {
                    result = 0;
                    return false;
                }

            }

            while (!done)
            {
                string sumToCheck = expression;

                // If there are brackets...
                if (expression.LastIndexOf('(') != -1)
                {
                    // Get substring to the first closing bracket and then evaluate what is inside the brackets 
                    int firstRightBracket = expression.IndexOf(')');
                    string toBracket = expression.Substring(0, firstRightBracket);
                    int lastLeftBracket = toBracket.LastIndexOf('(');
                    sumToCheck = toBracket.Substring(lastLeftBracket + 1, firstRightBracket - lastLeftBracket - 1);
                }

                List<string> parts = SplitExpression(sumToCheck);

                valid = IsValid(parts);

                if (valid)
                {
                    result = CalculateSum(parts, out valid);
                }
                else
                {
                    result = 0;
                    break;
                }

                if (expression.LastIndexOf('(') != -1)
                {
                    // Replace the part in brackets with the result 
                    string removeSum = "(" + sumToCheck + ")";
                    string nextSum = expression.Replace(removeSum, result.ToString());
                    expression = nextSum;
                }
                else
                {
                    done = true;
                }
            }

            return valid;
        }

        private bool IsValid(List<string> parts)
        {
            bool valid = true;

            if (parts.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < parts.Count; i++)
            {
                // The even index of a sum must alway be a symbol, not a number
                if (i % 2 == 0 && !int.TryParse(parts[i], out _))
                {
                    return false;
                }

                // A sum must always have an odd number of parts: Number Symbol Number etc.
                if (parts.Count % 2 == 0)
                {
                    return false;
                }
            }

            return valid;
        }


        private List<string> SplitExpression(string expr)
        {
            List<string> parts = new List<string>();
            string savedValue = "";

            for (int i = 0; i < expr.Length; i++)
            {
                // Breaks expression in numbers, symbols and miscellaneous characters
                if (Char.IsDigit(expr[i]))
                {
                    savedValue += expr[i];
                }
                else if (Char.IsWhiteSpace(expr[i]) && savedValue != "")
                {
                    parts.Add(savedValue);
                    savedValue = "";
                }
                else if (expr[i] == '+' || expr[i] == '-' || expr[i] == '*' || expr[i] == '/')
                {
                    if (savedValue != "")
                    {
                        parts.Add(savedValue);
                        savedValue = "";
                    }
                    parts.Add(expr[i].ToString());
                }
                else if (!Char.IsWhiteSpace(expr[i]))
                {
                    parts.Add(expr[i].ToString());
                }
            }

            if (savedValue != "")
            {
                parts.Add(savedValue);
            }
            return parts;
        }

        private int CalculateSum(List<string> parts, out bool isStillValid)
        {
            int answer = 0;
            isStillValid = true;

            if (parts.Count == 1)
            {
                answer = Int32.Parse(parts[0]);
            }

            for (int i = 0; i < parts.Count; i++)
            {
                if (i % 2 != 0)
                {
                    int firstNum = Int32.Parse(parts[i - 1]);
                    int secondNum = Int32.Parse(parts[i + 1]);

                    // Checks the symbols and performs the appropriate calculation
                    if (parts[i] == "+")
                    {
                        answer = firstNum + secondNum;
                        parts[i + 1] = answer.ToString();
                    }
                    else if (parts[i] == "-")
                    {
                        answer = firstNum - secondNum;
                        parts[i + 1] = answer.ToString();
                    }
                    else if (parts[i] == "*")
                    {
                        answer = firstNum * secondNum;
                        parts[i + 1] = answer.ToString();
                    }
                    else if (parts[i] == "/")
                    {
                        if (Int32.Parse(parts[i + 1]) != 0)
                        {
                            answer = firstNum / secondNum;
                            parts[i + 1] = answer.ToString();
                        }
                        else
                        {
                            isStillValid = false;
                            answer = 0;
                        }
                    }
                }
            }

            return answer;
        }
    }
}
