//-----------------------------------------------------------------------
// <copyright file="RecvMsg.cs" company="720U Enterprises">
// * Copyright (C) 2015 720U科技有限公司 版权所有。
// * version : 1.0.0.0
// * author  : tangxu
// * FileName: RecvMsg.cs
// * history : created by tangxu 2016/8/9 11:16:07
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineBox
{
    public class RecvMsg
    {
        /// <summary>
        /// 设备号
        /// </summary>
        public int DeviceNo { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        public string FuncNo { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 标记位
        /// </summary>
        public byte[] Flg { get; set; }
    }
}
