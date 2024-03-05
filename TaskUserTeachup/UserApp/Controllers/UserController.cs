using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UserApp.DTOs.User;
using UserApp.Services.Interfaces;

namespace UserApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region Register User

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/User/Register
        ///     {
        ///       "firstName": "Ljubomir",
        ///       "lastName": "Joldashev",
        ///       "username": "Ljupchecas",
        ///       "email": "Ljupchecas@nesto.com",
        ///       "password": "123456Ac2605",
        ///       "confirmPassword": "123456"
        ///     }
        ///     
        ///     Mandatory fields: FirstName, LastName, Username, Password, Email
        ///     
        /// </remarks>
        /// <param name="registerUserDto">The user registration information.</param>
        /// <returns>A response with status 201 if user creation is successful.</returns>
        /// <response code="201">Returns a success message if the user was created successfully.</response>
        /// <response code="500">Returns an error message if an exception occurs during the registration process.</response>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                // Log registration model information
                Log.Information($"Registration model info: FirstName: {registerUserDto.FirstName}, LastName {registerUserDto.LastName}, UserName: {registerUserDto.Username}");

                // Register the user
                _userService.Register(registerUserDto);

                // Log successful registration
                Log.Information($"Successfully registered {registerUserDto.Username}");

                // Return a 201 Created status with a message
                return StatusCode(StatusCodes.Status201Created, "User was created!");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during registration
                Log.Error($"RegisterUser: {ex.Message}");

                // Return a 500 Internal Server Error status with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Login User

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/User/Login
        ///     {
        ///       "username": "Ljupchecas",
        ///       "password": "123456Ac2605"
        ///     }
        ///     
        /// </remarks>
        /// <param name="loginUserDto">The user login information.</param>
        /// <returns>A response with status 200 and a JWT token if login is successful, or status 500 if an error occurs.</returns>
        /// <response code="200">Returns a JWT token if login is successful.</response>
        /// <response code="500">Returns an error message if an exception occurs during the login process.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                // Attempt to login the user and retrieve a JWT token
                string token = _userService.LoginUser(loginUserDto);

                // Log successful login
                Log.Information($"Successfully login {loginUserDto.Username}");

                // Return a 200 OK status with the JWT token
                return Ok(token);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during login
                Log.Error($"LoginUser: {ex.Message}");

                // Return a 500 Internal Server Error status with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Get User By Id

        /// <summary>
        /// Retrieves user information by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/User/11
        ///     
        /// </remarks>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A response with status 200 and user information if retrieval is successful.</returns>
        /// <response code="200">Returns user information if retrieval is successful.</response>
        /// <response code="500">Returns an error message if an exception occurs during the retrieval process.</response>
        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            try
            {
                // Retrieve user information by ID
                var userDto = _userService.GetUser(id);

                // Log successful user retrieval
                Log.Information($"Successfully retrieved user.");

                // Return a 200 OK status with user information
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user retrieval
                Log.Error($"GetUser: {ex.Message}");

                // Return a 500 Internal Server Error status with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Update User

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/User
        ///     {
        ///       "id": 12,
        ///       "firstName": "Ljubomir",
        ///       "lastName": "Joldashev",
        ///       "username": "LJ",
        ///       "password": "123456Ac2605",
        ///       "email": "Ljupche@nesto.com"
        ///     }
        ///     
        ///     Mandatory fields: FirstName, LastName, Username, Password, Email
        ///     
        /// </remarks>
        /// <param name="updateUserDto">The updated user information.</param>
        /// <returns>A response with status 200 if update is successful, or status 500 if an error occurs.</returns>
        /// <response code="200">Returns a success message if the user was updated successfully.</response>
        /// <response code="500">Returns an error message if an exception occurs during the update process.</response>
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                // Update user information
                _userService.UpdateUser(updateUserDto);

                // Log successful user update
                Log.Information($"Successfully updated user.");

                // Return a 200 OK status with a success message
                return Ok("User was updated.");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user update
                Log.Error($"UpdateUser: {ex.Message}");

                // Return a 500 Internal Server Error status with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Delete User

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/User/12
        ///     
        /// </remarks>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A response with status 200 if deletion is successful.</returns>
        /// <response code="200">Returns a success message if the user was deleted successfully.</response>
        /// <response code="500">Returns an error message if an exception occurs during the deletion process.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                // Delete user by ID
                _userService.DeleteUser(id);

                // Log successful user deletion
                Log.Information($"Successfully deleted user.");

                // Return a 200 OK status with a success message
                return Ok("User was deleted!");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user deletion
                Log.Error($"DeleteUser: {ex.Message}");

                // Return a 500 Internal Server Error status with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

    }
}
