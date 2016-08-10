using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 设备表
    /// </summary>
    public class Device
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public int DeviceNo { get; set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public string DeviceIP { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int Deleted { get; set; }

    }
}
