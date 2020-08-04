using Contracts;
using Entities;
using Entities.ExtendedModels;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task CreateAccountAsync(Account account)
        {
            account.Id = Guid.NewGuid();
            Create(account);
            await SaveAsync();
        }

        public async Task DeleteAccountAsync(Account account)
        {
            Delete(account);
            await SaveAsync();
        }

        public IEnumerable<Account> GetAllAccounts(Guid ownerId)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId));
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            try
            {
                var account = await FindByCondition(o => o.Id.Equals(accountId)).SingleAsync();
                return account;
            }

            catch
            {
                return new Account();
            }
        }

        

        public async Task UpdateAccountAsync(Account dbAccount, Account account)
        {
            dbAccount.Map(account);
            Update(dbAccount);
            await SaveAsync();
        }
    }
}
