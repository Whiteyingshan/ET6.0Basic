using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace ET
{
    struct NumericExporterType
    {
        public const string Base = "Base";
        public const string Add = "Add";
        public const string Pct = "Pct";
        public const string FinalAdd = "FinalAdd";
        public const string FinalPct = "FinalPct";

        public const int BaseId = 1;
        public const int AddId = 2;
        public const int PctId = 3;
        public const int FinalAddId = 4;
        public const int FinalPctId = 5;
    }
    public static class NumericExporter
    {
        private const string Pattern = @"[a-zA-Z\u2E80-\u9FFF][0-9a-zA-Z_\u2E80-\u9FFF]+";
        private const string ExcelPath = "../Excel/NumericTypeConfig.xlsx";
        private const string ClassPath = "../Unity/Codes/Model/Module/Numeric/NumericType.cs";
        private const string ExpressionPath = "../Unity/Codes/Model/Module/Numeric/NumericExpression.cs";

        private static readonly Dictionary<string, string> NumericCodes = new Dictionary<string, string>();

        public static void Export()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage package = new ExcelPackage(ExcelPath);
            NumericCodes.Clear();
            ExportClass(package);
            ExportExpression(package);
            Console.WriteLine("导表成功!");
        }

        private static void ExportClass(ExcelPackage package)
        {
            string space = "        ";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ET");
            sb.AppendLine("{");
            sb.AppendLine("    public enum NumericType : long");
            sb.AppendLine("    {");
            foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
            {
                for (int i = 6; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[$"B{i}"].Text.StartsWith('#'))
                    {
                        continue;
                    }
                    string id = worksheet.Cells[$"C{i}"].Text;
                    string code = worksheet.Cells[$"D{i}"].Text;
                    string remarks = worksheet.Cells[$"F{i}"].Text;
                    NumericCodes.Add(code, id);
                    sb.Append(space).AppendLine("/// <summary>");
                    sb.Append(space).AppendLine($"/// {remarks}");
                    sb.Append(space).AppendLine("/// </summary>");
                    sb.Append(space).AppendLine($"{code} = {id},");
                }
            }
            sb.AppendLine("    }");
            sb.Append('}');

            File.WriteAllText(ClassPath, sb.ToString());
        }

        private static void ExportExpression(ExcelPackage package)
        {
            Regex regex = new Regex(Pattern);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace ET.Module.Numeric");
            sb.Append('{');
            foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
            {
                for (int i = 6; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[$"B{i}"].Text.StartsWith('#'))
                    {
                        continue;
                    }
                    if (worksheet.Cells[$"E{i}"].Text != "3")
                    {
                        continue;
                    }

                    string id = worksheet.Cells[$"C{i}"].Text;
                    string code = worksheet.Cells[$"D{i}"].Text;
                    string expression = worksheet.Cells[$"G{i}"].Text;

                    sb.AppendLine();
                    sb.AppendLine($"    //{id}");
                    sb.AppendLine($"    [NumericExpression(NumericType.{code})]");
                    sb.AppendLine($"    internal sealed class {code}_Expression : INumericExpression");
                    sb.AppendLine("    {");
                    sb.AppendLine("        public long Calculate(NumericComponent num)");
                    sb.AppendLine("        {");
                    sb.AppendLine($"            return (long)({regex.Replace(expression, Replace)});");
                    sb.AppendLine("        }");
                    sb.AppendLine("    }");
                }
            }
            sb.Append('}');
            File.WriteAllText(ExpressionPath, sb.ToString());
        }

        private static string Replace(Match match)
        {
            return match.Value.StartsWith("ID") ? $"num[{match.Value.Remove(0, 2)}]" : $"num[{NumericCodes[match.Value]}]";
        }
    }
}