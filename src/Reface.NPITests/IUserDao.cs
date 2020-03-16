using Reface.NPI;
using System;
using System.ComponentModel;

namespace Reface.NPITests
{

    public interface IUserDao : INpiDao<User>
    {
        [Description("SELECT * FROM [User] WHERE [Id] = @Id")]
        void SelectById(int id);

        [Description("SELECT * FROM [User] WHERE [Id] = @Id")]
        void GetById(int id);

        [Description("SELECT [Name] FROM [User] WHERE [Id] = @Id")]
        void GetNameById(int id);

        [Description("SELECT [Id],[Name] FROM [User] WHERE [Birthday] > @Birthday")]
        void GetIdAndNameByBirthdayGreaterthan(DateTime birthday);

        [Description("SELECT [Id],[Name] FROM [User] WHERE [Birthday] > @Birthday ORDER BY [Sn] Asc")]
        void GetIdAndNameByBirthdayGreaterthanOrderbySn(DateTime birthday);

        [Description("SELECT [Id],[Name] FROM [User] WHERE [Birthday] > @Birthday ORDER BY [Sn] Asc,[Type] Desc")]
        void GetIdAndNameByBirthdayGreaterthanOrderbySnTypeDesc(DateTime birthday);

        [Description("DELETE FROM [User] WHERE [Id] = @Id")]
        void DeleteById(int id);

        [Description("DELETE FROM [User] WHERE [Id] = @Id And [State] = @State")]
        void DeleteByIdAndState(int id);

        [Description("UPDATE [User] SET [Password] = @Password WHERE [Id] = @Id")]
        void UpdatePasswordById(string password, string id);


        [Description("UPDATE [User] SET [Password] = @Password,[Name] = @Name WHERE [Id] = @Id")]
        void UpdatePasswordAndNameById(string password, string name, string id);


        [Description("UPDATE [User] SET [Password] = @Password,[Name] = @Name WHERE [Id] = @Id And [Uid] = @Uid")]
        void UpdatePasswordAndNameByIdAndUid(string password, string name, string id, string uid);

        [Description("INSERT INTO [User]([Id],[Name],[Password],[CreateTime])VALUES(@Id,@Name,@Password,@CreateTime)")]
        void Insert(User user);
    }
}
