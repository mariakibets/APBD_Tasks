using System;

namespace LegacyApp.interfaces;

public interface IValidationService
{
    bool IsValidName(string firstName, string lastName);
    bool IsValidEmail(string email);
    bool IsValidAge(DateTime dateOfBirth);
}