using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChatRoomApp.Controllers.Base
{
    public class BaseChatRoomController : Controller
    {
        /// <summary>
        /// This method show a list of your model errors atrributes.
        /// </summary>
        protected IEnumerable<string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var errorMessage = modelState.Values
                .SelectMany(m => m.Errors)
                .Select(error => error.ErrorMessage);

            return errorMessage;
        }

        /// <summary>
        /// This method show a list of your user model errors atrributes.
        /// </summary>
        protected void GetUserModelStateErrors(IEnumerable<IdentityError> errors, 
            ModelStateDictionary modelState)
        {
 
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }
        }
    }
}
