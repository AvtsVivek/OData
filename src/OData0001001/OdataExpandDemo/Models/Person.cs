using System;
using System.Collections.Generic;

namespace OdataExpandDemo.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
    }
    public class BankAccount
    {
        public int Id { get; set; }
        public string BankName { get; set; }
    }
}
