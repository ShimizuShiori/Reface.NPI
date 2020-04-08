[TOC]

# 什么是 NPI

*NPI* 全名 *.Net Persistent Interface* 。

这是一个利用 *interface* 实现的轻量级 **ORM** 框架，

它与市面上大多数的 **ORM** 框架不同，它不基于 *Linq* 进行数据库操作，而是基于 *Method Name*。

**例如**
```csharp
IList<User> SelectById(int id);

IList<User> SelecyByNameLike(string name);

void UpdatePasswordById(int id, string password);

bool DeleteById(int id);
```

*NPI* 提供了将上述 **MethodName** 和 **实际运算时的入参** 生成 **Sql执行信息** 的方法。

此库不实现以下功能
* 通过 *AOP* 产生 *interface* 的 *Proxy* ( 这个功能会在 *Reface.AppStarter.NPI* 中基于 *Castle.DynamicProxy* 实现 )
* 对 **Sql执行信息** 的执行
* 将查询结果映射到对应的实体中 ( 这个功能会在 *Reface.AppStarter.NPI* 以基于 *Dapper* 实现 )
* 对事务的管理 ( 这个功能预计在一个由 *Reface.AppStarter* 构建的一个业务框架中实现 )

不建议直接将此库用于业务功能的开发，
建议对该库进行一定的二次开发或封装后再投入使用，
开发者可以根据系统当前已经依赖的库进行封装。

**计划在未来转为 .NetStandard 版本，以同时提供给 .NetCore 使用。**


# 依赖项

* *Reface.StateMachine* ( 库中对方法名称解析的过程依赖于此库 )
* *Reface* ( 提供了一些基础的功能和方法，使用 .NetStandard2.0 编写 )

由于使用了 .NetStrand2.0，因此本库需要 .Net framework 4.6.1 及以上版本才能使用。

# 使用方法

## 四个分析器

系统中针对四种数据库不同的操作（增删改查），分别提供了四个不同的语义分析器
* ISelectParser
* IInsertParser
* IUpdateParser
* IDeleteParser

这四种转化器能够将一个字符串分析成为结构化的数据库处理结构，如
```csharp
ISelectParser parser = new DefaultSelectParser();
string command = "ByIdAndName";
SelectInfo info = parser.Parse(command);
// info.Fields = [];
// info.Conditions[0].Field = "Id";
// info.Conditions[1].Field = "Name";
// info.Orders = [];
```

四种分析器分别能生成四种不同的语句结构：
| Parser | 结果的类型 |
|---|---|
| ISelectParser | SelectInfo |
| IInsertParser | InsertInfo |
| IUpdateParser | UpdateInfo |
| IDeleteParser | DeleteInfo |

这些 *xxxInfo* 的结构并不复杂，这里将不对其展开进行更多的介绍。

## 四个分析器的整合

*ICommandParser* 对四个分析器做了整合，以便我们不关心对方法的区分而直接得到 *ICommandInfo* , *SelectInfo* , *InsertInfo* , *UpdateIfo* 和 *DeleteInfo* 都实现了 *ICommandInfo* 接口。

*ICommandParser* 通过方法的第一个单词对方法名进行分类，哪些前缀属于查询、哪些前缀属于更新，都是由它的实现的。

库中的 *DefaultCommandParser* 按照下面的进行逻辑区分 :

**查询语句**
* Get
* Select
* Fetch
* Find
* PagingGet
* PagingSelect
* PagingFetch
* PagingFind

**新增语句**
* Insert
* New
* Create

**更新语句**
* Update
* Modify

**删除语句**
* Delete
* Remove

下面的例子是分析了一个更新语句。
*ICommandInfo* 中的 *Type* 字段有助于你判断应当转化至四个 *Info* 中的哪一个。

```csharp
string command = "UpdateNameById";
ICommandParser parser = new DefaultCommandParser();
ICommandInfo info = parser.Parse(command);
// info.Type = CommandInfos.Update
UpdateInfo updateInfo = (UpdateInfo)info;
// updateInfo.SetFields[0].Field = "Name";
// updateInfo.Conditions[0].Field = "Id";
```

## 通过方名和参数生成执行信息

执行信息包含两个信息
* Sql 语句
* Sql 参数

在库中，生成执行信息是由 *ISqlCommandGenerator* 完成的。
```csharp
// ISqlCommandGenerator.cs
using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface ISqlCommandGenerator
    {
        SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments);
    }
}
```

设计该接口的初衷是希望使用方是以 **AOP** 的方式拦截某个方法的执行，
并将 *MethodInfo* 和 **拦截到的入参** 传递给 *ISqlCommandGenerator*，
再根据生成的执行信息直接执行，得到结果。

目前库中有一个它的实现类型 : *DefaultSqlServerCommandGenerator* 。
你会从名字上发现，它是面向 *SqlServer* 的实现，
很明显，不同的 **数据库** 往往支持的语句并不相同。
因此，为不同的 **数据库** 编写不同的 *ISqlCommandGenerator* 是有必要的。

*SqlCommandDescripion* 是一个简单的数据结构，它包含 *SqlCommand* 和 *Parameters* 两个主要的属性，使用这两个属性可以完成后续的 *Sql* 执行。

# 注意事项

* 对于表名的获取，是基于 **INpiDao&lt;TEntity&gt;** 中的 TEntity 来完成的，
通过反射 TEntity 上的 **System.ComponentModel.DataAnnotations.Schema.TableAttibute** 特征来获取表名。
* 由于 **TableAttribute** 在很多常见库中出现，所以要注意不要引用错了。
* *Reface.NPI* 允许你重写对表名的获取，对字段的获取等逻辑，重写方法会在后面的文章中介绍。

# 方法名及预期 Sql 参照表


| 方法名称 | 期望的 Sql | 说明 |
|---|---|---|
| SelectById | select * from [table] where id = ? | 以 Id 作为条件查询实体 |
| SelectNameAndAgeById | select name, age from [table] where id = ? | 以 Id 作为条件，只查询 Name 和 Age 字段 |
| SelectByRegistertimeGreaterthan | select * from [table] where Registertime > ? | 查询 RegisterTime 大于参数的实体 |
| SelectByRegistertimeGteq | select * from [table] where Registertime >= ? |查询 RegisterTime 大于等于参数的实体 |
| SelectByIdAndName | select * from [table] where id = ? and name = ? | 按 Id 且 Name 作为条件查询实体 |
| SelectByIdOrName | select * from [table] where id = ? or name = ? | 以 Id 或 Name 作为条件查询实体 |
| SelectByIdOrNameLike | select * from [table] where id = ? or name like ? | 以 Id 或 Name Like 作为条件查询实体 |
| SelectByIdOrderbyName | select * from [table] where id = ? order by name | 以 Id 查询并以 Name 排序 |
| SelectByIdOrderByNameDesc | select * from [table] where id = ? order by name desc | 以 Id 作为条件并以 Name 倒序排序 |
| DeleteById | delete from [table] where id = ? | 以 Id 作为条件删除 |
| UpdatePasswordById | update [table] set password = ? where id = ? | 以 Id 作为条件更新 Password |
| UpdatePasswordByNameLike | update [table] set password = ? where name like ? | 以 Name Like 作为条件更新 Password |
| UpdateStateAndTokenByLastoprtimeGt | update [table] set state=?,token=? where lastoprtime > ? | 以 LastOprTime 大于 作为条件更新 state 和 token |
| UpdateById | update [table] set ... where id = ? | 以 Id 作为条件，并以 Id 以外的字段作为 Set 子句 |
| UpdateWithoutCreatetimeById | update [table] set ... where id = ? | 以 Id 作为条件，并以 Id 和 Createtime 以外的字段作为 Set 子句 |
| UpdateWithoutStateCreatetimeById | update [table] set ... where id = ? | 以 Id 作为条件，并以 Id 、State 和 Createtime 以外的字段作为 Set 子句 |
| InsertWithoutIdCreatetime(Entity) | insert into [table] (...) values(?,...,?) | 排除 Id 和 Createtime 字段新增实现 |

**相关链接**

[Reface.NPI nuget 页面](https://www.nuget.org/packages/Reface.StateMachine)