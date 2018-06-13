using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class LuaScript : MonoBehaviour
    {
        public string luaScript;

        private LuaTable luaTable;
        private string module;

        private void Awake()
        {
            Load(luaScript);
        }

        public void Load(string luaScript)
        {
            if (string.IsNullOrEmpty(luaScript) || luaTable != null)
                return;

            this.luaScript = luaScript;

            var kv = luaScript.Split('.');
            module = kv[kv.Length - 1];

            ApplicationController.Instance.LuaDoFile(luaScript);
            var result = ApplicationController.Instance.LuaCall(module, "New");

            if (result == null)
            {
                Debug.Log(module);
            }

            if (result != null && result.Length > 0)
            {
                luaTable = (LuaTable)result[0];
            }

            if (luaTable != null)
            {
                var func = ApplicationController.Instance.LuaGetFunction(module, "Awake");
                if (func != null)
                {
                    func.BeginPCall();
                    func.Push(luaTable);
                    func.Push(this);
                    func.PCall();
                    func.EndPCall();
                    func.Dispose();
                }
            }
        }
    }
}
