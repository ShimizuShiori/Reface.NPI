using Reface.NPI.Models;
using Reface.NPI.Parsers.StateMachines;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;
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
            Console.WriteLine("Tokens : ");
            foreach (var token in tokens)
                Console.WriteLine($"    {token.Action.ToString()} : {token.Text}");
            var machine = new SelectStateMachine();
            machine.Parsing += Machine_Parsing;
            foreach (var token in tokens)
                machine.Push(token);
            return selectInfo;
        }

        private void Machine_Parsing(object sender, Events.TokenParsingEventArgs<States.SelectParseStates> e)
        {
            SelectStateMachine machine = (SelectStateMachine)sender;
            ConditionJoiners joiner = ConditionJoiners.And;
            SelectToken token;
            string field = "";
            OrderTypes orderTypes = OrderTypes.Asc;
            string opr;
            switch (e.NowState)
            {
                case SelectParseStates.OutputField:
                    selectInfo.Fields.Add
                        (
                            machine.TokenStack.Pop().Text
                        );
                    break;
                case SelectParseStates.Condition:
                case SelectParseStates.NextOutputField:
                    machine.TokenStack.Pop();
                    break;
                case SelectParseStates.ConditionField:
                    break;
                case SelectParseStates.OrderBy:
                    machine.TokenStack.Pop();
                    if (e.FromState == SelectParseStates.ConditionField)
                    {
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Conditions.Add(new ConditionInfo(field, ""));
                        break;
                    }
                    if (e.FromState == SelectParseStates.ConditionOperator)
                    {
                        opr = machine.TokenStack.Pop().Text;
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Conditions.Add(new ConditionInfo(field, opr));
                        break;
                    }
                    break;
                case SelectParseStates.ConditionOperator:
                    break;
                case SelectParseStates.NextCondition:
                    token = machine.TokenStack.Pop();
                    if (token.Text == SelectToken.TEXT_OR)
                        joiner = ConditionJoiners.Or;
                    else if (token.Text == SelectToken.TEXT_AND)
                        joiner = ConditionJoiners.And;
                    if (e.FromState == SelectParseStates.ConditionField)
                    {
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Conditions.Add(new ConditionInfo(field, "", joiner));
                        break;
                    }

                    opr = machine.TokenStack.Pop().Text;
                    field = machine.TokenStack.Pop().Text;
                    selectInfo.Conditions.Add(new ConditionInfo(field, opr, joiner));
                    break;
                case SelectParseStates.OrderByField:
                    if (e.FromState == SelectParseStates.OrderByField)
                    {
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Orders.Add(new OrderInfo(field));
                        break;
                    }
                    break;
                case SelectParseStates.AscOrDesc:
                    token = machine.TokenStack.Pop();
                    if (token.Text == SelectToken.TEXT_DESC)
                        orderTypes = OrderTypes.Desc;
                    token = machine.TokenStack.Pop();
                    field = token.Text;
                    selectInfo.Orders.Add(new OrderInfo(field, orderTypes));
                    break;
                case SelectParseStates.End:
                    machine.TokenStack.Pop();
                    if (e.FromState == SelectParseStates.ConditionField)
                    {
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Conditions.Add(new ConditionInfo(field, ""));
                        break;
                    }
                    if (e.FromState == SelectParseStates.ConditionOperator)
                    {
                        opr = machine.TokenStack.Pop().Text;
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Conditions.Add(new ConditionInfo(field, opr));
                        break;
                    }
                    if (e.FromState == SelectParseStates.OrderByField)
                    {
                        field = machine.TokenStack.Pop().Text;
                        selectInfo.Orders.Add(new OrderInfo(field));
                        break;
                    }
                    break;
                default:
                    break;
            }
        }

        public List<SelectToken> SplitCommandToTokens(string command)
        {

            List<SelectToken> result = new List<SelectToken>();
            if (string.IsNullOrEmpty(command))
                return result;
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
            result.Add(SelectToken.CreateEndToken());
            return result;
        }
    }
}
