using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THTS.SerialPort
{
    /// <summary>
    /// Enumeration used in the init function to specify which CRC algorithm to use.
    /// </summary>
    public enum CRCCoding { CRC16CCITT, CRC16, CRC32 };

    /// <summary>
    /// 
    /// </summary>
    public class CRCEntity
    {
        private int order = 16;
        private ulong polynom = 0x1021;

        private int direct = 1;
        private ulong crcinit = 0xFFFF;
        private ulong crcxor = 0x0;
        private int refin = 0;
        private int refout = 0;

        private ulong crcmask;
        private ulong crchighbit;
        private ulong crcinit_direct;
        private ulong crcinit_nondirect;
        private ulong[] crctab = new ulong[256];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codingType"></param>
        public CRCEntity(CRCCoding codingType)
        {
            Initialize(codingType);
        }
        /// <summary>2
        /// Calculate checking-sum of the data by Cyclic Redundancy Check[Fast-version].
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public ulong SumA(byte[] data)
        {
            //Fast lookup table algorithm without augmented zero bytes, e.g. used in pkzip.
            //Only usable with polynom orders of 8, 16, 24 or 32.
            ulong crc = crcinit_direct;
            if (refin != 0)
            {
                crc = reflect(crc, order);
            }
            if (refin == 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    crc = (crc << 8) ^ crctab[((crc >> (order - 8)) & 0xff) ^ data[i]];
                }
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    crc = (crc >> 8) ^ crctab[(crc & 0xff) ^ data[i]];
                }
            }
            if ((refout ^ refin) != 0)
            {
                crc = reflect(crc, order);
            }
            crc ^= crcxor;
            crc &= crcmask;
            return (crc);
        }
        /// <summary>
        /// Calculate checking-sum of the data by Cyclic Redundancy Check.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public ulong Sum(byte[] data)
        {
            //Normal lookup table algorithm with augmented zero bytes.
            //Only usable with polynom orders of 8, 16, 24 or 32.
            ulong crc = crcinit_nondirect;
            if (refin != 0)
            {
                crc = reflect(crc, order);
            }
            if (refin == 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    crc = ((crc << 8) | data[i]) ^ crctab[(crc >> (order - 8)) & 0xff];
                }
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    crc = (ulong)(((int)(crc >> 8) | (data[i] << (order - 8))) ^ (int)crctab[crc & 0xff]);
                }
            }
            if (refin == 0)
            {
                for (int i = 0; i < order / 8; i++)
                {
                    crc = (crc << 8) ^ crctab[(crc >> (order - 8)) & 0xff];
                }
            }
            else
            {
                for (int i = 0; i < order / 8; i++)
                {
                    crc = (crc >> 8) ^ crctab[crc & 0xff];
                }
            }

            if ((refout ^ refin) != 0)
            {
                crc = reflect(crc, order);
            }
            crc ^= crcxor;
            crc &= crcmask;

            return (crc);
        }
        /// <summary>
        /// This method is an algorithm found on the web for calculating the CRCITT checksum. It's a sealed, without using crc-table.
        /// </summary>
        public ushort SumCcitt(byte[] data)
        {
            uint uiCRCITTSum = 0xFFFF;
            uint uiByteValue;

            for (int iBufferIndex = 0; iBufferIndex < data.Length; iBufferIndex++)
            {
                uiByteValue = ((uint)data[iBufferIndex] << 8);
                for (int iBitIndex = 0; iBitIndex < 8; iBitIndex++)
                {
                    if (((uiCRCITTSum ^ uiByteValue) & 0x8000) != 0)
                    {
                        uiCRCITTSum = (uiCRCITTSum << 1) ^ 0x1021;
                    }
                    else
                    {
                        uiCRCITTSum <<= 1;
                    }
                    uiByteValue <<= 1;
                }
            }
            return (ushort)uiCRCITTSum;
        }

        #region Reverved.
        private ulong crcNormalByBit(byte[] data)
        {
            // bit by bit algorithm with augmented zero bytes.
            // does not use lookup table, suited for polynom orders between 1...32.
            int i;
            ulong j, c, bit;
            ulong crc = crcinit_nondirect;

            for (i = 0; i < data.Length; i++)
            {
                c = (ulong)data[i];
                if (refin != 0)
                {
                    c = reflect(c, 8);
                }

                for (j = 0x80; j != 0; j >>= 1)
                {
                    bit = crc & crchighbit;
                    crc <<= 1;
                    if ((c & j) != 0)
                    {
                        crc |= 1;
                    }
                    if (bit != 0)
                    {
                        crc ^= polynom;
                    }
                }
            }

            for (i = 0; (int)i < order; i++)
            {

                bit = crc & crchighbit;
                crc <<= 1;
                if (bit != 0) crc ^= polynom;
            }

            if (refout != 0)
            {
                crc = reflect(crc, order);
            }
            crc ^= crcxor;
            crc &= crcmask;

            return (crc);
        }

        private ulong crcFastByBit(byte[] data)
        {
            // fast bit by bit algorithm without augmented zero bytes.
            // does not use lookup table, suited for polynom orders between 1...32.
            int i;
            ulong j, c, bit;
            ulong crc = crcinit_direct;

            for (i = 0; i < data.Length; i++)
            {
                c = (ulong)data[i];
                if (refin != 0)
                {
                    c = reflect(c, 8);
                }

                for (j = 0x80; j > 0; j >>= 1)
                {
                    bit = crc & crchighbit;
                    crc <<= 1;
                    if ((c & j) > 0) bit ^= crchighbit;
                    if (bit > 0) crc ^= polynom;
                }
            }

            if (refout > 0)
            {
                crc = reflect(crc, order);
            }
            crc ^= crcxor;
            crc &= crcmask;

            return (crc);
        }
        #endregion

        #region Subroutines
        private ulong reflect(ulong crc, int bitnum)
        {
            // reflects the lower 'bitnum' bits of 'crc'
            ulong i, j = 1, crcout = 0;

            for (i = (ulong)1 << (bitnum - 1); i != 0; i >>= 1)
            {
                if ((crc & i) != 0)
                {
                    crcout |= j;
                }
                j <<= 1;
            }
            return (crcout);
        }
        private void generate_crc_table()
        {
            // make CRC lookup table used by table algorithms

            int i, j;
            ulong bit, crc;

            for (i = 0; i < 256; i++)
            {
                crc = (ulong)i;
                if (refin != 0)
                {
                    crc = reflect(crc, 8);
                }
                crc <<= order - 8;

                for (j = 0; j < 8; j++)
                {
                    bit = crc & crchighbit;
                    crc <<= 1;
                    if (bit != 0) crc ^= polynom;
                }

                if (refin != 0)
                {
                    crc = reflect(crc, order);
                }
                crc &= crcmask;
                crctab[i] = crc;
            }
        }
        private void Initialize(CRCCoding codingType)
        {
            switch (codingType)
            {
                case CRCCoding.CRC16CCITT:
                    order = 16; direct = 1; polynom = 0x1021; crcinit = 0xFFFF; crcxor = 0; refin = 0; refout = 0;
                    break;
                case CRCCoding.CRC16:
                    order = 16; direct = 1; polynom = 0x8005; crcinit = 0x0; crcxor = 0x0; refin = 1; refout = 1;
                    break;
                case CRCCoding.CRC32:
                    order = 32; direct = 1; polynom = 0x4c11db7; crcinit = 0xFFFFFFFF; crcxor = 0xFFFFFFFF; refin = 1; refout = 1;
                    break;
            }

            //Initialize all variables for seeding and builing based upon the given coding type
            //at first, compute constant bit masks for whole CRC and CRC high bit

            crcmask = ((((ulong)1 << (order - 1)) - 1) << 1) | 1;
            crchighbit = (ulong)1 << (order - 1);

            //generate lookup table
            generate_crc_table();

            ulong bit, crc;
            int i;
            if (direct == 0)
            {
                crcinit_nondirect = crcinit;
                crc = crcinit;
                for (i = 0; i < order; i++)
                {
                    bit = crc & crchighbit;
                    crc <<= 1;
                    if (bit != 0)
                    {
                        crc ^= polynom;
                    }
                }
                crc &= crcmask;
                crcinit_direct = crc;
            }
            else
            {
                crcinit_direct = crcinit;
                crc = crcinit;
                for (i = 0; i < order; i++)
                {
                    bit = crc & 1;
                    if (bit != 0)
                    {
                        crc ^= polynom;
                    }
                    crc >>= 1;
                    if (bit != 0)
                    {
                        crc |= crchighbit;
                    }
                }
                crcinit_nondirect = crc;
            }
        }
        #endregion
    }
}
