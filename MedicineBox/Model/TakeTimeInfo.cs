using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 服药时间信息(更新用)
    /// </summary>
    public class TakeTimeInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 早时
        /// </summary>
        public int MorningHour { get; set; }

        /// <summary>
        /// 早分
        /// </summary>
        public int MorningMinute { get; set; }

        /// <summary>
        /// 午时
        /// </summary>
        public int NoonHour { get; set; }

        /// <summary>
        /// 午分
        /// </summary>
        public int NoonMinute { get; set; }

        /// <summary>
        /// 晚时
        /// </summary>
        public int EveningHour { get; set; }

        /// <summary>
        /// 晚分
        /// </summary>
        public int EveningMinute { get; set; }

        /// <summary>
        /// 附时
        /// </summary>
        public int AdditionalHour { get; set; }

        /// <summary>
        /// 附分
        /// </summary>
        public int AdditionalMinute { get; set; }
    }
}
