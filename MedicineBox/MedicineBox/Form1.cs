using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logic;
using Model;
using Model.Enum;
using Qunau.NetFrameWork.Common.Extension;
using Qunau.NetFrameWork.Common.Write;

namespace MedicineBox
{
    public partial class Form1 : Form
    {
        private Socket sokListen;
        private List<MsgReTransmission> msgList = new List<MsgReTransmission>();
        private System.Timers.Timer everySec;

        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IniSocket();
            // 接收、处理数据线程
            Thread thHandleData = new Thread(HandleRecvDate) { IsBackground = true };
            thHandleData.Start();

            IniTimer();
            everySec.Enabled = true;

            ShowAllPatient();
        }

        /// <summary>
        /// 初始化udp套接字
        /// </summary>
        private void IniSocket()
        {
            sokListen = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, Config.Port);
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            sokListen.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            //当下位机掉线时的异常处理，即发送目标不在线时保证服务器不崩溃
            sokListen.Bind(ip);
        }

        /// <summary>
        /// 处理接收的数据
        /// </summary>
        /// <param name="obj"></param>
        private void HandleRecvDate(object obj)
        {
            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remote = (EndPoint)client;

            byte[] data = new byte[128];
            while (true)
            {
                try
                {
                    var recv = sokListen.ReceiveFrom(data, ref remote);
                    LogService.WriteLog($"收到数据：{Common.ByteToHexStr(data, recv)}");
                    if (Common.IsDataComplete(data, recv))
                    {
                        string strData = Common.GetData(Common.ByteToHexStr(data, recv));
                        RecvMsg recvMsg = new RecvMsg
                        {
                            DeviceNo = Convert.ToInt32(strData.Substring(0, 4), 16),
                            FuncNo = strData.Substring(4, 2),
                            Data = strData.Substring(6, strData.Length - 12),
                            Flg = Common.HexStringToByte(strData.Substring(strData.Length - 6, 4)),
                        };
                        switch (recvMsg.FuncNo)
                        {
                            case "04": //开机
                                SendTime(recvMsg.DeviceNo, remote);
                                if (MedicineLogic.IsDeviceExistbyDeviceNo(recvMsg.DeviceNo))
                                {
                                    Device device = MedicineLogic.QueryDeviceByDeviceNo(recvMsg.DeviceNo);
                                    MedicineLogic.UpdateDeviceIp(device.Id, remote.ToString());
                                }
                                else
                                {
                                    MedicineLogic.AddDevice(new Device
                                    {
                                        DeviceNo = recvMsg.DeviceNo,
                                        DeviceIP = remote.ToString()
                                    });
                                }
                                break;
                            case "06": //服药情况
                                {
                                    UpdateTakeTimeStatus(recvMsg);
                                    int id = MedicineLogic.QueryPatientIdByDeviceNo(recvMsg.DeviceNo);
                                    UpdateUi(id);
                                }
                                break;
                            case "07": //开药仓反馈
                                {
                                    int id = MedicineLogic.QueryPatientIdByDeviceNo(recvMsg.DeviceNo);
                                    int dispenId = Convert.ToInt32(Common.ByteToHexStr(recvMsg.Flg, 2), 16);
                                    MedicineLogic.UpdateIsTake(dispenId);
                                    //SetIsTakeOnUi(dispenId);
                                    UpdateUi(id);
                                    SendDispensing(id);
                                }
                                break;
                            default: //命令响应
                                lock (msgList)
                                {
                                    int index = msgList.FindIndex(n => ByteEquals(n.Flg, recvMsg.Flg));
                                    if (index >= 0)
                                    {
                                        msgList.RemoveAt(index);
                                    }

                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.WriteLog(ex, "数据处理异常");
                }
            }
        }

        /// <summary>
        /// 判断两个byte[]是否相等
        /// </summary>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        /// <returns></returns>
        private bool ByteEquals(byte[] byte1, byte[] byte2)
        {
            if (byte1.Length != byte2.Length)
            {
                return false;
            }
            return !byte1.Where((t, i) => t != byte2[i]).Any();
        }

        /// <summary>
        /// 发送时间
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="remote"></param>
        private void SendTime(int deviceNo, EndPoint remote)
        {
            byte[] zero = { 0x00 };
            byte[] bytedeviceNo = deviceNo <= 255 ? zero.Concat(Common.IntToHexByte(deviceNo)).ToArray() : Common.IntToHexByte(deviceNo);
            byte[] funcNo = { 0x03 };
            byte[] time = Common.IntToHexByte(DateTime.Now.Hour).Concat(Common.IntToHexByte(DateTime.Now.Minute)).Concat(Common.IntToHexByte(DateTime.Now.Second)).ToArray();
            byte[] flg = Common.BuildFlg();
            byte[] data = bytedeviceNo.Concat(funcNo).Concat(time).Concat(flg).ToArray();
            AutoSendData(remote, data, flg);
        }

        /// <summary>
        /// 初步处理更新服药时间
        /// </summary>
        /// <param name="recvMsg"></param>
        private void UpdateTakeTimeStatus(RecvMsg recvMsg)
        {
            string time = recvMsg.Data.Substring(0, 2);
            string status = recvMsg.Data.Substring(2);
            int id = MedicineLogic.QueryPatientIdByDeviceNo(recvMsg.DeviceNo);
            switch (time)
            {
                case "31": // 早
                    UpdateTakeTimeStatus(id, "MorningStatus", status);
                    break;
                case "32": //中
                    UpdateTakeTimeStatus(id, "NoonStatus", status);
                    break;
                case "33": //晚
                    UpdateTakeTimeStatus(id, "EveningStatus", status);
                    break;
                case "34": //附加
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 进一步处理更新服药时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prop"></param>
        /// <param name="status"></param>
        private void UpdateTakeTimeStatus(int id, string prop, string status)
        {
            switch (status)
            {
                case "01":
                    MedicineLogic.UpdateTakeTimeStatus(prop, TakeStatus.正常, id);
                    break;
                case "02":
                    MedicineLogic.UpdateTakeTimeStatus(prop, TakeStatus.异常, id);
                    break;
                case "03":
                    MedicineLogic.UpdateTakeTimeStatus(prop, TakeStatus.未吃药, id);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void IniTimer()
        {
            everySec = new System.Timers.Timer();
            everySec.Interval = 1000;
            everySec.Elapsed += timer_EverySec_Elapsed;

        }

        /// <summary>
        /// 定时器，每秒触发的任务=发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_EverySec_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            everySec.Enabled = false;
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59)
            {
                MedicineLogic.ResetIsTake();
            }
            if (msgList.Count > 0)
            {
                lock (msgList)
                {
                    for (int i = 0; i < msgList.Count; i++)
                    {
                        if (msgList[i].ReTransmissionCount <= 0)
                        {
                            LogService.WriteLog($"超过重传次数，消息为：{Common.ByteToHexStr(msgList[i].SendMsg, msgList[i].SendMsg.Length)}");
                            msgList.RemoveAt(i);
                            continue;
                        }
                        SendData(msgList[i].SendMsg, msgList[i].Remote);
                        msgList[i].ReTransmissionCount--;
                    }
                }
            }
            everySec.Enabled = true;
        }

        /// <summary>
        /// 发送数据到指定的目标
        /// </summary>
        /// <param name="bytBuff">数据</param>
        /// <param name="remote">远端地址</param>
        /// <returns>成功=true</returns>
        private bool SendData(byte[] bytBuff, EndPoint remote)
        {
            try
            {
                byte[] bytHead = { 0xEE };
                byte[] bytEnd = { 0xFC };
                bytBuff = bytHead.Concat(bytBuff).Concat(bytEnd).ToArray();
                if (sokListen.SendTo(bytBuff, bytBuff.Length, SocketFlags.None, remote) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "发送消息异常");
                return false;
            }

        }

        /// <summary>
        /// 自动发送数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="data"></param>
        /// <param name="flg"></param>
        private void AutoSendData(string ip, byte[] data, byte[] flg)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return;
            }
            IPAddress IPadr = IPAddress.Parse(ip.Split(':')[0]);         //string to ipaddress
            IPEndPoint endPoint = new IPEndPoint(IPadr, int.Parse(ip.Split(':')[1]));
            //SendData(data, endPoint);
            MsgReTransmission mr = new MsgReTransmission(endPoint, data, flg);
            lock (msgList)
            {
                msgList.Add(mr);
            }
        }

        /// <summary>
        /// 自动发送数据
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="data"></param>
        /// <param name="flg"></param>
        private void AutoSendData(EndPoint remote, byte[] data, byte[] flg)
        {
            //SendData(data, remote);
            MsgReTransmission mr = new MsgReTransmission(remote, data, flg);
            lock (msgList)
            {
                msgList.Add(mr);
            }
        }

        /// <summary>
        /// 显示所有病人信息
        /// </summary>
        private void ShowAllPatient()
        {
            List<Patient> patients = MedicineLogic.QueryAllPatients();
            foreach (var patient in patients)
            {
                int index = this.dgvPatient.Rows.Add();
                this.dgvPatient.Rows[index].Cells[0].Value = patient.Id;
                this.dgvPatient.Rows[index].Cells[1].Value = patient.Name;
                DataGridViewImageCell img = new DataGridViewImageCell();
                Bitmap bitmap = Properties.Resources.greenlight;
                if ((patient.MorningStatus == TakeStatus.正常 || patient.MorningStatus == TakeStatus.未知) &&
                    (patient.NoonStatus == TakeStatus.正常 || patient.NoonStatus == TakeStatus.未知) &&
                    (patient.EveningStatus == TakeStatus.正常 || patient.EveningStatus == TakeStatus.未知) &&
                    (patient.AdditionalStatus == TakeStatus.正常 || patient.AdditionalStatus == TakeStatus.未知))
                {
                    this.dgvPatient.Rows[index].Cells[2].Value = Properties.Resources.greenlight;
                }
                else
                {
                    this.dgvPatient.Rows[index].Cells[2].Value = Properties.Resources.redlight;
                }
            }
            this.dgvPatient.RowHeadersVisible = false;
            this.dgvPatient.Columns[0].Visible = false;
        }

        /// <summary>
        /// 单元格点击事件，查询病人信息并显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.RowIndex <= -1)
            {
                return;
            }
            int id = Convert.ToInt32(dgvPatient.Rows[e.RowIndex].Cells[0].Value);
            Patient patient = MedicineLogic.QueryPatientById(id);
            Device device = MedicineLogic.QueryDeviceById(id);
            labId.Text = id.ToString();
            labName.Text = patient.Name;
            labSex.Text = patient.Sex.ToString();
            labAge.Text = patient.Age.ToString();
            labHeight.Text = patient.Height.ToString("0.##");
            labWeight.Text = patient.Weight.ToString("0.##");
            labIllness.Text = patient.Illness;
            labWardNo.Text = patient.WardNo;
            labBedNo.Text = patient.BedNo;

            labDeviceNo.Text = device.DeviceNo.ToString();

            txtMorningHour.Text = patient.MorningHour == -1 ? string.Empty : patient.MorningHour.ToString();
            txtMorningMinute.Text = patient.MorningMinute == -1 ? string.Empty : patient.MorningMinute.ToString();
            txtNoonHour.Text = patient.NoonHour == -1 ? string.Empty : patient.NoonHour.ToString();
            txtnoonMinute.Text = patient.NoonMinute == -1 ? string.Empty : patient.NoonMinute.ToString();
            txtEveningHour.Text = patient.EveningHour == -1 ? string.Empty : patient.EveningHour.ToString();
            txtEveningMinute.Text = patient.EveningMinute == -1 ? string.Empty : patient.EveningMinute.ToString();
            txtAddHour.Text = patient.AdditionalHour == -1 ? string.Empty : patient.AdditionalHour.ToString();
            txtAddMinute.Text = patient.AdditionalMinute == -1 ? string.Empty : patient.AdditionalMinute.ToString();

            picMorningStatus.Image = GetLight(patient.MorningStatus);
            picNoonStatus.Image = GetLight(patient.NoonStatus);
            picEveningStatus.Image = GetLight(patient.EveningStatus);
            picAddStatus.Image = GetLight(patient.AdditionalStatus);

            ShowDispensing(id);
        }

        /// <summary>
        /// 获取监控图标
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Bitmap GetLight(TakeStatus status)
        {
            switch (status)
            {
                case TakeStatus.正常:
                    return Properties.Resources.greenlight;
                case TakeStatus.未知:
                    return null;
                case TakeStatus.异常:
                case TakeStatus.未吃药:
                    return Properties.Resources.redlight;
                default:
                    return Properties.Resources.redlight;
            }
        }

        /// <summary>
        /// 保存服药时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTakeTime_Click(object sender, EventArgs e)
        {
            if (labId.Text == "-1")
            {
                MessageBox.Show("请先选择病人");
                return;
            }
            TakeTimeInfo model;
            try
            {
                model = new TakeTimeInfo
                {
                    Id = Convert.ToInt32(labId.Text ?? "-1"),
                    MorningHour = Convert.ToInt32(txtMorningHour.Text ?? "-1"),
                    MorningMinute = Convert.ToInt32(txtMorningMinute.Text ?? "-1"),
                    NoonHour = Convert.ToInt32(txtNoonHour.Text ?? "-1"),
                    NoonMinute = Convert.ToInt32(txtnoonMinute.Text ?? "-1"),
                    EveningHour = Convert.ToInt32(txtEveningHour.Text ?? "-1"),
                    EveningMinute = Convert.ToInt32(txtEveningMinute.Text ?? "-1"),
                    AdditionalHour = Convert.ToInt32(txtAddHour.Text ?? "-1"),
                    AdditionalMinute = Convert.ToInt32(txtAddMinute.Text ?? "-1"),
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入数字");
                return;
            }
            if (!Common.IsHour(model.MorningHour) ||
                !Common.IsHour(model.NoonHour) ||
                !Common.IsHour(model.EveningHour) ||
                !Common.IsHour(model.AdditionalHour))
            {
                MessageBox.Show("小时必须在1到24之间");
                return;
            }
            if (!Common.IsMinute(model.MorningMinute) ||
                 !Common.IsMinute(model.NoonMinute) ||
                 !Common.IsMinute(model.EveningMinute) ||
                 !Common.IsMinute(model.AdditionalMinute))
            {
                MessageBox.Show("分钟必须在0到59之间");
                return;
            }
            MedicineLogic.UpdateTakeTime(model);
            SendTakeTime(model);
        }

        /// <summary>
        /// 发送服药时间
        /// </summary>
        /// <param name="info">服药时间信息</param>
        private void SendTakeTime(TakeTimeInfo info)
        {
            Device device = MedicineLogic.QueryDeviceById(info.Id);
            byte[] zero = { 0x00 };
            byte[] nodata = { 0xff };
            byte[] deviceNo = device.DeviceNo <= 255 ? zero.Concat(Common.IntToHexByte(device.DeviceNo)).ToArray() : Common.IntToHexByte(device.DeviceNo);
            byte[] funcNo = { 0x05 };
            byte[] morningHour = info.MorningHour == -1 ? nodata : Common.IntToHexByte(info.MorningHour);
            byte[] morningMinute = info.MorningMinute == -1 ? nodata : Common.IntToHexByte(info.MorningMinute);

            byte[] noonHour = info.NoonHour == -1 ? nodata : Common.IntToHexByte(info.NoonHour);
            byte[] noonMinute = info.NoonMinute == -1 ? nodata : Common.IntToHexByte(info.NoonMinute);

            byte[] eveningHour = info.EveningHour == -1 ? nodata : Common.IntToHexByte(info.EveningHour);
            byte[] eveningMinute = info.EveningMinute == -1 ? nodata : Common.IntToHexByte(info.EveningMinute);

            byte[] addHour = info.AdditionalHour == -1 ? nodata : Common.IntToHexByte(info.AdditionalHour);
            byte[] addMinute = info.AdditionalMinute == -1 ? nodata : Common.IntToHexByte(info.AdditionalMinute);

            byte[] byteflg = Common.BuildFlg();

            byte[] data =
                deviceNo.Concat(funcNo)
                    .Concat(morningHour)
                    .Concat(morningMinute)
                    .Concat(noonHour)
                    .Concat(noonMinute)
                    .Concat(eveningHour)
                    .Concat(eveningMinute)
                    .Concat(addHour)
                    .Concat(addMinute)
                    .Concat(byteflg)
                    .ToArray();
            AutoSendData(device.DeviceIP, data, byteflg);
        }

        /// <summary>
        /// 显示配药信息
        /// </summary>
        /// <param name="id"></param>
        private void ShowDispensing(int id)
        {
            dgvDispensing.Rows.Clear();
            List<Dispensing> dispensingList = MedicineLogic.QueryDispensingByPatientId(id);
            foreach (var dispensing in dispensingList)
            {
                Medicine medicine = MedicineLogic.QueryMedicineById(dispensing.MedicineId);
                int index = dgvDispensing.Rows.Add();
                dgvDispensing.Rows[index].Cells[0].Value = dispensing.Id;
                dgvDispensing.Rows[index].Cells[1].Value = medicine.MedicineName;
                dgvDispensing.Rows[index].Cells[2].Value = dispensing.MedicineNumber.ToString("0.##");

                dgvDispensing.Rows[index].Cells[3].Value = ((dispensing.TakeTime & TakeTime.早上) != 0) ? "√" : "×";
                dgvDispensing.Rows[index].Cells[4].Value = ((dispensing.TakeTime & TakeTime.中午) != 0) ? "√" : "×";
                dgvDispensing.Rows[index].Cells[5].Value = ((dispensing.TakeTime & TakeTime.晚上) != 0) ? "√" : "×";
                dgvDispensing.Rows[index].Cells[6].Value = ((dispensing.TakeTime & TakeTime.附加) != 0) ? "√" : "×";

                dgvDispensing.Rows[index].Cells[7].Value = dispensing.IsTake == 1
                    ? Properties.Resources.greenlight
                    : Properties.Resources.redlight;
            }
            dgvDispensing.RowHeadersVisible = false;
        }

        /// <summary>
        /// 开始配药
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDispensing_Click(object sender, EventArgs e)
        {
            if (labId.Text == "-1")
            {
                MessageBox.Show("请先选择病人");
                return;
            }
            int id = Convert.ToInt32(labId.Text);
            SendDispensing(id);
        }

        /// <summary>
        /// 发送配药指令
        /// </summary>
        /// <param name="id"></param>
        private void SendDispensing(int id)
        {
            Patient patient = MedicineLogic.QueryPatientById(id);
            Device device = MedicineLogic.QueryDeviceById(patient.DeviceId);
            Dispensing dispensing = MedicineLogic.QueryFirstDispensingByPatientId(id);
            if (dispensing == null || dispensing.Id <= 0)
            {
                return;
            }
            byte[] zero = { 0x00 };
            byte[] deviceNo = device.DeviceNo <= 255 ? zero.Concat(Common.IntToHexByte(device.DeviceNo)).ToArray() : Common.IntToHexByte(device.DeviceNo);
            byte[] funcNo = { 0x02 };
            byte[] data = { };
            byte[] open = { 0x31 };
            byte[] close = { 0x30 };
            byte[] byteflg = dispensing.Id > 255 ? Common.IntToHexByte(dispensing.Id) : zero.Concat(Common.IntToHexByte(dispensing.Id)).ToArray();
            data = Enum.GetValues(typeof(TakeTime)).Cast<object>().Aggregate(data, (current, i) => current.Concat((Convert.ToInt32(dispensing.TakeTime) & i.GetHashCode()) != 0 ? open : close).ToArray());
            data = deviceNo.Concat(funcNo).Concat(data).Concat(byteflg).ToArray();
            AutoSendData(device.DeviceIP, data, byteflg);
        }

        /// <summary>
        /// 修改设备编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveNewDeviceNo_Click(object sender, EventArgs e)
        {
            if (labId.Text == "-1")
            {
                MessageBox.Show("请先选择病人");
                return;
            }
            int currDeviceNo = Convert.ToInt32(labDeviceNo.Text);
            int deviceNo = Convert.ToInt32(txtNewDeviceNo.Text ?? "-1");
            if (deviceNo <= 0 || deviceNo >= 65535)
            {
                MessageBox.Show("设备号在1到65534之间");
                return;
            }
            if (MedicineLogic.IsDeviceExistbyDeviceNo(deviceNo))
            {
                MessageBox.Show("该设备已存在");
                return;
            }
            Device device = MedicineLogic.QueryDeviceByDeviceNo(currDeviceNo);
            MedicineLogic.UpdateDeviceNo(device.Id, deviceNo);
            byte[] zero = { 0x00 };
            byte[] funcNo = { 0x01 };
            byte[] oldDeviceNo = currDeviceNo > 255 ? Common.IntToHexByte(currDeviceNo) : zero.Concat(Common.IntToHexByte(currDeviceNo)).ToArray();
            byte[] newDeviceNo = deviceNo > 255 ? Common.IntToHexByte(deviceNo) : zero.Concat(Common.IntToHexByte(deviceNo)).ToArray();
            byte[] flg = Common.BuildFlg();
            byte[] data = oldDeviceNo.Concat(funcNo).Concat(newDeviceNo).Concat(flg).ToArray();
            AutoSendData(device.DeviceIP, data, flg);
            labDeviceNo.Text = deviceNo.ToString();
        }

        /// <summary>
        /// 在界面上更新是否配药完成
        /// </summary>
        /// <param name="dispensId">配药主键</param>
        private void SetIsTakeOnUi(int dispensId)
        {
            foreach (DataGridViewRow row in dgvDispensing.Rows)
            {
                if (row.Cells[0].Value.ToString() == dispensId.ToString())
                {
                    row.Cells[7].Value = Properties.Resources.greenlight;
                    break;
                }
            }
        }

        /// <summary>
        /// 重新查询数据
        /// </summary>
        /// <param name="patientid"></param>
        private void UpdateUi(int patientid)
        {
            if (dgvPatient.CurrentRow != null)
            {
                int currIndex = dgvPatient.CurrentRow.Index;
                foreach (DataGridViewRow row in dgvPatient.Rows)
                {
                    if (row.Cells[0].Value.ToString() == patientid.ToString())
                    {
                        if (row.Index == currIndex)
                        {
                            dgvPatient_CellClick(dgvPatient, new DataGridViewCellEventArgs(0, row.Index));
                            break;
                        }
                        Patient patient = MedicineLogic.QueryPatientById(patientid);
                        if ((patient.MorningStatus == TakeStatus.正常 || patient.MorningStatus == TakeStatus.未知) &&
                            (patient.NoonStatus == TakeStatus.正常 || patient.NoonStatus == TakeStatus.未知) &&
                            (patient.EveningStatus == TakeStatus.正常 || patient.EveningStatus == TakeStatus.未知) &&
                            (patient.AdditionalStatus == TakeStatus.正常 || patient.AdditionalStatus == TakeStatus.未知))
                        {
                            row.Cells[2].Value = Properties.Resources.greenlight;
                        }
                        else
                        {
                            row.Cells[2].Value = Properties.Resources.redlight;
                        }
                    }

                }
            }

        }
    }
}

