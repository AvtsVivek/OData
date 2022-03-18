using System;
using System.Collections.Generic;

namespace OdataExpandDemo.Tests
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int Score { get; set; }
        public List<BankAccountModel> BankAccounts { get; set; } = default!;
    }

    public class BankAccountModel
    {
        public int Id { get; set; }
        public string BankName { get; set; } = default!;
    }

}
