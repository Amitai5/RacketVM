﻿using System.Collections.Generic;
using RacketLite.ConsoleTools;
using RacketLite.Expressions;
using RacketLite.ValueTypes;
using System.Diagnostics;
using System.Text;
using System.IO;
using System;

namespace RacketLite
{
    public class RacketInterpreter
    {
        public bool PrintTree { get; }
        public bool ExpressionClear { get; }

        public const string Version = "v3.0 beta";
        public const string CommandLinePrefix = ">";
        public static string Title => $"Welcome to Racket-Lite {Version} [cs].";

        private readonly ConsoleHelper helper;
        private readonly Dictionary<string, Action> interpreterCommands = new Dictionary<string, Action>();

        public RacketInterpreter(bool printTree, bool expressionClear)
        {
            PrintTree = printTree;
            ExpressionClear = expressionClear;

            //Add interpreter commands
            interpreterCommands.Add("#help", ShowRacketHelp);
            interpreterCommands.Add("#ldf", () => ParseAndPrintFile());
            interpreterCommands.Add("#loadfile", () => ParseAndPrintFile());
            interpreterCommands.Add("#cls", () => { Console.Clear(); Console.WriteLine(Title); });
            interpreterCommands.Add("#clear", () => { Console.Clear(); Console.WriteLine(Title); });

            helper = new ConsoleHelper(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine(Title);
        }

        public bool ParseAndPrintFile()
        {
            string fileLocation = helper.ReadFilename(ConsoleColor.Green);

            if (File.Exists(fileLocation))
            {
                helper.WriteLine("Reading File...", ConsoleColor.Yellow);
                helper.ResetColors();

                string fileText = File.ReadAllText(fileLocation);
                return ParseAndPrintMultiLine(fileText);
            }

            helper.WriteLine($"Could not find file, {fileLocation}", ConsoleColor.Red);
            helper.ResetColors();
            return false;
        }

        public bool ParseAndPrintMultiLine(string str)
        {
            string[] expressions = str.Split('\n');
            StringBuilder currentExpression = new StringBuilder();

            for (int i = 0; i < expressions.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(expressions[i]) || expressions[i][0] == ';' || expressions[i][0] == '#')
                {
                    //Ignore comments and reader directives
                    continue;
                }

                //Parse multi-line expressions
                currentExpression.Append(expressions[i]);
                if (!parenthesisBalance(currentExpression.ToString(), out _) && i != expressions.Length - 1)
                {
                    continue;
                }

                if (!ParseAndPrintLine(currentExpression.ToString()))
                {
                    helper.WriteLine($"\tError: failed to parse line {i}", ConsoleColor.Red);
                    helper.ResetColors();
                    return false;
                }
                currentExpression.Clear();
            }
            return true;
        }



        public bool ReadAndParseLine()
        {
            Console.Write($"{CommandLinePrefix} ");
            string str = Console.ReadLine() ?? "";

            if (ExpressionClear)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to Racket-Lite {Version} [cs].");
                Console.WriteLine($"{CommandLinePrefix} {str}");
            }
            return ParseAndPrintLine(str);
        }

        public bool ParseAndPrintLine(string str)
        {
            if (ParseInterpreterCommand(str))
            {
                return true;
            }

            (RacketExpression? expression, string result) = ParseLine(str);
            bool validRacket = expression != null && !result.StartsWith(';');

            if (PrintTree)
            {
                helper.Write($"\n{expression}", ConsoleColor.DarkGray);
                helper.WriteLine("--------------------------", ConsoleColor.DarkGray);
                helper.ResetColors();
                Console.Write("Result: ");
            }

            if (!validRacket)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write(result);
            helper.ResetColors();
            return validRacket;
        }

        private (RacketExpression?, string) ParseLine(string str)
        {
            RacketExpression? expression = null;
            RacketValueType? retValue;
            string retValueString;

            try
            {
                expression = RacketExpression.Parse(str);
                retValue = expression?.Evaluate();

                retValueString = retValue?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                retValueString = $"; {ex.Message}";
            }

            if (!string.IsNullOrEmpty(retValueString))
            {
                retValueString += Environment.NewLine;
            }
            return (expression, retValueString);
        }

        #region Helper Methods

        private bool ParseInterpreterCommand(string str)
        {
            str = str.Trim().ToLower();
            if (interpreterCommands.ContainsKey(str))
            {
                interpreterCommands[str].Invoke();
                return true;
            }
            else if (str[0] == '#')
            {
                return true;
            }
            return false;
        }

        private void ShowRacketHelp()
        {
            helper.Write("For help with Racket-Lite, visit ", ConsoleColor.Yellow);
            helper.WriteLine("https://github.com/Amitai5/RacketLite/wiki", ConsoleColor.Blue);
            Process.Start(new ProcessStartInfo("cmd", "/c start https://github.com/Amitai5/RacketLite/wiki"));
            helper.ResetColors();
        }

        private static bool parenthesisBalance(string str, out int balance)
        {
            balance = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(')
                {
                    balance++;
                }
                else if (str[i] == ')')
                {
                    balance--;
                }
            }
            return balance == 0;
        }

        #endregion Helper Methods
    }
}