﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ScrapInsurance.Util
{
    internal static class Tools
    {
        static Terminal terminal = null;
        public static void FindCodeInstruction(ref int index, ref List<CodeInstruction> codes, object findValue, MethodInfo addCode, bool skip = false, bool requireInstance = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, string errorMessage = "Not found")
        {
            bool found = false;
            for (; index < codes.Count; index++)
            {
                if (!CheckCodeInstruction(codes[index], findValue)) continue;
                found = true;
                if (skip) break;
                if (andInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.And));
                if (!andInstruction && orInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.Or));
                if (notInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.Not));
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, addCode));
                if (requireInstance) codes.Insert(index + 1, new CodeInstruction(OpCodes.Ldarg_0));
                break;
            }
            if (!found) Plugin.mls.LogError(errorMessage);
            index++;
        }
        public static int FindLocalField(int index, ref List<CodeInstruction> codes, int localIndex, object addCode, bool skip = false, bool store = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            bool found = false;
            for (; index < codes.Count; index++)
            {
                if (!CheckCodeInstruction(codes[index], localIndex, store)) continue;
                found = true;
                if (skip) break;
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, addCode));
                if (requireInstance) codes.Insert(index + 1, new CodeInstruction(OpCodes.Ldarg_0));
                break;
            }
            if (!found) Plugin.mls.LogError(errorMessage);
            return index + 1;
        }
        public static void FindString(ref int index, ref List<CodeInstruction> codes, string findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findValue, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindField(ref int index, ref List<CodeInstruction> codes, FieldInfo findField, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findField, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindMethod(ref int index, ref List<CodeInstruction> codes, MethodInfo findMethod, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findMethod, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindFloat(ref int index, ref List<CodeInstruction> codes, float findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findValue, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindInteger(ref int index, ref List<CodeInstruction> codes, sbyte findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findValue, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindSub(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Sub, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        public static void FindDiv(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Div, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        public static void FindAdd(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Add, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        public static void FindMul(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Mul, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        private static bool CheckCodeInstruction(CodeInstruction code, int localIndex, bool store = false)
        {
            if (!store)
            {
                switch (localIndex)
                {
                    case 0: return code.opcode == OpCodes.Ldloc_0;
                    case 1: return code.opcode == OpCodes.Ldloc_1;
                    case 2: return code.opcode == OpCodes.Ldloc_2;
                    case 3: return code.opcode == OpCodes.Ldloc_3;
                    default: return code.opcode == OpCodes.Ldloc && (int)code.operand == localIndex;
                }
            }
            else
            {
                switch (localIndex)
                {
                    case 0: return code.opcode == OpCodes.Stloc_0;
                    case 1: return code.opcode == OpCodes.Stloc_1;
                    case 2: return code.opcode == OpCodes.Stloc_2;
                    case 3: return code.opcode == OpCodes.Stloc_3;
                    default: return code.opcode == OpCodes.Stloc && (int)code.operand == localIndex;
                }
            }
        }
        private static bool CheckCodeInstruction(CodeInstruction code, object findValue)
        {
            if (findValue is sbyte)
            {
                bool result = CheckIntegerCodeInstruction(code, findValue);
                return result;
            }
            if (findValue is float)
            {
                bool result = code.opcode == OpCodes.Ldc_R4 && code.operand.Equals(findValue);
                return result;
            }
            if (findValue is string) return code.opcode == OpCodes.Ldstr && code.operand.Equals(findValue);
            if (findValue is MethodInfo) return (code.opcode == OpCodes.Call || code.opcode == OpCodes.Callvirt) && code.operand == findValue;
            if (findValue is FieldInfo) return (code.opcode == OpCodes.Ldfld || code.opcode == OpCodes.Stfld) && code.operand == findValue;
            if (findValue is OpCode) return code.opcode == (OpCode)findValue;
            return false;
        }
        private static bool CheckIntegerCodeInstruction(CodeInstruction code, object findValue)
        {
            switch ((sbyte)findValue)
            {
                case 0: return code.opcode == OpCodes.Ldc_I4_0;
                case 1: return code.opcode == OpCodes.Ldc_I4_1;
                case 2: return code.opcode == OpCodes.Ldc_I4_2;
                case 3: return code.opcode == OpCodes.Ldc_I4_3;
                case 4: return code.opcode == OpCodes.Ldc_I4_4;
                case 5: return code.opcode == OpCodes.Ldc_I4_5;
                case 6: return code.opcode == OpCodes.Ldc_I4_6;
                case 7: return code.opcode == OpCodes.Ldc_I4_7;
                case 8: return code.opcode == OpCodes.Ldc_I4_8;
                default:
                    {
                        return code.opcode == OpCodes.Ldc_I4_S && code.operand.Equals(findValue);
                    }
            }
        }
        public static void ShuffleList<T>(List<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool SpawnMob(string mob, Vector3 position, int numToSpawn) // this could be moved to tools
        {
            for (int i = 0; i < RoundManager.Instance.currentLevel.Enemies.Count; i++)
            {
                if (RoundManager.Instance.currentLevel.Enemies[i].enemyType.enemyName == mob)
                {
                    for (int j = 0; j < numToSpawn; j++)
                    {
                        RoundManager.Instance.SpawnEnemyOnServer(position, 0f, i);
                    }
                    return true;
                }
            }
            return false;
        }

        internal static string GenerateInfoForUpgrade(string infoFormat, int initialPrice, int[] incrementalPrices, Func<int, float> infoFunction)
        {
            string info = string.Format(infoFormat, 1, initialPrice, infoFunction(0));
            for (int i = 0; i < incrementalPrices.Length; i++)
            {
                float infoResult = infoFunction(i + 1);
                if (infoResult % 1 == 0) // It's an Integer
                    info += string.Format(infoFormat, i + 2, incrementalPrices[i], Mathf.RoundToInt(infoResult));
                else
                    info += string.Format(infoFormat, i + 2, incrementalPrices[i], infoResult);
            }
            return info;
        }

        public static Color ConvertValueToColor(string hex, Color defaultValue)
        {
            if (hex == null || !ColorUtility.TryParseHtmlString("#" + hex.Trim('#', ' '), out Color color))
                return defaultValue;
            return color;
        }

        internal static string WrapText(string text, int availableLength, string leftPadding = "", string rightPadding = "", bool padLeftFirst = true)
        {
            int actualLength = availableLength - leftPadding.Length - rightPadding.Length;
            string result = "";
            string currentLine = "";
            int currentLinePosition = 0; // due to use of HTML tags
            int possibleWrap = -1;
            bool first = true;
            bool HTMLTag = false;
            for (int characterIndex = 0; characterIndex < text.Length; characterIndex++)
            {
                char character = text[characterIndex];
                if (character == '<')
                    HTMLTag = true;
                if (character == ' ' && !HTMLTag)
                {
                    possibleWrap = currentLine.Length;
                }
                if (character != '\n')
                {
                    currentLine += character;
                    if (!HTMLTag) currentLinePosition++;
                }
                if (character == '>' && HTMLTag) HTMLTag = false;
                if (currentLinePosition >= actualLength || character == '\n')
                {
                    if (character != '\n' && character != ' ')
                    {
                        if (possibleWrap != -1)
                        {
                            string newText = (padLeftFirst || !first ? leftPadding : "") + currentLine.Substring(0, possibleWrap) + new string(' ', Mathf.Max(0, actualLength - possibleWrap)) + rightPadding;
                            result += newText + '\n';
                            currentLine = currentLine.Substring(possibleWrap + 1);
                        }
                        else
                        {
                            string newText = (padLeftFirst || !first ? leftPadding : "") + currentLine + rightPadding;
                            result += newText + "\n";
                            currentLine = "";
                        }
                    }
                    else
                    {
                        if (currentLine != "") result += (padLeftFirst || !first ? leftPadding : "") + currentLine + new string(' ', Mathf.Max(0, actualLength - currentLinePosition)) + rightPadding + '\n';
                        currentLine = "";
                    }
                    possibleWrap = -1;
                    first = false;
                    currentLinePosition = currentLine.Length;
                }

            }
            if (currentLine != "") result += (padLeftFirst || !first ? leftPadding : "") + currentLine + new string(' ', Mathf.Max(0, actualLength - currentLinePosition)) + rightPadding + '\n';
            return result;
        }
        internal static void SpawnExplosion(Vector3 explosionPosition, bool spawnExplosionEffect = false, float killRange = 1f, float damageRange = 1f, int nonLethalDamage = 50, float physicsForce = 0f, GameObject overridePrefab = null)
        {
            Landmine.SpawnExplosion(explosionPosition, spawnExplosionEffect, killRange, damageRange, nonLethalDamage, physicsForce, overridePrefab);
        }

        internal static Terminal GetTerminal()
        {
            if (terminal == null) terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
            return terminal;
        }
    }
}
