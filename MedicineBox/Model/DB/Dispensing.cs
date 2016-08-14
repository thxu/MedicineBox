using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Model
{
    /// <summary>
    /// 配药表
    /// </summary>
    public class Dispensing
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// 药品ID
        /// </summary>
        public int MedicineId { get; set; }

        /// <summary>
        /// 药品数量
        /// </summary>
        public decimal MedicineNumber { get; set; }

        /// <summary>
        /// 服用时间
        /// </summary>
        public TakeTime TakeTime { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public int IsTake { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int Deleted { get; set; }

    }
}
