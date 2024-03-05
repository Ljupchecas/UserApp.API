using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Serilog;
using System.Security.Claims;
using UserApp.DTOs.User;
using UserApp.Services.Interfaces;

namespace UserApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryService _userHistoryService;
        public UserHistoryController(IUserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        #region Get History By User

        /// <summary>
        /// Retrieves login history for a user by username.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/User/getHistory?username=Ljupchecas
        ///     
        ///     Mandatory fields: username
        ///     
        /// </remarks>
        /// <param name="username">The username of the user to retrieve login history for.</param>
        /// <returns>A response with status 200 and login history if retrieval is successful.</returns>
        /// <response code="200">Returns login history if retrieval is successful.</response>
        /// <response code="500">Returns an error message if an exception occurs during the retrieval process.</response>
        [HttpGet]
        public ActionResult<List<LoginHistoryDto>> GetHistoryByUser([FromQuery] string username)
        {
            try
            {
                // Retrieve login history for the user by username
                var loginHistoryDtos = _userHistoryService.GetHistoryByUser(username);

                // Return a 200 OK status with login history
                return Ok(loginHistoryDtos);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during retrieval of login history
                Log.Error($"GetHistoryByUser: {ex.Message}");

                // Return a 500 Internal Server Error status with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

    }
}
