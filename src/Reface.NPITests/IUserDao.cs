using Reface.NPI;
using Reface.NPI.Attributes;
using Reface.NPI.Generators;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Reface.NPITests
{

    /// <summary>
    /// 这个测试接口，使用 Description 代替了预期结果。
    /// 以便测试每一个方法是否达到预期
    /// </summary>
    public interface IUserDao : INpiDao<User>
    {

        [Description("SELECT * FROM [User] WHERE [Gid] = @Gid")]
        void SelectByGid(Guid gid);

        [Description("SELECT * FROM ( SELECT *,ROW_NUMBER() OVER ( ORDER BY [Createtime] Asc ) AS __RN__ FROM ( SELECT * FROM [User] WHERE [Gid] = @Gid) t ) t WHERE t.__RN__ > @BEGINRN AND t.__RN__ <= @ENDRN")]
        void PagingSelectByGidOrderbyCreatetime(Guid gid, Paging page);


        [Description("SELECT * FROM ( SELECT *,ROW_NUMBER() OVER ( ORDER BY [Createtime] Asc ) AS __RN__ FROM ( SELECT * FROM [User] WHERE [Uid] = @Uid) t ) t WHERE t.__RN__ > @BEGINRN AND t.__RN__ <= @ENDRN")]
        void PagingSelectByUidOrderbyCreatetime(Guid uid);

        [Description("SELECT * FROM ( SELECT *,ROW_NUMBER() OVER ( ORDER BY [Id] Asc ) AS __RN__ FROM ( SELECT * FROM [User]) t ) t WHERE t.__RN__ > @BEGINRN AND t.__RN__ <= @ENDRN")]
        void PagingSelectOrderbyId(Paging page);

        [Description("SELECT * FROM ( SELECT *,ROW_NUMBER() OVER ( ORDER BY [Id] Desc ) AS __RN__ FROM ( SELECT * FROM [User]) t ) t WHERE t.__RN__ > @BEGINRN AND t.__RN__ <= @ENDRN")]
        void PagingSelectOrderbyIdDesc(Paging page);

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

        [Description("DELETE FROM [User] WHERE [Name] = @Name")]
        void DeleteByName(string name);

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

        [Description("UPDATE [User] SET [Loginname] = @Loginname WHERE [Id] = @Id")]
        void UpdateLoginnameById(string name, string id);

        [Description("SELECT * FROM [User] WHERE [Id] In @Id")]
        void GetByIdIn(int[] id);

        [Description("SELECT * FROM [User] WHERE [Uid] In @Uid")]
        void GetByUidIn(List<int> uid);

        #region 带参数的语句生成

        [Description("SELECT * FROM [User] WHERE [Name] Like @Aname Or [Name] Like @Bname Or [Name] Like @Cname")]
        void SelectByNameLikeAnameOrNameLikeBnameOrNameLikeCname();

        [Description("DELETE FROM [User] WHERE [Name] Like @Aname Or [Name] Like @Bname Or [Name] Like @Cname")]
        void DeleteByNameLikeAnameOrNameLikeBnameOrNameLikeCname();

        [Description("UPDATE [User] SET [Password] = @Newpassword WHERE [Userid] = @Userid And [Password] = @Oldpassword")]
        void UpdatePasswordEqualsNewpasswordByUseridAndPasswordIsOldpassword(string newPassword, string userId, string oldPassword);

        #endregion

        #region 带 Without 的 Insert

        [Description("INSERT INTO [User]([Name],[Password])VALUES(@Name,@Password)")]
        void InsertWithoutIdCreatetime(User user);

        #endregion

        #region 不带 Set 的 Update

        [Description("UPDATE [User] SET [Name] = @Name,[Password] = @Password,[CreateTime] = @CreateTime WHERE [Id] = @Id")]
        void UpdateById(User user);


        [Description("UPDATE [User] SET [Name] = @Name,[Password] = @Password WHERE [Id] = @Id")]
        void UpdateWithoutCreatetimeById(User user);

        #endregion

        #region Count 语句

        [Description("SELECT COUNT(*) AS [C] FROM [User] WHERE [Id] = @Id")]
        int CountById(int id);

        #endregion

        #region Query

        [Description("SELECT @@INDENTITY_SCOPE")]
        [SelectQuery("SELECT @@INDENTITY_SCOPE")]
        string ReturnNewId();

        [Description("select * from [a] where id <> @id and name <> @name")]
        [SelectQuery("select * from [a] where id <> @id and name <> @name")]
        int DiyQueryByIdAndName(int id, string name);

        [Description("Delete from [a] where id <> @id")]
        [DeleteQuery("Delete from [a] where id <> @id")]
        int DiyDeleteById(int i);

        #endregion
    }
}
