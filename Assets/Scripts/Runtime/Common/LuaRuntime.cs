using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class LuaRuntime
    {
        public bool Initialized;

        private LuaState luaState = null;
        private LuaLooper luaLooper = null;

        public void StartLua(GameObject go)
        {
            luaState = new LuaState();

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        luaState.OpenLibs(LuaDLL.luaStateopen_bit);
#endif

            luaState.OpenLibs(LuaDLL.luaopen_pb);
            luaState.OpenLibs(LuaDLL.luaopen_lpeg);

            luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            luaState.OpenLibs(LuaDLL.luaopen_cjson);
            luaState.LuaSetField(-2, "cjson");

            luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
            luaState.LuaSetField(-2, "cjson.safe");


            luaState.LuaSetTop(0);

            LuaBinder.Bind(luaState);

            luaState.Start();

            luaLooper = go.AddComponent<LuaLooper>();
            luaLooper.luaState = luaState;

            Initialized = true;
        }

        public void DoFile(string scriptName)
        {
            if (luaState != null)
            {
                luaState.DoFile(scriptName);
            }
        }

        public object[] Call(string module, string name, params object[] args)
        {
            if (luaState != null)
            {
                string funcName = module + "." + name;
                var func = luaState.GetFunction(funcName);
                var result = func.LazyCall(args);
                func.Dispose();
                return result;
            }
            return null;
        }

        public LuaFunction GetFunction(string module, string func)
        {
            if (luaState != null)
            {
                string funcName = module + "." + func;
                return luaState.GetFunction(funcName);
            }
            return null;
        }
    }
}
