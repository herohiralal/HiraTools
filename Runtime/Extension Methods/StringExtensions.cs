using System;

namespace UnityEngine
{
    public static class StringExtensions
    {
        public static int GetAnimatorHash(this string current) => Animator.StringToHash(current);
        public static int GetShaderHash(this string current) => Shader.PropertyToID(current);

        public static string ConcatenateStringsWith<T>(this T[] input, Func<T, string> preprocessor, string connector)
        {
            var s = "";
            
            var inputLength = input.Length;
            for (var i = 0; i < inputLength; i++)
            {
                s += preprocessor.Invoke(input[i]);
                if (i != inputLength - 1) s += connector;
            }

            return s;
        }
    }
}