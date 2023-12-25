using MyHttpServer.Attributes;
using MyHttpServer.Model;
using MyHttpServer.ORM;
using System.Text.Json;

namespace MyHttpServer.Controllers
{
    [Controller("Account")]
    public class AccountController
    {
        private MyDataContext _myDataContext;
        private List<Account> ? _accounts;

        public AccountController()
        {
            _myDataContext = new MyDataContext();
        }

        [Post("Add")]
        public void Add(string email, string password)
        {
            _accounts = _myDataContext.Select<Account>();
            int new_id = _accounts[_accounts.Count - 1].id + 1;

            _myDataContext.Add<Account>(new Account
            {
                id = new_id,
                email = email,
                password = password
            });
        }

        [Post("Delete")]
        public void Delete(string id)
        {
            _myDataContext.Delete<Account>(int.Parse(id));
        }

        [Post("Update")]
        public void Update(string id, string email, string password)
        {
            Account account = _myDataContext.SelectById<Account>(int.Parse(id));
            account.email = email;
            account.password = password;

            _myDataContext.Update<Account>(account);
        }

        [Get("GetById")]
        public Account? GetById(string id)
        {
            return _myDataContext.SelectById<Account>(int.Parse(id));
        }

        [Get("GetAll")]
        public Account[] GetAll()
        {
            return _myDataContext.Select<Account>().ToArray();
        }
    }
}

