using System;
using LegacyApp.interfaces;

namespace LegacyApp;

public class ValidationService : IValidationService
{
    public bool IsValidName(string firstName, string lastName)
    {
        return !string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName);
    }

    public bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public bool IsValidAge(DateTime dateOfBirth)
    {
        return CalculateAge(dateOfBirth) >= 21;
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var now = DateTime.UtcNow;
        int age = now.Year - dateOfBirth.Year;

        if (now < dateOfBirth.AddYears(age))
        {
            age--;
        }
        return age;
    }
    
}