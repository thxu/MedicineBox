using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 男
        /// </summary>
        男 = 1,

        /// <summary>
        /// 女
        /// </summary>
        女 = 2,

        /// <summary>
        /// 未知
        /// </summary>
        未知 = 3,
    }

    /// <summary>
    /// 吃药状态
    /// </summary>
    public enum TakeStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        正常 = 1,

        /// <summary>
        /// 异常
        /// </summary>
        异常 = 2,

        /// <summary>
        /// 未吃药
        /// </summary>
        未吃药 = 3,

        /// <summary>
        /// 未知
        /// </summary>
        未知 = 4,
    }

    /// <summary>
    /// 服药时间
    /// </summary>
    [Flags]
    public enum TakeTime
    {
        /// <summary>
        /// 早上
        /// </summary>
        早上 = 1,

        /// <summary>
        /// 中午
        /// </summary>
        中午 = 2,

        /// <summary>
        /// 晚上
        /// </summary>
        晚上 = 4,

        /// <summary>
        /// 附加
        /// </summary>
        附加 = 8,
    }
}
