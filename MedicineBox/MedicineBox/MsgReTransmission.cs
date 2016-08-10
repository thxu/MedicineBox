using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MedicineBox
{
    /// <summary>
    /// 消息重传
    /// </summary>
    public class MsgReTransmission
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count">重传次数</param>
        /// <param name="remote">远端地址</param>
        /// <param name="msg">发送消息</param>
        /// <param name="flg">标记位</param>
        public MsgReTransmission(EndPoint remote, byte[] msg, byte[] flg, int count = 3)
        {
            this.ReTransmissionCount = count;
            this.Remote = remote;
            this.SendMsg = msg;
            this.Flg = flg;
        }

        /// <summary>
        /// 重传次数，默认3次
        /// </summary>
        public int ReTransmissionCount { get; set; } = 3;

        /// <summary>
        /// 远端地址
        /// </summary>
        public EndPoint Remote { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public byte[] SendMsg { get; set; }

        /// <summary>
        /// 标记位
        /// </summary>
        public byte[] Flg { get; set; }
    }
}
