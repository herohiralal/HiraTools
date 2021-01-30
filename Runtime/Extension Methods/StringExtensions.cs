using System;

namespace UnityEngine
{
    public static class StringExtensions
    {
        public static int GetAnimatorHash(this string current) => Animator.StringToHash(current);
        public static int GetShaderHash(this string current) => Shader.PropertyToID(current);

        public static string ConcatenateStringsWith<T>(this T[] input, Func<T, string> preprocessor, string connector, string prefix = "", string suffix = "")
        {
            var s = prefix;
            
            var inputLength = input.Length;
            for (var i = 0; i < inputLength; i++)
            {
                s += preprocessor.Invoke(input[i]);
                s += i == inputLength - 1 ? suffix : connector;
            }

            return s;
        }
    }
}