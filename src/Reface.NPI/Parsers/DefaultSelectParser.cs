using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reface.NPI.Parsers
{
    public class DefaultSelectParser : ISelectParser
    {
        private SelectInfo selectInfo;

        public SelectInfo Parse(string command)
        {
            selectInfo = new SelectInfo();
            List<SelectToken> tokens = this.SplitCommandToTokens(command);
            var machine = new SelectStateMachine();
            machine.Parsing += Machine_Parsing;
            foreach (var token in tokens)
                machine.Push(token);
            return selectInfo;
        }

        private void Machine_Parsing(object sender, Events.SelectTokenParsingEvenrArgs e)
        {
            SelectStateMachine machine = (SelectStateMachine)sender;
            ConditionJoiners joiner = ConditionJoiners.And;
            ConditionOperators opr = ConditionOperators.Is;
            SelectToken token;
            string field = "";
            OrderTypes orderTypes = OrderTypes.Asc;
            switch (e.State)
            {
                case SelectParseStates.OutputField:
                    selectInfo.Fields.Add
                        (
                            machine.TokenStacks.Pop().Text
                        );
                    break;
                case SelectParseStates.Condition:
                case SelectParseStates.NextOutputField:
                case SelectParseStates.OrderBy:
                    machine.TokenStacks.Pop();
                    break;
                case SelectParseStates.ConditionField:
                    break;
                case SelectParseStates.ConditionOperator:
                    break;
                case SelectParseStates.NextCondition:
                    if (machine.TokenStacks.Peek().Action == SelectParseActions.Or)
                        joiner = ConditionJoiners.Or;

                    if (machine.TokenStacks.Count == 1)
                    {
                        opr = ConditionOperators.Is;
                        field = machine.TokenStacks.Pop().Text;
                    }

                    if (machine.TokenStacks.Count == 2)
                    {
                        token = machine.TokenStacks.Pop();
                        switch (token.Text)
                        {
                            case SelectToken.TEXT_IS:
                                opr = ConditionOperators.Is;
                                break;
                            case SelectToken.TEXT_LIKE:
                                opr = ConditionOperators.Like;
                                break;
                            case SelectToken.TEXT_GREATER_THAN:
                                opr = ConditionOperators.GreaterThan;
                                break;
                            case SelectToken.TEXT_LESS_THAN:
                                opr = ConditionOperators.LessThan;
                                break;
                            default:
                                break;
                        }
                        field = machine.TokenStacks.Pop().Text;
                    }

                    selectInfo.Conditions.Add
                        (
                            new ConditionInfo
                            (
                                field,
                                opr,
                                joiner
                            )
                        );

                    break;
                case SelectParseStates.OrderByField:
                    break;
                case SelectParseStates.AscOrDesc:
                    token = machine.TokenStacks.Pop();
                    if (token.Text == SelectToken.TEXT_ASC)
                        orderTypes = OrderTypes.Asc;
                    if (token.Text == SelectToken.TEXT_DESC)
                        orderTypes = OrderTypes.Desc;

                    selectInfo.Orders.Add
                        (
                            new OrderInfo
                            (
                                machine.TokenStacks.Pop().Text,
                                orderTypes
                            )
                        );

                    break;
                default:
                    break;
            }
        }

        public List<SelectToken> SplitCommandToTokens(string command)
        {
            List<SelectToken> result = new List<SelectToken>();
            StringBuilder sb = new StringBuilder();
            foreach (var c in command)
            {
                if (!Char.IsUpper(c))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length == 0)
                {
                    sb.Append(c);
                    continue;
                }

                result.Add(SelectToken.Create(sb.ToString()));
                sb.Clear();
                sb.Append(c);
            }
            result.Add(SelectToken.Create(sb.ToString()));
            return result;
        }
    }
}
