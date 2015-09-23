using System.ComponentModel.DataAnnotations;

namespace Hunter.Services
{
    public class HunterValidation
    {
        private static IPoolService _poolService;

        public HunterValidation(IPoolService poolService)
        {
            _poolService = poolService;
        }

        public static ValidationResult ValidateIsPoolNameExist(string name)
        {
            var isValid = _poolService.IsPoolNameExist(name);

            return isValid ? new ValidationResult("Pool name alreafy exists!") : ValidationResult.Success;
        }
    }
}
