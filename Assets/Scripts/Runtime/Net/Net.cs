using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class Net : MonoSingleton<Net>
    {
        public void Send(Protocol protocol,Action callBack)
        {

        }

        public void Receive()
        {
            //接收
            //入队列
            //通知
        }
    }
}
