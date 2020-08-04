using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOwnerServer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public AccountController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{id}", Name = "AccountById")]
        public async Task<IActionResult> GetAccountByIdAsync(Guid id)
        {
            try
            {
                var account = await _repository.Account.GetAccountByIdAsync(id);

                if (account.IsEmptyObject())
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id: {id}");
                    return Ok(account);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAccountById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            try
            {

                if (account.IsObjectNull())
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid account object sent from client");
                    return BadRequest("Invalid model object");
                }

                await _repository.Account.CreateAccountAsync(account);

                return CreatedAtRoute("AccountById", new { id = account.Id }, account);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] Account account)
        {
            try
            {

                if (account.IsObjectNull())
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid account object sent from client");
                    return BadRequest("Invalid model object");
                }

                var dbAccount = await _repository.Account.GetAccountByIdAsync(id);
                if (dbAccount.IsEmptyObject())
                {
                    _logger.LogError($"Account with id {id} not found in db.");
                    return NotFound("Account object is null");
                }

                await _repository.Account.UpdateAccountAsync(dbAccount, account);

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {

                var account = await _repository.Account.GetAccountByIdAsync(id);

                if (account.IsEmptyObject())
                {
                    _logger.LogError($"Account with id {id} not found in db.");
                    return NotFound("Account object is null");
                }

                await _repository.Account.DeleteAccountAsync(account);

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}