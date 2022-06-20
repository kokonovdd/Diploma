interface IUserService
{
    bool CheckAuth(string user_login, string user_pass);

    int Create(Users user);

    void Delete(int user_id);

    Users GetById(int user_id);

    Users GetByLogin(string user_login);

    Users EldestByUser(string user_login);

    Users EldestByBlock(int block_id);

    IEnumerable<Users> GetAll();


    void Update(Users user);

}