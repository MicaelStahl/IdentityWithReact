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

        public PersonApiController(IPersonRepository service)
        {
            _service = service;
        }

        #endregion D.I

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody]Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
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
                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        #endregion Create

        #region Find

        [HttpGet("Find/{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    ModelState.AddModelError(string.Empty, StatusMessages.EmptyId);

                    throw new Exception(StatusMessages.EmptyId);
                }

                var result = await _service.Find(id);

                if (result.Message == ActionMessages.Found)
                {
                    return Ok(result.Person);
                }
                else if (result.Message == StatusMessages.EmptyId)
                {
                    throw new Exception(StatusMessages.EmptyId);
                }
                else if (result.Message == StatusMessages.NotFound)
                {
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
                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("find-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.FindAll();

            if (result.Message == ActionMessages.Found)
            {
                return Ok(result); // sending down entire viewmodel becasue react doesn't like getting lists sent down.
            }
            else if (result.Message == StatusMessages.EmptyList)
            {
                ModelState.AddModelError(string.Empty, StatusMessages.EmptyList);

                return BadRequest(StatusMessages.EmptyList);
            }
            else
            {
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
                if (!ModelState.IsValid)
                {
                    throw new Exception(StatusMessages.InvalidFields);
                }

                var result = await _service.Edit(person);

                if (result.Message == ActionMessages.Updated)
                {
                    return Ok(result.Person);
                }
                else if (result.Message == StatusMessages.NotFound)
                {
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
                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        #endregion Edit

        #region Delete

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            try
            {
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
                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        #endregion Delete
    }
}