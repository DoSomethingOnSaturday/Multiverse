using LuaInterface;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class Initialization : MonoSingleton<Initialization>
    {
        private FiniteStateMachine gameState = null;
        private LuaState lua;

        void Start()
        {
            InitializeLua();
            InitializeUI();
            InitializeLogic();
        }

        void Update()
        {

        }

        void InitializeLua()
        {
            lua = new LuaState();

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            lua.OpenLibs(LuaDLL.luaopen_bit);
#endif

            lua.OpenLibs(LuaDLL.luaopen_pb);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);

            LuaBinder.Bind(lua);

            lua.Start();

            LuaDriver.Instance.luaState = lua;

            LuaDriver.Instance.DoFile(Application.dataPath+"/Lua/Test");
        }

        void InitializeUI()
        {
            //TODO
            //1.检查更新，如果需要更新，则更新
            //2.账号登录和服务器选择
        }

        void InitializeLogic()
        {
            gameState = new FiniteStateMachine();

            //GameObject go = Resource.Load("cube");
        }
    }
}
