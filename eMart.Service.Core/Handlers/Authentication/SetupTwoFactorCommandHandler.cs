using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Services;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class SetupTwoFactorCommandHandler : IRequestHandler<SetupTwoFactorCommand, CommonResponse<TwoFactorSetupDto>>
    {
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly IUserOtpRepository _userOtpRepository;

        public SetupTwoFactorCommandHandler(
            ITwoFactorAuthService twoFactorAuthService,
            IUserOtpRepository userOtpRepository)
        {
            _twoFactorAuthService = twoFactorAuthService;
            _userOtpRepository = userOtpRepository;
        }

        public async Task<CommonResponse<TwoFactorSetupDto>> Handle(SetupTwoFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Generate 2FA setup
                var setup = await _twoFactorAuthService.GenerateTwoFactorSetupAsync(request.UserId);

                // Save the secret key to the database (not verified yet)
                var userOtp = await _userOtpRepository.GetUserOtpByUserIdAsync(request.UserId);
                if (userOtp == null)
                {
                    // Create new user OTP record
                    userOtp = new eMart.Service.DataModels.UserOtp
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = request.UserId,
                        SecretKey = setup.SecretKey,
                        IsVerified = false,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _userOtpRepository.CreateUserOtpAsync(userOtp);
                }
                else
                {
                    // Update existing record
                    userOtp.SecretKey = setup.SecretKey;
                    userOtp.IsVerified = false; // Not verified until user confirms with code
                    userOtp.UpdatedAt = DateTime.UtcNow;
                    await _userOtpRepository.UpdateUserOtpAsync(userOtp);
                }

                return new CommonResponse<TwoFactorSetupDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = setup,
                    Message = "Two-factor authentication setup generated successfully"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<TwoFactorSetupDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while setting up two-factor authentication",
                    Data = null
                };
            }
        }
    }
}
