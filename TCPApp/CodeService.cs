namespace TCPApp
{
    public static class CodeService
    {
        #region Field
        static List<string> _codes = new() { "AA000000" };
        #endregion

        #region Public Methods
        /// <summary>
        /// Generate discount codes or client
        /// </summary>
        /// <param name="numberOfCode"></param>
        /// <param name="length"></param>
        /// <returns>List<string></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> GenerateDiscountCodesAsync(ushort numberOfCode, byte length)
        {
            if (length < 7 || length > 8)
            {
                throw new Exception($"{nameof(length)} is not valid, please enter either 7 or 8");
            }
            while (numberOfCode > 0)
            {
                _codes.Add(GenerateCode(_codes, length));
                numberOfCode--;
            }
            _codes.Remove(_codes[0]);
            return _codes;
        }

        /// <summary>
        /// Confirm if the supplied code by client is valid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CheckDiscountCode(string code)
        {
            if (!_codes.Contains(code))
            {
                return "Invalid code";
            }
            return "Code verified successfully";
        }

        #endregion


        #region Private Methods
        private static string GenerateCode(List<string> _service, int length)
        {
            int intLength = length - 2;

            int num = int.Parse(_service[_service.Count - 1].Substring(2, intLength));
            string myString = _service[_service.Count - 1];
            char firstChar = myString[0];
            char secondChar = myString[1];
            if (num + 1 > 99999)
            {
                secondChar = (char)(secondChar + 1);
                num = 0;
                if (secondChar > 'Z')
                {
                    firstChar = (char)(firstChar + 1);
                    secondChar = (char)(secondChar - 26);

                    if (firstChar > 'Z')
                    {
                        firstChar = (char)(firstChar - 26);
                    }
                }
            }
            var temp = (num + 1).ToString($"D{intLength}");
            return (firstChar.ToString() + secondChar.ToString() + temp);
        }
        #endregion
    }
}
