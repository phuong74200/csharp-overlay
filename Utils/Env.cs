namespace Internal
{
  public class Utils
  {
    public static bool IsDebug()
    {
#if DEBUG
      return true;
#else
        return false;
#endif
    }

    public static string UpperFirstLetter(string str)
    {
      char.ToUpper(str[0]);
      return str;
    }
  }
}