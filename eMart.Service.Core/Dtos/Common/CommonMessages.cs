using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Common
{
    public static class CommonMessages
    {
        // Token messages
        public const string TokenExpired = "The token is expired and not valid anymore.";
        public const string TokenNotFound = "The token is not found";
        public const string TokenUsernamePasswordWrong = "Incorrect username or password.";
        public const string UserDisabled = "User is disabled!";
        public const string TokenSignatureError = "Token signature not valid.";
        public const string TokenJsonError = "Unable to read JSON value.";
        public const string BlankToken = "Couldn't find authentication bearer, will ignore the header.";

        // User messages
        public const string UserNotFound = "User not found with given id or username and password.";
        public const string UserIsAlredyExits = "User is Already exit with These UserName and Email.";
        public const string UserPasswordNotValid = "Entered Old password is not valid.";
        public const string UserPasswordChange = "Password changed successfully.";
        public const string UserIdNotFound = "User id not found.";
        public const string ErrorInGetUser = "Error in getting users.";
        public const string PasswordNotMatch = "Password not match.";
        public const string InvalidPassword = "Invalid Credentials";
        public const string PasswordResetMsg = "A password reset message was sent to your email address. Please click the link to \"reset password\".";
        public const string OldPasswordNotMatch = "Old password not match.";
        public const string PasswordNotValid = "Password is not valid. Password should contain 6 to 20 characters string with at least one digit, one upper case letter, one lower case letter, and one special symbol (\"@#$%\").";
        public const string ErrorInRegistrationEmailSending = "Error while sending email at registration.";
        public const string ErrorInForgotPasswordEmailSending = "Error occurred while sending email at forgot password.";
        public const string ErrorInResetPasswordEmailSending = "Error occurred while sending email at reset password.";
        public const string UsernameDuplicateEntry = "User already exists with given email.";
        public const string UserDeleted = "User deleted successfully.";
        public const string UserNotActive = "User is InActive";
        public const string UserEmailSubject = "User Details";
        public const string UserEmailSuccessResponse = "Email Sent Successfully";
        public const string UserFound = "User found successfully.";
        public const string UserUpdatedSuccessfully = "User updated successfully.";
        public const string UserDeletedSuccessfully = "User deleted successfully.";
        public const string UsersFound = "Users found successfully.";

        // Product Messages

        public const string ProductAddedSuccessfully = "Added Product successfully";
        public const string ProductFound = "User found all Products.";
        public const string ProductNotFound = "TheProduct is Not found.";
        public const string ProductEditSuccessfully = "Successfully edited Product Details.";
        public const string ProductRemoved = "Product Deleted successfully.";

        // Miscellaneous messages
        public const string ForgotPasswordEmailSendSuccessfully = "Email sent successfully.";
        public const string UserAddedSuccessfully = "Added user successfully.";
        public const string UserAddedError = "Error while saving user.";
        public const string UserEmailError = "Error while sending email to user!";
        public const string UserGetListSuccessful = "Successfully got list of users.";
        public const string VisitorGetListSuccessful = "Successfully got list of visitors.";
        public const string VisitorGetListError = "Error in retrieving list of visitors.";
        public const string VisitorGetSuccessful = "Successfully got visitor.";
        public const string VisitorGetError = "Error while getting visitor.";
        public const string VisitorDeleteSuccessfully = "Successfully deleted visitor.";
        public const string VisitorDeleteError = "Error while deleting visitor.";
        public const string VisitorCreateSuccessfully = "Created visitor successfully.";
        public const string VisitorCreateError = "Error while creating visitor.";
        public const string ErrorWrongRequest = "Error in request.";
        public const string ErrorInvalidRequest = "Invalid passing data.";
        public const string VisitorPresignedUrlGetSuccessfully = "Fetched value of presignedUrl successfully.";
        public const string VisitorPresignedUrlGetError = "Error while fetching value of presignedUrl.";
        public const string VisitorEmailError = "Error while sending email.";
        public const string VisitorUpdateSuccessfully = "Updated visitor successfully.";
        public const string VisitorUpdateError = "Error while updating visitor.";
        public const string UserIdEditError = "Error while editing user.";
        public const string UserIdEditSuccessfully = "Successfully edited user.";
        public const string UserProfileEditSuccessfully = "Successfully edited user profile.";
        public const string UserDeleteSuccessfully = "Successfully deleted user.";
        public const string UserDeleteError = "Error while deleting user.";
        public const string UserDeleteAlreadySuccessfully = "User already deleted.";
        public const string UserIdGetError = "Error while getting user.";
        public const string UserIdGetSuccessfully = "Successfully got user.";
        public const string VisitorDetailsSuccessfully = "Successfully got visitor details.";
        public const string VisitorDetailsMonthwiseCount = "Successfully got cashier visitor count.";
        public const string VisitorDetailsError = "Error getting visitor details.";
        public const string VisitorIdNotFound = "Cannot find visitor.";
        public const string UserForgotPasswordEmailSuccessfully = "Email sent successfully.";
        public const string UserCreateError = "Error while creating user.";
        public const string VisitorChartCountSuccessfully = "Successfully fetch graph data.";
        public const string DataSuccessfully = "Successfully fetch data.";
        public const string EmailSentSuccessfully = "Email sent !";
        public const string Unauthorized = "Unauthorized";

        // Category messages
        public const string CategoryAddedSuccessfully = "Added Category successfully.";
        public const string CategoryNotFound = "User not found with Category with given id or Name.";
        public const string CategoryFound = "User found all Categories.";
        public const string CategoryIsAlredyExits = "User is Already exit with These UserName and Email.";
        public const string CategoryPasswordNotValid = "Entered Old password is not valid.";
        public const string CategoryPasswordChange = "Password changed successfully.";
        public const string CategoryEditSuccessfully = "Successfully edited Category Details.";
        public const string CategoryUpdatedSuccessfully = "Category updated successfully.";
        public const string CategoryDeletedSuccessfully = "Category deleted successfully.";
        public const string CategoriesFound = "Categories found successfully.";

        // Favorite messages
        public const string ProductAddedInFavorite = "Product Added into Favorite successfully.";
        public const string FavoriteRemoved = "Product Removed from Favorite successfully.";
        public const string AlreadyInFavorite = "Product is Already in Favorite.";
        public const string FavoriteFound = "User found all Favorite Products.";
        public const string FavoriteNotFound = "User Not found the Favorite Products.";
    }
}
