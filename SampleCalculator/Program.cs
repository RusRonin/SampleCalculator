using System;
using System.Collections.Generic;

namespace SampleCalculator
{
    class Program
    {
        private static Dictionary<char, int> operationPriorities = new() { { '+', 2 }, { '-', 2 }, { '*', 3 }, { '/', 3 }, { '(', 1 } };

        public static void Main(string[] args)
        {
            string rawExpression = Console.ReadLine();
            string[] expression = PreformatExpression(rawExpression).Trim().Split(' ');
            IEnumerable<string> postfixExpression = null;
            
            try
            {
                postfixExpression = ExpressionToPostfixNotation(expression);
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect input");
                return;
            }

            try
            {
                Console.WriteLine(CountPostfixExpressionResult(postfixExpression));
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("Can't divide by zero");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect input");
            }
        }

        //Adds whitespaces befire brackets and makes fractional numbers presented correctly
        private static string PreformatExpression(string expression)
        {
            string preformattedExpression = string.Empty;
            foreach (char c in expression)
            {
                if ((c.Equals('(')) || (c.Equals(')')))
                {
                    preformattedExpression += $" {c} ";
                    continue;
                }
                if (c.Equals('.'))
                {
                    preformattedExpression += ',';
                    continue;
                }
                preformattedExpression += c;
            }
            return preformattedExpression;
        }

        private static IEnumerable<string> ExpressionToPostfixNotation(IEnumerable<string> expression)
        {
            Stack<char> operators = new Stack<char>();
            List<string> postfixExpression = new List<string>();
            foreach (string s in expression)
            {
                if ((s.Equals(" ")) || (s.Equals(String.Empty)))
                {
                    continue;
                }
                if (double.TryParse(s, out double i))
                {
                    //this is a number
                    postfixExpression.Add(s);
                    continue;
                }
                if (s.Equals("("))
                {
                    operators.Push(s[0]);
                    continue;
                }
                if (s.Equals(")"))
                {
                    char topElement = operators.Pop();
                    while (!topElement.Equals('('))
                    {
                        postfixExpression.Add(topElement.ToString());
                        topElement = operators.Pop();
                    }
                    continue;
                }
                while ((operators.Count > 0) && (operationPriorities[operators.Peek()] >= operationPriorities[s[0]]))
                {
                    postfixExpression.Add(operators.Pop().ToString());
                }
                operators.Push(s[0]);
            }
            while (operators.Count > 0)
            {
                postfixExpression.Add(operators.Pop().ToString());
            }
            return postfixExpression;
        }

        private static double CountPostfixExpressionResult(IEnumerable<string> postfixExpression)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string s in postfixExpression)
            {
                if (double.TryParse(s, out double i))
                {
                    stack.Push(s);
                }
                else
                {
                    double secondOperand = double.Parse(stack.Pop());
                    double firstOperand = double.Parse(stack.Pop());
                    double result = 0;
                    switch (s)
                    {
                        case "+":
                            result = firstOperand + secondOperand;
                            break;
                        case "-":
                            result = firstOperand - secondOperand;
                            break;
                        case "*":
                            result = firstOperand * secondOperand;
                            break;
                        case "/":
                            result = firstOperand / secondOperand;
                            break;
                    }
                    stack.Push(result.ToString());
                }
            }
            return double.Parse(stack.Pop());
        }
    }
}
