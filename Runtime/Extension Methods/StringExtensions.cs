namespace UnityEngine
{
    public static class StringExtensions
    {
        public static int GetAnimatorHash(this string current) => Animator.StringToHash(current);
        public static int GetShaderHash(this string current) => Shader.PropertyToID(current);
    }
}