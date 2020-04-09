# 方法名规则详解

**基本规则**

* 方法名称以 **Insert , Delete , Select , Update** 开头
* 方法名以数个单词构成，关键字、字段、操作符、参数名都是一个单词
* 每个单词以大写开头，其余全小写
    * UserName 会被认作两个单词
    * Username 会被认作一个单词
* 使用入参填充 **Sql参数** 时，不计大小写，你可以用 *id* 填充 *@Id* 的值

## 1 *Insert* 规则

仅使用 *Insert* 将使用实体中的所有字段的值写入。

```csharp
void Insert(Entity entity);
```

开发者也可以通过 *Without* 关键字排除一些字段的写入，特别是那些依赖数据库本身的字段。

下面的例子不会对 *Id* 和 *CreateTime* 字段写入
```csharp
bool InsertWithoutIdCreatetime(User user);
```


## 2 *Delete* 规则

*Delete* 方法名包含一个 **条件规则**。

**条件规则** 允许开发者定义一个 *where* 子语。


### 2.1 使用 *By* 声明一个 **条件规则**

要在方法名中声明一个条件，必须以 *By* 作为开始。

下面的方法，将会生成 *where Id = @Id* , 并以入参的 *id* 作为 *@Id* 的值。
```csharp
void DeleteById(int id);
```

### 2.2 使用 *And* 和 *Or* 合并多个条件

你也可以使用 *And* 和 *Or* 合并多个条件

下面两个方法将会生成 *where Id = @Id and/or Name = @Name* 并以入参的 *id* 和 *name* 分别作用 *@Id* 和 *@Name* 的值。

```csharp
bool DeleteByIdAndName(int id, string name)

bool DeleteByIdOrName(int id, string name)
```

**注意**

* 因为没有想到一个好的方法可以对条件进行分组，所以目前不可以声明条件组，形如 *where ( Id = @Id and Name = @Name ) or ( State = @State and Loginname = @Loginname )*。

**如果您有好的想法，您也可以告诉我，共同完善 *Library* 。非常期待您的分享，感谢！**

### 2.3 更多的操作符

很明显，我们不可能总是用 *=* 作为条件的判断操作，
我们还有 大于，小于，Like等等。

你可以在字段名后面加上操作符来实现此功能。

下面的例子会生成 *where Name Like @Name* 的条件。

```csharp
int DeleteByNameLike(string name)
```

目前系统中支持的操作符有

| Sql | Method |
|---|---|
| = | Is , Equal , Equals |
| > | Greaterthan , Gt |
| >= | Greaterthanandequals , Gteq |
| < | Lessthan , Lt |
| <= | Lessthanandequals , Lteq |
| Like | Like , Likes |
| In | In |

### 2.4 自定义参数名

在上面的例子中，参数名直接与字段名相同。

我们也可以在操作符后加上参数名来改名这个默认的参数名。

下面的例子会生成 *where Password Like @Badpassworda Or Password Like @Badpasswordb*
```csharp
int DeleteByPasswordLikeBadpasswordaOrPasswordLikeBadpasswordb(string badPasswordA, string badPasswordB);
```

## 3 *Update* 规则

*Update* 规则由两个部分组成
* 条件规则 ( 与 *Delete* 规则相同)
* *Set* 规则

**Set 规则**

### 3.1 指定 *set* 的字段

*Update* 关键字后接的部分，一直到 *By* 之前，都是 *Set* 的字段。

下面的例子会生成 *set Password = @Password where Id = @Id*

```csharp
void UpdatePasswordById(int id, string password);
```

### 3.2 多个 *set* 

你可以用 *And* 连接多个 *set*。

下面的例子会生成 *set Password = @Password , Changingtime = @Changingtime where Id = @Id*。

```csharp
bool UpdatePasswordAndChangingtimeById(int id, string password, DateTime changingTime);
```

### 3.3 自定义 *set* 参数

和 **条件规则** 一样，生成的语句中，会默认使用字段名作为参数名。

你也可以在字段后添加 *Equals* 再加上 参数名称 来自定义参数名。

下面的例子会生成 *set Count = @Newcount where Id = @Id And Count = @Oldcount*

```csharp
int UpdateCountEqualsNewcountByIdAndCountIsOldcount(int id, int oldCount, int newCount);
```

### 3.4 不指定 *set* 子句

当没有 *set* 子句的时候，
会以排除了 *By* 子句的条件后的所有字段作为 *set* 子句。

下面的例子会生成 *update [user] set name = ?, password = ? where id = ?*
```csharp
// User : Id, Name, Password
int UpdateById(int id, User user);
```

若表中还有一些字段在 *Update* 时即不是条件，也不打算更新。可以使用 *Without* 关键字指定。
下面的例子中，User 包含四个属性
* Id
* Name
* Password
* CreateTime

```csharp
// 下面的语句不会对 CreateTime 进行更新
void UpdateWithoutCreatetimeById(int id, User user);
```


## 4 *Select* 规则

**Select规则** 包含以下三个规则
* 条件规则 ( 与 *Delete* 和 *Update* 相同 )
* 输出字段规则
* 排序规则
* 分页查询

## 4.1 输出字段

这个规则比较简单，
只要把字段列在 *Select* 后即可，
多个字段可以用 *And* 连接。
输出字段是可选的，你可以跳过这个部分直接编写条件。

```csharp
IList<Entity> SelectIdAndNameAndCreatetime();
```

## 4.2 条件规则
与 *Update* 和 *Delete* 一样，使用 *By* 关键字开始条件子句

```csharp
User SelectById(int id);
```

### 4.3 排序规则

#### 4.3.1 *Orderby* 子句

排序规则是由关键字 *Orderby* 开头的 ( 除了 O 都是 小写 )。

下面的例子会生成 *ORDER BY Id Asc / Desc*

```csharp
IList<Entity> SelectOrderbyId();
IList<Eneity> SelectOrderbyIdDesc();
```

#### 4.3.2 多个排序

多个排序不需要使用 *And* 连接，直接拼接即可。
```csharp
IList<User> SelectOrderbyUsernameCreatetime();
```

#### 4.3.3 分页查询

为 *Select* 方法前加上 *Paging* 就可以使用分页查询功能。

**注意**

* 使用分页查询时，必须提供类型为 *Paging* 的参数。

```csharp
IList<Order> PagingSelectByCreatetimeGt(DateTime createTime);
```

---
[Home](../readme.md)