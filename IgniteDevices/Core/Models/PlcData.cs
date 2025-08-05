using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Core.Models
{
    // <summary>
    /// PLC通用数据容器（支持所有Modbus数据类型）
    /// </summary>
    public class PlcData
    {
        // 原始字节数据
        public byte[] RawBytes { get; set; }

        // 结构化数据（按需转换）
        public ushort[] HoldingRegisters { get; set; }

        public bool[] CoilStatuses { get; set; }
        public float[] FloatValues { get; set; }
        public int[] Int32Values { get; set; }

        // 元数据
        public int StartAddress { get; set; }

        public DataType DataType { get; set; } = DataType.UInt16;

        /// <summary>
        /// 将原始Modbus寄存器数据转换为强类型PLC数据结构
        /// </summary>
        /// <param name="rawRegisters">原始寄存器数组</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="dataType">目标数据类型</param>
        /// <returns>包含转换后数据的PlcData对象</returns>
        public static PlcData ConvertToPlcData(ushort[] rawRegisters, ushort startAddress, DataType dataType)
        {
            var plcData = new PlcData
            {
                StartAddress = startAddress,
                DataType = dataType,
                RawBytes = ToByteArray(rawRegisters),
                HoldingRegisters = rawRegisters // 保留原始数据
            };

            try
            {
                switch (dataType)
                {
                    case DataType.UInt16:
                        // 原始数据已是ushort数组
                        plcData.HoldingRegisters = rawRegisters;
                        break;

                    case DataType.Int32:
                        plcData.Int32Values = ConvertToInt32Array(rawRegisters);
                        break;

                    case DataType.Float:
                        plcData.FloatValues = ConvertToFloatArray(rawRegisters);
                        break;

                    case DataType.Boolean:
                        plcData.CoilStatuses = ConvertToBoolArray(rawRegisters);
                        break;

                    case DataType.RawBytes:
                        // 已在构造函数处理
                        break;

                    default:
                        throw new NotSupportedException($"不支持的数据类型: {dataType}");
                }
            }
            catch (Exception ex)
            {
                // 转换失败时保留原始数据
                //  plcData.Message = $"数据类型转换失败: {ex.Message}";
            }

            return plcData;
        }

        // --------------------------
        // 以下是转换工具方法
        // --------------------------

        /// <summary>
        /// ushort数组转字节数组（保留字节序）
        /// </summary>
        private static byte[] ToByteArray(ushort[] registers)
        {
            var byteArray = new byte[registers.Length * 2];
            Buffer.BlockCopy(registers, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        /// <summary>
        /// 将寄存器对转换为Int32数组（大端序）
        /// </summary>
        private static int[] ConvertToInt32Array(ushort[] registers)
        {
            if (registers.Length % 2 != 0)
                throw new ArgumentException("INT32转换需要偶数个寄存器");

            var result = new int[registers.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                int hi = registers[i * 2];
                int lo = registers[i * 2 + 1];
                result[i] = (hi << 16) | lo;
            }
            return result;
        }

        /// <summary>
        /// 将寄存器对转换为IEEE754浮点数数组
        /// </summary>
        private static float[] ConvertToFloatArray(ushort[] registers)
        {
            if (registers.Length % 2 != 0)
                throw new ArgumentException("FLOAT转换需要偶数个寄存器");

            var result = new float[registers.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                byte[] bytes = new byte[4];
                Buffer.BlockCopy(registers, i * 4, bytes, 0, 4);

                // Modbus通常为大端序，需根据设备调整
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                result[i] = BitConverter.ToSingle(bytes, 0);
            }
            return result;
        }

        /// <summary>
        /// 将寄存器的位转换为布尔数组（每个寄存器16个布尔值）
        /// </summary>
        private static bool[] ConvertToBoolArray(ushort[] registers)
        {
            var result = new bool[registers.Length * 16];
            for (int i = 0; i < registers.Length; i++)
            {
                ushort reg = registers[i];
                for (int bit = 0; bit < 16; bit++)
                {
                    result[i * 16 + bit] = (reg & (1 << bit)) != 0;
                }
            }
            return result;
        }
    }

    public enum DataType
    {
        UInt16,
        Int32,
        Float,
        Boolean,
        RawBytes
    }
}