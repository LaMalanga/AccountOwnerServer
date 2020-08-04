using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extensions
{
    public static class AccountExtension
    {
        public static void Map(this Account dbAccount, Account account)
        {
            dbAccount.AccountType = account.AccountType;
            dbAccount.DateCreated = account.DateCreated;
        }
    }
}
