namespace Benraz.Infrastructure.Common.DateTimeHelpers;

using System;

/// <summary>
/// Date time converter.
/// </summary>
public static class DateTimeConverter
{
    /// <summary>
    /// Utc format.
    /// </summary>
    public const string UTC_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";

    /// <summary>
    /// Get utc string.
    /// </summary>
    /// <param name="utc">Utc.</param>
    /// <returns></returns>
    public static string ToUtcString(this DateTime? utc)
    {
        if (utc == null || utc.Value == DateTime.MinValue)
            return null;

        return utc.Value.Kind == DateTimeKind.Unspecified
            ? utc.Value.ToString(UTC_FORMAT)
            : utc.Value.ToUniversalTime().ToString(UTC_FORMAT);
    }
}


