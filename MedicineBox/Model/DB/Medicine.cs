using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 药品表
    /// </summary>
    public class Medicine
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 药品编号
        /// </summary>
        public string MedicineNo { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        public string MedicineName { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int Deleted { get; set; }

    }
}
