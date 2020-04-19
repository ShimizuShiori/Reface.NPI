# 一些已知的问题

**在 SqlServer 中使用 Insert 语句后，使用 Scope_Identity 会抛出异常**

由于参数化查询是使用的数据库中的 *sp_sqlexecute* 实现的。
因此，在 *sp_sqlexecute* 完全后，再使用 *Scope_Identity()* 将无法返回最后的 Id。
**建议** ，使用 *@@Identity* 代替 *Scope_Identity()* ，并不要在数据库中使用触发器，触发器会影响 **@@Identity** 的结果。

---
[Home](../readme.md)