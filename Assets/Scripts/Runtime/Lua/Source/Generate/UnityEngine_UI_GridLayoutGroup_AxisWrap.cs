﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_UI_GridLayoutGroup_AxisWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(UnityEngine.UI.GridLayoutGroup.Axis));
		L.RegVar("Horizontal", get_Horizontal, null);
		L.RegVar("Vertical", get_Vertical, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
		TypeTraits<UnityEngine.UI.GridLayoutGroup.Axis>.Check = CheckType;
		StackTraits<UnityEngine.UI.GridLayoutGroup.Axis>.Push = Push;
	}

	static void Push(IntPtr L, UnityEngine.UI.GridLayoutGroup.Axis arg)
	{
		ToLua.Push(L, arg);
	}

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(typeof(UnityEngine.UI.GridLayoutGroup.Axis), L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Horizontal(IntPtr L)
	{
		ToLua.Push(L, UnityEngine.UI.GridLayoutGroup.Axis.Horizontal);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Vertical(IntPtr L)
	{
		ToLua.Push(L, UnityEngine.UI.GridLayoutGroup.Axis.Vertical);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		UnityEngine.UI.GridLayoutGroup.Axis o = (UnityEngine.UI.GridLayoutGroup.Axis)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}
