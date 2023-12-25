using MyHttpServer.Attributes;
using MyHttpServer.Model;
using System.Text.Json;

namespace MyHttpServer.Controllers
{
    [Controller("Account")]
    public class AccountController
    {
        private const string _accountsFilePath = "accounts_list.json";
        private List<Account> ? _accounts;

        public AccountController()
        {
            GetAccountsFromJson();
        }

        private void GetAccountsFromJson()
        {
            if (!File.Exists(_accountsFilePath))
            {
                Console.WriteLine("json file not found!");
                throw new FileNotFoundException("json file not found!");
            }

            using (FileStream file = File.OpenRead(_accountsFilePath))
            {
                _accounts = JsonSerializer.Deserialize<List<Account>>(file);
            }
        }

        [Post("Add")]
        public void Add(string email, string password)
        {
            int new_id = _accounts[_accounts.Count - 1].id + 1;

            _accounts.Add(new Account
            {
                id = new_id,
                email = email,
                password = password
            });

            string json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_accountsFilePath, json);
        }

        [Post("Delete")]
        public void Delete(string id)
        {
            foreach(var account in _accounts)
            {
                if (account.id == int.Parse(id))
                {
                    _accounts.Remove(account);
                    break;
                } 
            }

            string json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_accountsFilePath, json);
        }

        [Post("Update")]
        public void Update(string id, string email, string password)
        {
            foreach (var account in _accounts)
            {
                if (account.id == int.Parse(id))
                {
                    account.email = email;
                    account.password = password;
                    break;
                }
            }

            string json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_accountsFilePath, json);
        }

        [Get("GetById")]
        public Account? GetById(string id)
        {
            Account acc = new Account();

            foreach (var account in _accounts)
            {
                if (account.id == int.Parse(id))
                {
                    acc = account;
                    break;
                }   
            }

            return acc;
        }

        [Get("GetAll")]
        public Account[] GetAll()
        {
            return _accounts.ToArray();
        }
    }
}

