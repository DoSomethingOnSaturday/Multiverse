using System;
using LuaInterface;
using UnityEngine;
using System.Collections;

namespace Multiverse.Runtime
{
    public class LuaScript : MonoBehaviour
    {
        public string luaScript;

        public Transform[] transforms;
        public string[] strings;
        public float[] floats;
        public int[] ints;

        public bool eventOnTriggerEnter;
        public bool eventOnTriggerExit;

        public LuaTable self;
        public string module;

        public static Type GetType(string TypeName)
        {
            return ApplicationController.GetType(TypeName);
        }

        public object InvokeDelegate(Delegate func, params object[] args)
        {
            return func.Method.Invoke(func.Target, args);
        }

        protected void Awake()
        {
            Load(luaScript);
        }

        public void Load(string luaScript)
        {
            if (string.IsNullOrEmpty(luaScript) || self != null)
                return;

            this.luaScript = luaScript;

            var kv = luaScript.Split('.');
            module = kv[kv.Length - 1];

            ApplicationController.Instance().LuaDoFile(luaScript);
            var result = ApplicationController.Instance().LuaCall(module, "New");

            if (result == null)
            {
                Debug.Log(module);
            }
            if (result != null && result.Length > 0)
            {
                self = (LuaTable)result[0];
            }

            if (self != null)
            {
                var func = ApplicationController.Instance().LuaGetFunction(module, "Awake");
                if (func != null)
                {
                    func.BeginPCall();
                    func.Push(self);
                    func.Push(this);
                    func.PCall();
                    func.EndPCall();
                    func.Dispose();
                }
            }
        }

        public void LuaCall(string func, params object[] args)
        {
            if (self != null)
            {
                var Params = new ArrayList { self };
                Params.AddRange(args);
                ApplicationController.Instance().LuaCall(module, func, Params.ToArray());
            }
        }

        private void Start()
        {
            if (self != null)
            {
                var func = ApplicationController.Instance().LuaGetFunction(module, "Start");
                if (func != null)
                {
                    func.BeginPCall();
                    func.Push(self);
                    func.PCall();
                    func.EndPCall();
                    func.Dispose();
                }
            }
        }

        public void OnDestroy()
        {
            if (self != null)
            {
                var func = ApplicationController.Instance().LuaGetFunction(module, "OnDestroy");
                if (func != null)
                {
                    func.BeginPCall();
                    func.Push(self);
                    func.PCall();
                    func.EndPCall();
                    func.Dispose();
                }

                //ApplicationController.Instance().LuaReleaseTable(self);
            }
        }

        public void OnDisable()
        {
            if (self != null)
            {
                var func = ApplicationController.Instance().LuaGetFunction(module, "OnDisable");
                if (func != null)
                {
                    func.BeginPCall();
                    func.Push(self);
                    func.PCall();
                    func.EndPCall();
                    func.Dispose();
                }
            }
        }

        public void OnEnable()
        {
            if (self != null)
            {
                var func = ApplicationController.Instance().LuaGetFunction(module, "OnEnable");
                if (func != null)
                {
                    func.BeginPCall();
                    func.Push(self);
                    func.PCall();
                    func.EndPCall();
                    func.Dispose();
                }
            }
        }


        public void OnTriggerEnter(Collider other)
        {
            if (eventOnTriggerEnter && self != null)
            {
                ApplicationController.Instance().LuaCall(module, "OnTriggerEnter", self, other);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (eventOnTriggerExit && self != null)
            {
                ApplicationController.Instance().LuaCall(module, "OnTriggerExit", self, other);
            }
        }
    }
}