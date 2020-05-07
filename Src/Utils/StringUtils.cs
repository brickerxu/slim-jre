namespace slim_jre.Utils
{
    class StringUtils
    {
        public static bool isNotEmpty(string str)
        {
            return !isEmpty(str);
        }
        public static bool isEmpty(string str)
        {
            if (null == str || "" == str)
            {
                return true;
            }
            return false;
        }
    }
}
