using System;
using System.Collections.Generic;
using System.Text;

namespace PUMz
{
    public class PathUtils
    {
        public static string ValidatePath(string path)
        {
            string result = path;

            result = result.Replace(":", " ");
            result = result.Replace("|", " ");
            result = result.Replace("<", " ");
            result = result.Replace(">", " ");
            result = result.Replace("\"", " ");
            result = result.Replace("*", " ");
            result = result.Replace("?", " ");
            result = result.Replace("/", " ");

            return result;
        }

        public static string ValidateFileName(string fileName)
        {
            string result = fileName;

            result = ValidatePath(result);

            result = result.Replace("\\", " ");

            return result;
        }
    }
}
