namespace ProjectsSoftuni.Common
{
    using System;

    public static class Validator
    {
        public static void ThrowIfStringIsNullOrEmpty(string str, string exceptionMessage = null)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(exceptionMessage);
            }
        }
    }
}
