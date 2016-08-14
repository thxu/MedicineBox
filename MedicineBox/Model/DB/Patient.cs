using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 病情及禁忌
        /// </summary>
        public string Illness { get; set; }

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

        /// <summary>
        /// 早状态
        /// </summary>
        public TakeStatus MorningStatus { get; set; }

        /// <summary>
        /// 午状态
        /// </summary>
        public TakeStatus NoonStatus { get; set; }

        /// <summary>
        /// 晚状态
        /// </summary>
        public TakeStatus EveningStatus { get; set; }

        /// <summary>
        /// 附加状态
        /// </summary>
        public TakeStatus AdditionalStatus { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// 病房号
        /// </summary>
        public string WardNo { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public string BedNo { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int Deleted { get; set; }

    }
}
