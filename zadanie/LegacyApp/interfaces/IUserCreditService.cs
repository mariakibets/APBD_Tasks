using System;

namespace LegacyApp.interfaces;

public interface IUserCreditService
{
    int GetCreditLimit(string lastName, DateTime dateOfBirth);
}