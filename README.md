# Reface.NPI

## 1 简介

**.Net Persistent Interface**，简称 **NPI**

编写此库是为了能够在不编写 Sql 的前提下，灵活的对数据进行增删改查。

该库主要实现的功能是从 **方法名称** 解析出最终的 Sql 语句的过程，并不实现对 Sql 的执行，对数据库返回结果解析的逻辑。

该库不直接面向业务实现，而是面向功能库提供核心的功能。

开发者可以使用此库进行二次开发

* 重次定义方法名的解析过程
* 对不同的数据库进行不同的 Sql 生成
* 后续的 Sql 执行
* 后续的结果映射
* 更多的后续操作

## 2 依赖项

* Reface.StateMachine, 提供了对方法名解析的状态下推
* Reface, 提供了一些基础的功能和方法，使用 .NetStandard2.0 编写

因此本库需要 .Net framework 4.6.1 及以上版本才能使用。

## 3 成员介绍

### 3.1 ISqlCommandGenerator

这个接口是面向外部的最主要的接口，通过传入的方法信息和参数列表，返回一个待执行的 SqlCommandDescription，它包含一个方法。

```csharp
SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments);
```

目前这个接口有两个实现类
* SqlCommandGeneratorBase
* DefaultSqlServerCommandGenerator

#### 3.1.1 SqlCommandGeneratorBase

这是 **ISqlCommandGenerator** 的基本实现，
它实现的逻辑是：
1. 将 MethodInfo 通过前缀分别进行 Select、Udpate、Delete 方法的名称进行解析，对于 Insert 的情况，不需要解析，解析结果以 ICommandInfo 的形式表达。对 Select、Update、Delete 的解析分别使用相应的 IParser 完成
    * ISelectParser
    * IUpdateParser
    * IDeleteParser
2. 根据不同的 ICommandInfo.Type 分别交给子类的 GenerateSelect, GenerateUpdate, GenerateDelete, GenerateInsert 来生成 SqlCommand
3. 根据 MethodInfo 中的参数信息 和 arguments 开始填充执行参数。填充过程由 IParameterLookup 完成，IParameterLookup 由多个，并通过 IParameterLookupFactory 来进行调度。

#### 3.1.2 DefaultSqlServerCommandGenerator

继承于 **SqlCommandGeneratorBase**。
是面向 SqlServer 的实现。


### 3.2 NpiServicesCollection

该库的服务集合，这是一个 IOC 组件，用于解除组件之间的耦合性，让它们仅通过接口类型进行耦合。
该类型提供了四个静态方法
```csharp
// 注册一个组件，如果 T 已经 ServicesCollection 中存在，则追加
void RegsiterService<T>(Func<T, object> factory);

// 替换一个组件，如果 T 已经存在，则替换，二次开发可以利用这个方法替换默认的组件
void ReplaceService<T>(Func<T, object> facotory);

// 获取一个服务实例
T GetService<T>();

// 获取多个服务实现，通过 RegisterService 可以注册多个相同 T 的组件，并通过这个方法获取它们
IEnumerable<T> GetServices<T>();
```

### 3.3 INpiDao&lt;TEntity&gt;

期望的数据层访问接口形式。
目前主要用途是通过该接口获取到实现类型，以便生 Insert 语句中的字段信息。
后期可以考虑优化成别的形式，以减少对上层引用的要求，提高透明性。

### 3.4 IParameterLookup

该组件负责从方法的参数中查询 Sql 语句的参数，并填充到 SqlCommandDescription 中。
开发者可以实现自己的 IParameterLookup, 并通过 NpiServicesCollection.RegsiterService 方法注册到服务集合中。
当 **SqlCommandGeneratorBase** 开始解析参数时，会从 **ServicesCollection** 得到它们，并运用它们解析参数。
该接口提供的两个方法分别是用来匹配和查询的。匹配用来决定该组件在什么条件下可以进行查询，查询则是具体的过程。

### 3.5 Resources文件夹

文档结构如下 : 
```shell
- Resources
    - OperatorMappings # 这是用来将方法名称中的条件运算符转化为 Sql 条件运算符的映射文件
        - SqlServer.xml # 这是用于 SqlServer 的映射
    - StateMachines # 这里存在的是解析语句用到的状态机
        - Select.csv
        - Update.csv
        - Delete.csv
```

状态机的定义方法可以参见这个库 : [Reface.StateMachine](https://github.com/ShimizuShiori/Reface.StateMachine)

## 4 注意事项

对于表名的获取，是基于 **INpiDao&lt;TEntity&gt;** 中的 TEntity 来完成的，
通过反射 TEntity 上的 **System.ComponentModel.DataAnnotations.Schema.TableAttibute** 特征来获取表名。
由于 **TableAttribute** 在很多库中，所以要注意不要引用错了。

## 5 方法名规则

| 方法名称 | 期望的 Sql |
|---|---|
| SelectById | select * from [table] where id = ? |
| SelectNameAndAgeById | select name, age from [table] where id = ? |
| SelectByRegistertimeGreaterthan | select * from [table] where Registertime > ? |
| SelectByRegistertimeGteq | select * from [table] where Registertime >= ? |
| SelectByIdAndName | select * from [table] where id = ? and name = ? |
| SelectByIdOrName | select * from [table] where id = ? or name = ? |
| SelectByIdOrNameLike | select * from [table] where id = ? or name like ? |
| SelectByIdOrderbyName | select * from [table] where id = ? order by name |
| SelectByIdOrderByNameDesc | select * from [table] where id = ? order by name desc |
| DeleteById | delete from [table] where id = ? |
| UpdatePasswordById | update [table] set password = ? where id = ? |
| UpdatePasswordByNameLike | update [table] set password = ? where name like ? |
| UpdateStateAndTokenByLastoprtimeGt | update [table] set state=?,token=? where lastoprtime > ? |

说明:
* Select、Update、Delete 中条件的部分的解析模式是相同的
* Insert 语句不涉及，因为模式单一
* By部分的，不写操作符视作 Equals
* Select 与 Get、Fetch 解析相同
* Insert 与 Create、New 相同
* Update 与 Modify 相同
* Delete 与 Remove 相同