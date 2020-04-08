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
建议使用该接口，可以减少对 实体分析、表名分析、字段名分析 等操作。

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

### 3.6 Providers

一共包括以下几样

* IEntityTypeProvider
* ITableNameProvider
* IFieldNameProvider

这三样，分别是在生成 **SqlCommandDescription** 阶段时获取实体类型、表名、字段名使用的组件。

目前是按照 INpiDao&lt;TEntity>, TableAttribute, ColumnAttribute 的逻辑实现的。

开发者可以根据自己的需要替换这些逻辑，使用 **NpiServicesCollection.ReplaceService** 可以替换这些默认的组件

