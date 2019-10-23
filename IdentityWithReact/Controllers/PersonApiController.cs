using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityWithReact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonApiController : ControllerBase
    {
        #region D.I

        private readonly IPersonRepository _service;
        private readonly ILogger<PersonApiController> _logger;

        public PersonApiController(IPersonRepository service, ILogger<PersonApiController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #endregion D.I

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody]Person person)
        {
            try
            {
                _logger.LogInformation("Create Person {Person}");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(message: "Person {Person} could not be created: Invalid modelstate.");

                    return BadRequest(ModelState);
                }

                var result = await _service.Create(person);

                if (result.Message == ActionMessages.Created)
                {
                    return Ok(result.Person);
                }
                else
                {
                    throw new Exception(result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(exception: ex.InnerException, ex.Message);

                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        #endregion Create

        #region Find

        [HttpGet("Find/{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Find Person {Person} with {id}");

                if (id == Guid.Empty)
                {
                    _logger.LogWarning(exception: new NullReferenceException(StatusMessages.EmptyId), StatusMessages.EmptyId);

                    ModelState.AddModelError(string.Empty, StatusMessages.EmptyId);

                    throw new Exception(StatusMessages.EmptyId);
                }

                var result = await _service.Find(id);

                if (result.Message == ActionMessages.Found)
                {
                    _logger.LogInformation("Person {Person} was successfully found with {id}");

                    return Ok(result.Person);
                }
                else if (result.Message == StatusMessages.EmptyId)
                {
                    throw new Exception(StatusMessages.EmptyId);
                }
                else if (result.Message == StatusMessages.NotFound)
                {
                    _logger.LogWarning(StatusMessages.NotFound, id);

                    ModelState.AddModelError(string.Empty, StatusMessages.NotFound);

                    return NotFound(StatusMessages.NotFound);
                }
                else
                {
                    throw new Exception("Unexpected error occurred: Something went wrong.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(exception: ex.InnerException, ex.Message);

                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("find-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            _logger.LogInformation("Find all People {List<Person>}");

            var result = await _service.FindAll();

            if (result.Message == ActionMessages.Found)
            {
                _logger.LogInformation("People {List<Person>} was successfully found.");

                return Ok(result); // sending down entire viewmodel becasue react doesn't like getting lists sent down.
            }
            else if (result.Message == StatusMessages.EmptyList)
            {
                _logger.LogWarning("No {List<Person>} was found.");

                ModelState.AddModelError(string.Empty, StatusMessages.EmptyList);

                return BadRequest(StatusMessages.EmptyList);
            }
            else
            {
                _logger.LogError("Unexpected error: Something went wrong.");

                ModelState.AddModelError(string.Empty, "Something went wrong.");

                return BadRequest("Something went wrong.");
            }
        }

        #endregion Find

        #region Edit

        [HttpPut("Edit")]
        public async Task<IActionResult> EditAsync([FromBody]Person person)
        {
            try
            {
                _logger.LogInformation("Update Person {Person} with {person} data");

                if (!ModelState.IsValid)
                {
                    throw new Exception(StatusMessages.InvalidFields);
                }

                var result = await _service.Edit(person);

                if (result.Message == ActionMessages.Updated)
                {
                    _logger.LogInformation("Person {Person} was successfully updated");

                    return Ok(result.Person);
                }
                else if (result.Message == StatusMessages.NotFound)
                {
                    _logger.LogWarning("Person {Person} could not be found with ID {person.Id}");

                    ModelState.AddModelError(string.Empty, result.Message);

                    return NotFound(result.Message);
                }
                else
                {
                    throw new Exception(result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(exception: ex.InnerException, message: ex.Message);

                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        #endregion Edit

        #region Delete

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Delete Person {Person} with ID {id}");

                if (id == Guid.Empty)
                {
                    throw new Exception(StatusMessages.EmptyId);
                }

                var result = await _service.Delete(id);

                if (result == ActionMessages.Deleted)
                {
                    return Ok(result);
                }
                else if (result == StatusMessages.NotFound)
                {
                    _logger.LogWarning("Person {Person} was not deleted: ID {id} did not match any people");

                    ModelState.AddModelError(string.Empty, "Unexpected error occurred: ID did not match any people.");

                    return NotFound(result);
                }
                else
                {
                    throw new Exception(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(exception: ex.InnerException, message: ex.Message);

                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        #endregion Delete
    }
}