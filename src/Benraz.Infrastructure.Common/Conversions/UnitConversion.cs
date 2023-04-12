namespace Benraz.Infrastructure.Common.Conversions;

/// <summary>
/// Unit conversion.
/// </summary>
public static class UnitConversion
{
    /// <summary>
    /// Convert LBL to KB.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Convert value.</returns>
    public static decimal ConvertLbToKg(decimal value)
    {
        return value * (decimal)0.453592;
    }

    /// <summary>
    /// Calculate centimeter to CBM.
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <param name="length">Length.</param>
    /// <returns></returns>
    public static decimal CalculateCentimeterToCbm(decimal width, decimal height ,decimal length)
    {
        var value = width * height * length;
        return value / 1000000;
    }

    /// <summary>
    /// Convert meter to centimeter.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Centimeter.</returns>
    public static decimal ConvertMeterToCentimeter(decimal value)
    {
        return value * 100;
    }

    /// <summary>
    /// Convert inches to centimeter.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Centimeter.</returns>
    public static decimal ConvertInchesToCentimeter(decimal value)
    {
        return value * (decimal)2.54;
    }

    /// <summary>
    /// Convert feet to centimeter.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Centimeter.</returns>
    public static decimal ConvertFeetToCentimeter(decimal value)
    {
        return value * (decimal)30.48;
    }

    /// <summary>
    /// Convert CBM to CFT.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>CBF.</returns>
    public static decimal ConvertCbmToCft(decimal value)
    {
        return value * (decimal)35.315;
    }

    /// <summary>
    /// Convert CFT to CBM.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>CBM.</returns>
    public static decimal ConvertCftToCbm(decimal value)
    {
        return value * (decimal)0.0283168466;
    }

    /// <summary>
    /// Convert CUI to CBM.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>CBM.</returns>
    public static decimal ConvertCuiToCbm(decimal value)
    {
        return value * (decimal)0.0000163870;
    }
}



