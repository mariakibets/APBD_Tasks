using System;
using LegacyApp.interfaces;

namespace LegacyApp
{
    public class UserService
    {

        private IClientRepository _clientRepository;
        private IUserCreditService _userCreditService;
        private IUserService _userService;
        private IValidationService _validationService;

        public UserService()
        {
            _clientRepository = new ClientRepository();  
            _userCreditService = new UserCreditService();
            _userService = new UserServiceImplementation();  
            _validationService = new ValidationService();
        }

        public UserService(IClientRepository clientRepository, IUserCreditService userCreditService,
            IUserService userService, IValidationService validationService)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
            _userService = userService;
            _validationService = validationService;
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            
            if (!_validationService.IsValidName(firstName, lastName) || 
                !_validationService.IsValidEmail(email) || 
                !_validationService.IsValidAge(dateOfBirth))
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                DateOfBirth = dateOfBirth,
                Client = client
            };

            SetCreditLimit(user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            _userService.AddUser(user);
            return true;
        }

        private void SetCreditLimit(User user)
        {
            if (user.Client.ClientType == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (user.Client.ClientType == "ImportantClient")
            {
                user.HasCreditLimit = true;
                user.CreditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
            }
            else
            {
                user.HasCreditLimit = true;
                user.CreditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
            }
        }
    }
}
