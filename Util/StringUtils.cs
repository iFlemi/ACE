namespace Ace.Util;

public static class StringUtils
{
  public static string FormatPercentage(this float value) =>
    $"{value:P0}";
}