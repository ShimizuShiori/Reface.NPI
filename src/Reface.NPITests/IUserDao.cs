using Reface.NPI.Attributes;
using System.ComponentModel;

namespace Reface.NPITests
{

    [Table("User")]
    public interface IUserDao
    {
        [Description("SELECT * FROM [User] WHERE [Id] = @Id")]
        void SelectById(int id);

        [Description("SELECT * FROM [User] WHERE [Id] = @Id")]
        void GetById(int id);

        [Description("SELECT [Name] FROM [User] WHERE [Id] = @Id")]
        void GetNameById(int id);

        [Description("SELECT [Id],[Name] FROM [User] WHERE [Birthday] > @Birthday")]
        void GetIdAndNameByBirthdayGreaterthan(int id);

        [Description("SELECT [Id],[Name] FROM [User] WHERE [Birthday] > @Birthday ORDER BY [Sn] Asc")]
        void GetIdAndNameByBirthdayGreaterthanOrderbySn(int id);

        [Description("SELECT [Id],[Name] FROM [User] WHERE [Birthday] > @Birthday ORDER BY [Sn] Asc,[Type] Desc")]
        void GetIdAndNameByBirthdayGreaterthanOrderbySnTypeDesc(int id);

        [Description("DELETE FROM [User] WHERE [Id] = @Id")]
        void DeleteById(int id);

        [Description("DELETE FROM [User] WHERE [Id] = @Id And [State] = @State")]
        void DeleteByIdAndState(int id);

        [Description("UPDATE [User] SET [Password] = @Password WHERE [Id] = @Id")]
        void UpdatePasswordById(string password, string id);


        [Description("UPDATE [User] SET [Password] = @Password,[Name] = @Name WHERE [Id] = @Id")]
        void UpdatePasswordAndNameById(string password, string id);


        [Description("UPDATE [User] SET [Password] = @Password,[Name] = @Name WHERE [Id] = @Id And [Uid] = @Uid")]
        void UpdatePasswordAndNameByIdAndUid(string password, string id);
    }
}
