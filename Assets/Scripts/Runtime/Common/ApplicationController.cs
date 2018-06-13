using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class ApplicationController : MonoSingleton<ApplicationController>
    {
        LuaRuntime luaRuntime;

        bool Initialized = false;

        public void Init()
        {
            //初始化Lua
            //加载各种数据
            //显示登录或者注册界面
            luaRuntime = new LuaRuntime();
            luaRuntime.StartLua(gameObject);
            Initialized = true;

            ABLoader.Instance.LoadFromLocal("login", LoginLoaded);
        }

        public void LoginLoaded(AssetBundle ab)
        {
            GameObject go = Instantiate(ab.LoadAsset("Login")) as GameObject;
            Debug.LogError("Login Loaded");
        }

        public void LuaDoFile(string scriptName)
        {
            if (luaRuntime != null && Initialized)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    luaRuntime.DoFile(Application.dataPath + "/Lua/" + scriptName);
                }
                else
                {
                    if (System.IO.File.Exists(Application.persistentDataPath + "/Lua/" + scriptName))
                    {
                        luaRuntime.DoFile(Application.persistentDataPath + "/Lua/" + scriptName);
                    }
                    else
                    {
                        luaRuntime.DoFile(Application.streamingAssetsPath + "/Lua/" + scriptName);
                    }
                }
            }
        }

        public object[] LuaCall(string module, string name, params object[] args)
        {
            if (luaRuntime != null && Initialized)
            {
                return luaRuntime.Call(module, name, args);
            }
            return null;
        }

        public LuaFunction LuaGetFunction(string module, string func)
        {
            if (luaRuntime != null && Initialized)
            {
                string funcName = module + "." + func;
                return luaRuntime.GetFunction(module, funcName);
            }
            return null;
        }
    }
}
