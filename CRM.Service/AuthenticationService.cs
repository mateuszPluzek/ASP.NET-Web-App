using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Service.IService;
using CRM.Utility.IUtility;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.Mail;
using System.Net.Mime;

namespace CRM.Service
{
    public class AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IApplicationEmailSender applicationEmailSender
        ) : IAuthenticationService
    {
        public Task<bool> ChangePasswordAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<bool>> ForgotPasswordAsync(ApplicationUserForgotPasswordInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "USer not found",
                    Data = false
                };
            }
            user.VerificationCode = GenerateVerificationCode();
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to save verification code"
                };
            }
            // send email with code
            model.Code = user.VerificationCode.ToString();
            model.FullName = $"{user.FirstName} {user.LastName}";
            await SendEmailConfirmationCodeAsync(model);

            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Succesfully sent verification code"
            };
        }
        public async Task<ResponseModel<bool>> LoginAsync(ApplicationUserLoginInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            ArgumentNullException.ThrowIfNull(model.Password);

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = true,
                    Message = "Login succesful",
                    Data = true
                };
            }
            string errorMessage = result.IsLockedOut ? "User is locked out." :
                                  result.IsNotAllowed ? "Login is not allowed." :
                                  result.RequiresTwoFactor ? "Two factor authentication is required." :
                                  "invalid login attempt.";

            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = errorMessage,
                Data = false
            };
        }
        public Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<bool>> RegisterAsync(ApplicationUserRegisterInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            ArgumentNullException.ThrowIfNull(model.Password);

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                RegistrationDate = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = true,
                    Message = "User created successfully",
                    Data = true
                };
            }

            string ErrorMessage = result.Errors.Any()
                ? string.Join("; ", result.Errors.Select(e => e.Code))
                : "Unable to create user due to unknown errors";


            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = ErrorMessage,
                Data = false
            };
        }
        public async Task<ResponseModel<bool>> ConfirmEmailAsync(ApplicationUserConfirmEmailInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "USer not found",
                    Data = false
                };
            }
            if (user.EmailConfirmed)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Email already confirmed",
                    Data = false
                };
            }
            user.VerificationCode = GenerateVerificationCode();
            var result = await userManager.UpdateAsync(user);

            if(!result.Succeeded)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to save verification code"
                };
            }
            // send email with code
            model.Code = user.VerificationCode.ToString();
            model.FullName = $"{user.FirstName} {user.LastName}";
            await SendEmailConfirmationCodeAsync(model);

            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Succesfully sent verification code"
            };
        }

        public async Task<ResponseModel<bool>> ConfirmEmailVerifyCodeAsync(ApplicationUserConfirmEmailInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            ArgumentNullException.ThrowIfNull(model.Code);

            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            model.FullName = $"{user.FirstName} {user.LastName}";
            bool isCodeValid = user.VerificationCode != null && user.VerificationCode.ToString() == model.Code;
            if(isCodeValid)
            {
                user.EmailConfirmed = true;
                user.Activity = true;
                var result = await userManager.UpdateAsync(user);
                return new ResponseModel<bool>
                {
                    IsSuccess = result.Succeeded,
                    Message = result.Succeeded ? "Email confirmed successfully" : "email confirmation failed"
                };
            }
            else
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid confirmation code"
                };
            }
        }

        public async Task<ResponseModel<bool>> ResetPasswordAsync(ApplicationUserForgotPasswordInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model.Email);
            ArgumentNullException.ThrowIfNull(model.Code);
            ArgumentNullException.ThrowIfNull(model.Password);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            bool isCodeValid = user.VerificationCode != null && user.VerificationCode.ToString() == model.Code;
            if (isCodeValid)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                return new ResponseModel<bool>
                {
                    IsSuccess = result.Succeeded,
                    Message = result.Succeeded ? "Password reset successful" : "Unable to reset password"
                };
            }
            else
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid confirmation code"
                };
            }
        }

        private short GenerateVerificationCode()
        {
            Random random = new Random();
            return (short)random.Next(1000, 9999);
        }

        private async Task SendEmailConfirmationCodeAsync(ApplicationUserVerificationBaseInputModel model)
        {
            MailMessage email = new();
            email.To.Add(model.Email);
            email.Subject = "ASPNET app";

            var emailContent = model.EmailTemplate.Replace("{FullName}", model.FullName).Replace("{Code}", model.Code);
            var alternateView = AlternateView.CreateAlternateViewFromString(emailContent, null, MediaTypeNames.Text.Html);
            email.AlternateViews.Add(alternateView);
            await applicationEmailSender.SendEmailAsync(email);

        }
    }
}
