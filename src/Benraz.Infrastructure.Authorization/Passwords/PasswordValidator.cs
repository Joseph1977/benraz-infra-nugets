using System.Text.RegularExpressions;

namespace Benraz.Infrastructure.Authorization.Passwords;

/// <summary>
/// Password validator.
/// </summary>
public class PasswordValidator
{
    /// <summary>
    /// Password regex.
    /// </summary>
    public const string PASSWORD_REGEX = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$";

    /// <summary>
    /// Is valid password.
    /// </summary>
    /// <param name="password">Password.</param>
    /// <returns>Boolean, valid or not.</returns>
    public static bool IsValid(string password)
    {
        return Validator(true, true, true, true, false, password)
               && password.Length >= 8
               && Regex.IsMatch(password, PASSWORD_REGEX);
    }

    /// <summary>
    /// Checks if the password created is valid.
    /// </summary>
    /// <param name="includeLowercase">Boolean, if lowercase are required.</param>
    /// <param name="includeUppercase">Boolean, if uppercase are required.</param>
    /// <param name="includeNumeric">Boolean, if numerics are required.</param>
    /// <param name="includeSpecial">Boolean if special characters are required.</param>
    /// <param name="includeSpaces">Boolean, if spaces are required.</param>
    /// <param name="password">Generated password.</param>
    /// <returns>True or False, if the password is valid or not.</returns>
    private static bool Validator(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, string password)
    {
        const string REGEX_LOWERCASE = @"[a-z]";
        const string REGEX_UPPERCASE = @"[A-Z]";
        const string REGEX_NUMERIC = @"[\d]";
        const string REGEX_SPECIAL = @"([!#$%&*@\\])+";
        const string REGEX_SPACE = @"([ ])+";

        bool lowerCaseIsValid = !includeLowercase || (includeLowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
        bool upperCaseIsValid = !includeUppercase || (includeUppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
        bool numericIsValid = !includeNumeric || (includeNumeric && Regex.IsMatch(password, REGEX_NUMERIC));
        bool symbolsAreValid = !includeSpecial || (includeSpecial && Regex.IsMatch(password, REGEX_SPECIAL));
        bool spacesAreValid = !includeSpaces || (includeSpaces && Regex.IsMatch(password, REGEX_SPACE));

        return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid && spacesAreValid;
    }
}


