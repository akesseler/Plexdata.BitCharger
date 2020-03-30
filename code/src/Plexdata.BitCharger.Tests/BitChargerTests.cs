/*
 * MIT License
 * 
 * Copyright (c) 2020 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Moq;
using NUnit.Framework;
using Plexdata.Converters.Abstraction;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Plexdata.Converters.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    [TestFixtureSource(typeof(BitCharger))]
    public class BitChargerTests
    {
        #region Prologue

        private Mock<IByteOrder> byteOrder;

        [SetUp]
        public void SetUp()
        {
            this.byteOrder = new Mock<IByteOrder>();
            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);
        }

        #endregion

        #region BitCharger

        [Test]
        public void BitCharger_StandardConstruction_PropertiesAsExpected()
        {
            IBitCharger instance = new BitCharger();

            Assert.That(instance.Capacity, Is.EqualTo(128));
            Assert.That(instance.Length, Is.Zero);
            Assert.That(instance.Bytes, Is.Zero);
            Assert.That(instance.Content, Is.Empty);
        }

        [Test]
        public void BitCharger_LengthConstruction_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => new BitCharger(-1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(0, 0, 0, "")]
        [TestCase(7, 8, 1, "00000000")]
        [TestCase(8, 8, 1, "00000000")]
        [TestCase(10, 16, 2, "0000000000000000")]
        [TestCase(15, 16, 2, "0000000000000000")]
        [TestCase(16, 16, 2, "0000000000000000")]
        [TestCase(17, 24, 3, "000000000000000000000000")]
        public void BitCharger_LengthConstruction_PropertiesAsExpected(Int32 actual, Int32 length, Int32 bytes, String content)
        {
            IBitCharger instance = new BitCharger(actual);

            Assert.That(instance.Capacity, Is.EqualTo(128));
            Assert.That(instance.Length, Is.EqualTo(length));
            Assert.That(instance.Bytes, Is.EqualTo(bytes));
            Assert.That(instance.Content, Is.EqualTo(content));
        }

        [Test]
        public void BitCharger_InternalConstructionInterfaceIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new BitCharger(null), Throws.ArgumentNullException);
        }

        #endregion

        #region SetBitAt

        [Test]
        public void SetBitAt_IndexIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.SetBitAt(-1, false), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SetBitAt_IndexIsLessThanDefaultLength_PropertiesAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(7, true);

            Assert.That(instance.Length, Is.EqualTo(8));
        }

        [Test]
        public void SetBitAt_IndexIsGreaterThanDefaultLength_PropertiesAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(135, true);

            Assert.That(instance.Capacity, Is.EqualTo(264));
            Assert.That(instance.Length, Is.EqualTo(136));
        }

        [Test]
        [TestCase(0, "00000001")]
        [TestCase(2, "00000100")]
        [TestCase(5, "00100000")]
        [TestCase(7, "10000000")]
        [TestCase(8, "0000000100000000")]
        [TestCase(9, "0000001000000000")]
        [TestCase(14, "0100000000000000")]
        [TestCase(15, "1000000000000000")]
        [TestCase(16, "000000010000000000000000")]
        public void SetBitAt_WithIndexAndEnabled_ContentAsExpected(Int32 index, String expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(index, true);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region HasBitAt

        [Test]
        public void HasBitAt_IndexIsLessThanZero_ResultIsFalse()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(instance.HasBitAt(-1), Is.False);
        }

        [Test]
        public void HasBitAt_IndexIsGreaterThanLength_ResultIsFalse()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(instance.HasBitAt(instance.Length + 1), Is.False);
        }

        [Test]
        [TestCase(5, 0, false)]
        [TestCase(5, 1, false)]
        [TestCase(5, 2, false)]
        [TestCase(5, 3, false)]
        [TestCase(5, 4, false)]
        [TestCase(5, 5, true)]
        [TestCase(5, 6, false)]
        [TestCase(5, 7, false)]
        public void HasBitAt_IndexIsInRange_ResultAsExpected(Int32 index, Int32 actual, Boolean expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(index, true);

            Assert.That(instance.HasBitAt(actual), Is.EqualTo(expected));
        }

        #endregion

        #region SetBytes

        [Test]
        public void SetBytes_SourceIsNullOffsetUnused_ThrowsArgumentNullException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.SetBytes(null), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_SourceIsEmptyOffsetUnused_ContentRemainsUnchanged()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = instance.Content;

            instance.SetBytes(new Byte[0]);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_SourceIsNullOffsetZero_ThrowsArgumentNullException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.SetBytes(null, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_SourceIsEmptyOffsetInvalid_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.SetBytes(new Byte[0], -1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SetBytes_SourceIsEmptyOffsetZero_ContentRemainsUnchanged()
        {
            IBitCharger instance = this.CreateInstance();

            // 10101100 (0xAC, 172)
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = instance.Content;

            instance.SetBytes(new Byte[0], 0);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #region Little Endian

        [Test]
        public void SetBytes_SourceThreeBytesOffsetZeroLE_ContentAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            // 10101100 (0xAC, 172) -> Bits to overwrite...
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = this.MakeExpected(new Byte[] { 0x81, 0x82, 0x83 });

            Byte[] source = new Byte[] { 0x81, 0x82, 0x83 };

            instance.SetBytes(source, 0);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_SourceThreeBytesOffsetOneLE_ContentAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            // 10101100 (0xAC, 172) -> Bits to keep...
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = this.MakeExpected(new Byte[] { 0x81, 0x82, 0x83, 0xAC });

            Byte[] source = new Byte[] { 0x81, 0x82, 0x83 };

            instance.SetBytes(source, 1);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_SourceThreeBytesOffsetThreeLE_ContentAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            // 10101100 (0xAC, 172) -> Bits to keep...
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = this.MakeExpected(new Byte[] { 0x81, 0x82, 0x83, 0x00, 0x00, 0xAC });

            Byte[] source = new Byte[] { 0x81, 0x82, 0x83 };

            instance.SetBytes(source, 3);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region Big Endian

        [Test]
        public void SetBytes_SourceThreeBytesOffsetZeroBE_ContentAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(false);

            // 10101100 (0xAC, 172) -> Bits to overwrite...
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = this.MakeExpected(new Byte[] { 0x81, 0x82, 0x83 });

            Byte[] source = new Byte[] { 0x83, 0x82, 0x81 };

            instance.SetBytes(source, 0);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_SourceThreeBytesOffsetOneBE_ContentAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(false);

            // 10101100 (0xAC, 172) -> Bits to keep...
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = this.MakeExpected(new Byte[] { 0x81, 0x82, 0x83, 0xAC });

            Byte[] source = new Byte[] { 0x83, 0x82, 0x81 };

            instance.SetBytes(source, 1);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_SourceThreeBytesOffsetThreeBE_ContentAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(false);

            // 10101100 (0xAC, 172) -> Bits to keep...
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            String expected = this.MakeExpected(new Byte[] { 0x81, 0x82, 0x83, 0x00, 0x00, 0xAC });

            Byte[] source = new Byte[] { 0x83, 0x82, 0x81 };

            instance.SetBytes(source, 3);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region GetBytes

        [Test]
        public void GetBytes_OffsetUnusedLengthUnused_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            // 10101100 (0xAC, 172)
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            Byte[] actual = instance.GetBytes();

            Assert.That(actual.Length, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(172));
        }

        [Test]
        public void GetBytes_OffsetBelowMinLengthUnused_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.GetBytes(-1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetBytes_OffsetZeroLengthUnused_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            // 10101100 (0xAC, 172)
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            Byte[] actual = instance.GetBytes(0);

            Assert.That(actual.Length, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(172));
        }

        [Test]
        public void GetBytes_OffsetBelowMinLengthZero_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.GetBytes(-1, 0), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetBytes_OffsetBeyondMaxLengthZero_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(15, false);

            Assert.That(() => instance.GetBytes(instance.Bytes + 1, 0), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetBytes_OffsetZeroLengthBelowMin_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.GetBytes(0, -1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetBytes_OffsetZeroLengthBeyondMax_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(15, false);

            Assert.That(() => instance.GetBytes(0, instance.Bytes + 1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetBytes_OffsetPlusLengthBeyondMax_ThrowsArgumentOutOfRangeException()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(15, false);

            Assert.That(() => instance.GetBytes(1, 2), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => instance.GetBytes(2, 1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetBytes_OffsetZeroLengthZero_ResultIsEmpty()
        {
            IBitCharger instance = this.CreateInstance();

            instance.SetBitAt(15, false);

            Assert.That(instance.GetBytes(0, 0), Is.Empty);
        }

        [Test]
        public void GetBytes_OffsetZeroLengthOne_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            // 10101100 (0xAC, 172)
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(5, true);
            instance.SetBitAt(7, true);

            // 11110000 (0xF0, 240)
            instance.SetBitAt(12, true);
            instance.SetBitAt(13, true);
            instance.SetBitAt(14, true);
            instance.SetBitAt(15, true);

            Byte[] actual = instance.GetBytes(0, 2);

            Assert.That(actual.Length, Is.EqualTo(2));
            Assert.That(actual[0], Is.EqualTo(240));
            Assert.That(actual[1], Is.EqualTo(172));
        }

        #region Little Endian

        [Test]
        public void GetBytes_SourceInt32OffsetZeroLE_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            Int32 expected = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            // LE: 0x02 => 00000010
            instance.SetBitAt(0, false);
            instance.SetBitAt(1, true);
            instance.SetBitAt(2, false);
            instance.SetBitAt(3, false);
            instance.SetBitAt(4, false);
            instance.SetBitAt(5, false);
            instance.SetBitAt(6, false);
            instance.SetBitAt(7, false);

            // LE: 0xCE => 11001110
            instance.SetBitAt(8, false);
            instance.SetBitAt(9, true);
            instance.SetBitAt(10, true);
            instance.SetBitAt(11, true);
            instance.SetBitAt(12, false);
            instance.SetBitAt(13, false);
            instance.SetBitAt(14, true);
            instance.SetBitAt(15, true);

            // LE: 0xDA => 11011010
            instance.SetBitAt(16, false);
            instance.SetBitAt(17, true);
            instance.SetBitAt(18, false);
            instance.SetBitAt(19, true);
            instance.SetBitAt(20, true);
            instance.SetBitAt(21, false);
            instance.SetBitAt(22, true);
            instance.SetBitAt(23, true);

            // LE: 0x9F => 10011111 
            instance.SetBitAt(24, true);
            instance.SetBitAt(25, true);
            instance.SetBitAt(26, true);
            instance.SetBitAt(27, true);
            instance.SetBitAt(28, true);
            instance.SetBitAt(29, false);
            instance.SetBitAt(30, false);
            instance.SetBitAt(31, true);

            Byte[] result = instance.GetBytes(0, sizeof(Int32));

            Int32 actual = BitConverter.ToInt32(result, 0);

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Big Endian

        [Test]
        public void GetBytes_SourceInt32OffsetZeroBE_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(false);

            Int32 expected = 47110815; // BE: 0x02CEDA9F => 00000010 11001110 11011010 10011111

            // BE: 0x9F => 10011111
            instance.SetBitAt(0, true);
            instance.SetBitAt(1, true);
            instance.SetBitAt(2, true);
            instance.SetBitAt(3, true);
            instance.SetBitAt(4, true);
            instance.SetBitAt(5, false);
            instance.SetBitAt(6, false);
            instance.SetBitAt(7, true);

            // BE: 0xDA => 11011010
            instance.SetBitAt(8, false);
            instance.SetBitAt(9, true);
            instance.SetBitAt(10, false);
            instance.SetBitAt(11, true);
            instance.SetBitAt(12, true);
            instance.SetBitAt(13, false);
            instance.SetBitAt(14, true);
            instance.SetBitAt(15, true);

            // BE: 0xCE => 11001110
            instance.SetBitAt(16, false);
            instance.SetBitAt(17, true);
            instance.SetBitAt(18, true);
            instance.SetBitAt(19, true);
            instance.SetBitAt(20, false);
            instance.SetBitAt(21, false);
            instance.SetBitAt(22, true);
            instance.SetBitAt(23, true);

            // BE: 0x02 => 00000010 
            instance.SetBitAt(24, false);
            instance.SetBitAt(25, true);
            instance.SetBitAt(26, false);
            instance.SetBitAt(27, false);
            instance.SetBitAt(28, false);
            instance.SetBitAt(29, false);
            instance.SetBitAt(30, false);
            instance.SetBitAt(31, false);

            Byte[] result = instance.GetBytes(0, sizeof(Int32));

            Int32 actual = BitConverter.ToInt32(result, 0);

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Charge

        [Test]
        public void Charge_SourceIsInvalid_ThrowsArgumentException([Values(null, "", "  ")] String source, [Values(null, true, false)] Boolean? reverse)
        {
            IBitCharger instance = this.CreateInstance();

            if (reverse.HasValue)
            {
                Assert.That(() => instance.Charge(source, reverse.Value), Throws.ArgumentException);
            }
            else
            {
                Assert.That(() => instance.Charge(source), Throws.ArgumentException);
            }
        }

        [Test]
        [TestCase("_")]
        [TestCase("-")]
        [TestCase("_-")]
        [TestCase("_ -")]
        [TestCase("0b")]
        [TestCase("0b_")]
        [TestCase("0b _")]
        [TestCase("0b-")]
        [TestCase("0b -")]
        [TestCase("0b_-")]
        [TestCase("0b_ -")]
        [TestCase("0b _ -")]
        public void Charge_SourceContainsControlCharactersOnly_ThrowsArgumentException(String source)
        {
            IBitCharger instance = this.CreateInstance();

            Assert.That(() => instance.Charge(source), Throws.ArgumentException);
        }

        [Test]
        [TestCase(" 10101010 ", "10101010")]
        [TestCase(" 0b10101010 ", "10101010")]
        [TestCase(" 1010 1010 ", "10101010")]
        [TestCase(" 0b1010 1010 ", "10101010")]
        [TestCase(" 1010_1010 ", "10101010")]
        [TestCase(" 0b1010_1010 ", "10101010")]
        [TestCase(" 1010-1010 ", "10101010")]
        [TestCase(" 0b1010-1010 ", "10101010")]
        [TestCase(" 1010101011001100 ", "1010101011001100")]
        [TestCase(" 0b1010101011001100 ", "1010101011001100")]
        [TestCase(" 1010 1010 1100 1100 ", "1010101011001100")]
        [TestCase(" 0b1010 1010 1100 1100 ", "1010101011001100")]
        [TestCase(" 1010_1010_1100_1100 ", "1010101011001100")]
        [TestCase(" 0b1010_1010_1100_1100 ", "1010101011001100")]
        [TestCase(" 1010-1010-1100-1100 ", "1010101011001100")]
        [TestCase(" 0b1010-1010-1100-1100 ", "1010101011001100")]
        [TestCase(" 101010101100110011110000 ", "101010101100110011110000")]
        [TestCase(" 0b101010101100110011110000 ", "101010101100110011110000")]
        [TestCase(" 1010 1010 1100 1100 1111 0000 ", "101010101100110011110000")]
        [TestCase(" 0b1010 1010 1100 1100 1111 0000 ", "101010101100110011110000")]
        [TestCase(" 1010_1010_1100_1100_1111_0000 ", "101010101100110011110000")]
        [TestCase(" 0b1010_1010_1100_1100_1111_0000 ", "101010101100110011110000")]
        [TestCase(" 1010-1010-1100-1100-1111-0000 ", "101010101100110011110000")]
        [TestCase(" 0b1010-1010-1100-1100-1111-0000 ", "101010101100110011110000")]
        public void Charge_SourceMatchesEightBitRestriction_ContentAsExpected(String source, String expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.Charge(source);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(" 10101010 ", "10101010")]
        [TestCase(" 0b10101010 ", "10101010")]
        [TestCase(" 1010 1010 ", "10101010")]
        [TestCase(" 0b1010 1010 ", "10101010")]
        [TestCase(" 1010_1010 ", "10101010")]
        [TestCase(" 0b1010_1010 ", "10101010")]
        [TestCase(" 1010-1010 ", "10101010")]
        [TestCase(" 0b1010-1010 ", "10101010")]
        [TestCase(" 1010101011001100 ", "1100110010101010")]
        [TestCase(" 0b1010101011001100 ", "1100110010101010")]
        [TestCase(" 1010 1010 1100 1100 ", "1100110010101010")]
        [TestCase(" 0b1010 1010 1100 1100 ", "1100110010101010")]
        [TestCase(" 1010_1010_1100_1100 ", "1100110010101010")]
        [TestCase(" 0b1010_1010_1100_1100 ", "1100110010101010")]
        [TestCase(" 1010-1010-1100-1100 ", "1100110010101010")]
        [TestCase(" 0b1010-1010-1100-1100 ", "1100110010101010")]
        [TestCase(" 101010101100110011110000 ", "111100001100110010101010")]
        [TestCase(" 0b101010101100110011110000 ", "111100001100110010101010")]
        [TestCase(" 1010 1010 1100 1100 1111 0000 ", "111100001100110010101010")]
        [TestCase(" 0b1010 1010 1100 1100 1111 0000 ", "111100001100110010101010")]
        [TestCase(" 1010_1010_1100_1100_1111_0000 ", "111100001100110010101010")]
        [TestCase(" 0b1010_1010_1100_1100_1111_0000 ", "111100001100110010101010")]
        [TestCase(" 1010-1010-1100-1100-1111-0000 ", "111100001100110010101010")]
        [TestCase(" 0b1010-1010-1100-1100-1111-0000 ", "111100001100110010101010")]
        public void Charge_SourceMatchesEightBitRestriction_ReverseContentAsExpected(String source, String expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.Charge(source, true);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(" 10a0b010 ", "10000010")]
        [TestCase(" 10c0d010 ef001100 ", "1000001000001100")]
        public void Charge_SourceMatchesEightBitRestrictionButContainsInvalidCharacters_ContentAsExpected(String source, String expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.Charge(source);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1", "00000001")]
        [TestCase("11", "00000011")]
        [TestCase("111", "00000111")]
        [TestCase("1111", "00001111")]
        [TestCase("11111", "00011111")]
        [TestCase("111111", "00111111")]
        [TestCase("1111111", "01111111")]
        [TestCase("11111111", "11111111")]
        [TestCase("101011001", "0000000101011001")]
        [TestCase("1010110011", "0000001010110011")]
        [TestCase("10101100111", "0000010101100111")]
        [TestCase("101011001111", "0000101011001111")]
        [TestCase("1010110011111", "0001010110011111")]
        [TestCase("10101100111111", "0010101100111111")]
        [TestCase("101011001111111", "0101011001111111")]
        [TestCase("1010110011111111", "1010110011111111")]
        public void Charge_SourceWithWrongLength_ContentAsExpected(String source, String expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.Charge(source);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1", "00000001")]
        [TestCase("11", "00000011")]
        [TestCase("111", "00000111")]
        [TestCase("1111", "00001111")]
        [TestCase("11111", "00011111")]
        [TestCase("111111", "00111111")]
        [TestCase("1111111", "01111111")]
        [TestCase("11111111", "11111111")]
        [TestCase("101011001", "0000000110101100")]
        [TestCase("1010110011", "0000001110101100")]
        [TestCase("10101100111", "0000011110101100")]
        [TestCase("101011001111", "0000111110101100")]
        [TestCase("1010110011111", "0001111110101100")]
        [TestCase("10101100111111", "0011111110101100")]
        [TestCase("101011001111111", "0111111110101100")]
        [TestCase("1010110011111111", "1111111110101100")]
        public void Charge_SourceWithWrongLength_ReverseContentAsExpected(String source, String expected)
        {
            IBitCharger instance = this.CreateInstance();

            instance.Charge(source, true);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region Clone

        [Test]
        public void Clone_CloneInstance_ClonePropertiesAreEqual()
        {
            IBitCharger instance = this.CreateInstance();

            // 0x02 => 00000010
            instance.SetBitAt(0, false);
            instance.SetBitAt(1, true);
            instance.SetBitAt(2, false);
            instance.SetBitAt(3, false);
            instance.SetBitAt(4, false);
            instance.SetBitAt(5, false);
            instance.SetBitAt(6, false);
            instance.SetBitAt(7, false);

            // 0xCE => 11001110
            instance.SetBitAt(8, false);
            instance.SetBitAt(9, true);
            instance.SetBitAt(10, true);
            instance.SetBitAt(11, true);
            instance.SetBitAt(12, false);
            instance.SetBitAt(13, false);
            instance.SetBitAt(14, true);
            instance.SetBitAt(15, true);

            // 0xDA => 11011010
            instance.SetBitAt(16, false);
            instance.SetBitAt(17, true);
            instance.SetBitAt(18, false);
            instance.SetBitAt(19, true);
            instance.SetBitAt(20, true);
            instance.SetBitAt(21, false);
            instance.SetBitAt(22, true);
            instance.SetBitAt(23, true);

            IBitCharger actual = instance.Clone() as IBitCharger;

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Capacity, Is.EqualTo(instance.Capacity));
            Assert.That(actual.Length, Is.EqualTo(instance.Length));
            Assert.That(actual.Bytes, Is.EqualTo(instance.Bytes));
            Assert.That(actual.Content, Is.EqualTo(instance.Content));
        }

        #endregion

        #region ISerializable

        [Test]
        [Category("IntegrationTest")]
        public void Serialization_SerializeAndDeserialize_NeverThrowsAndPropertiesAsEqual()
        {
            try
            {
                IBitCharger instance = this.CreateInstance();

                instance.SetBitAt(0, true);
                instance.SetBitAt(242, true);

                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, instance);

                    stream.Position = 0;

                    IBitCharger actual = formatter.Deserialize(stream) as IBitCharger;

                    Assert.That(actual, Is.Not.Null);
                    Assert.That(actual.Capacity, Is.EqualTo(instance.Capacity));
                    Assert.That(actual.Length, Is.EqualTo(instance.Length));
                    Assert.That(actual.Content, Is.EqualTo(instance.Content));
                }
            }
            catch (Exception error)
            {
                Assert.That(false, error.Message);
            }
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Simple_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            Int32 source = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            instance.SetBytes(BitConverter.GetBytes(source), 0);

            String expected = "10011111 11011010 11001110 00000010";

            String actual = instance.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        #region Little Endian

        [Test]
        public void ToString_GroupingDisabledLE_ResultIsContent()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            Int32 source = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            instance.SetBytes(BitConverter.GetBytes(source), 0);

            String actual = instance.ToString(false);

            Assert.That(actual, Is.EqualTo(instance.Content));
        }

        [Test]
        public void ToString_GroupingEnabledLE_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(true);

            Int32 source = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            instance.SetBytes(BitConverter.GetBytes(source), 0);

            String expected = "10011111 11011010 11001110 00000010";

            String actual = instance.ToString(true);

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Big Endian

        [Test]
        public void ToString_GroupingDisabledBE_ResultIsContent()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(false);

            Int32 source = 47110815; // BE: 0x02CEDA9F => 00000010 11001110 11011010 10011111

            instance.SetBytes(BitConverter.GetBytes(source), 0);

            String actual = instance.ToString(false);

            Assert.That(actual, Is.EqualTo(instance.Content));
        }

        [Test]
        public void ToString_GroupingEnabledBE_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            this.byteOrder.SetupGet(x => x.IsLittleEndian).Returns(false);

            Int32 source = 47110815; // BE: 0x02CEDA9F => 00000010 11001110 11011010 10011111

            instance.SetBytes(BitConverter.GetBytes(source), 0);

            String expected = "00000010 11001110 11011010 10011111";

            String actual = instance.ToString(true);

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Helpers

        private IBitCharger CreateInstance()
        {
            return new BitCharger(this.byteOrder?.Object);
        }

        private String MakeExpected(Byte[] source)
        {
            StringBuilder result = new StringBuilder(source.Length * 8);

            foreach (Byte value in source)
            {
                result.Append(Convert.ToString(value, 2).PadLeft(8, '0'));
            }

            return result.ToString();
        }

        #endregion
    }
}
