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
using Plexdata.Converters.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Converters.Tests.Extensions
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    [TestFixtureSource(typeof(BitCharger))]
    public class BitChargerExtensionTests
    {
        #region Prologue

        private Mock<IByteOrder> byteOrder;

        [SetUp]
        public void SetUp()
        {
            this.byteOrder = new Mock<IByteOrder>();
            this.byteOrder.Setup(x => x.IsLittleEndian).Returns(true);
        }

        #endregion

        #region SByte

        #region SetBytes

        [Test]
        public void SetBytes_SByteOverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            SByte value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_SByteOverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            SByte value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_SByteOverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            SByte value = 42; // LE: 0x2A => 00101010

            String expected = "00101010";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_SByteOverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            SByte value = 42; // LE: 0x2A => 00101010
            Int32 offset = 2;

            String expected = "001010100000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToSByte

        [Test]
        public void ToSByte_SByteOverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToSByte(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToSByte_SByteOverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToSByte(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToSByte_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            SByte expected = 42; // LE: 0x2A => 00101010

            Byte[] array = new Byte[] { 0x2A };

            instance.SetBytes(array);

            Assert.That(instance.ToSByte(), Is.EqualTo(expected));
        }

        [Test]
        public void ToSByte_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            SByte expected = 42; // LE: 0x2A => 00101010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x2A };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToSByte(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Byte

        #region SetBytes

        [Test]
        public void SetBytes_ByteOverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Byte value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_ByteOverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Byte value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_ByteOverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Byte value = 42; // LE: 0x2A => 00101010

            String expected = "00101010";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_ByteOverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Byte value = 42; // LE: 0x2A => 00101010
            Int32 offset = 2;

            String expected = "001010100000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToByte

        [Test]
        public void ToByte_ByteOverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToByte(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToByte_ByteOverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToByte(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToByte_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Byte expected = 42; // LE: 0x2A => 00101010

            Byte[] array = new Byte[] { 0x2A };

            instance.SetBytes(array);

            Assert.That(instance.ToByte(), Is.EqualTo(expected));
        }

        [Test]
        public void ToByte_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Byte expected = 42; // LE: 0x2A => 00101010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x2A };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToByte(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Int16

        #region SetBytes

        [Test]
        public void SetBytes_Int16OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int16 value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_Int16OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int16 value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_Int16OverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int16 value = 4711; // LE: 0x6712 => 01100111 00010010

            String expected = "0110011100010010";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_Int16OverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int16 value = 4711; // LE: 0x6712 => 01100111 00010010
            Int32 offset = 2;

            String expected = "01100111000100100000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToInt16

        [Test]
        public void ToInt16_Int16OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToInt16(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToInt16_Int16OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToInt16(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToInt16_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int16 expected = 4711; // LE: 0x6712 => 01100111 00010010

            Byte[] array = new Byte[] { 0x67, 0x12 };

            instance.SetBytes(array);

            Assert.That(instance.ToInt16(), Is.EqualTo(expected));
        }

        [Test]
        public void ToInt16_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int16 expected = 4711; // LE: 0x6712 => 01100111 00010010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x67, 0x12 };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToInt16(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region UInt16

        #region SetBytes

        [Test]
        public void SetBytes_UInt16OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            UInt16 value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_UInt16OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            UInt16 value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_UInt16OverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt16 value = 4711; // LE: 0x6712 => 01100111 00010010

            String expected = "0110011100010010";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_UInt16OverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt16 value = 4711; // LE: 0x6712 => 01100111 00010010
            Int32 offset = 2;

            String expected = "01100111000100100000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToUInt16

        [Test]
        public void ToUInt16_UInt16OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToUInt16(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToUInt16_UInt16OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToUInt16(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToUInt16_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt16 expected = 4711; // LE: 0x6712 => 01100111 00010010

            Byte[] array = new Byte[] { 0x67, 0x12 };

            instance.SetBytes(array);

            Assert.That(instance.ToUInt16(), Is.EqualTo(expected));
        }

        [Test]
        public void ToUInt16_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt16 expected = 4711; // LE: 0x6712 => 01100111 00010010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x67, 0x12 };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToUInt16(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Int32

        #region SetBytes

        [Test]
        public void SetBytes_Int32OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_Int32OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_Int32OverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int32 value = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            String expected = "10011111110110101100111000000010";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_Int32OverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int32 value = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010
            Int32 offset = 2;

            String expected = "100111111101101011001110000000100000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToInt32

        [Test]
        public void ToInt32_Int32OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToInt32(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToInt32_Int32OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToInt32(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToInt32_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int32 expected = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02 };

            instance.SetBytes(array);

            Assert.That(instance.ToInt32(), Is.EqualTo(expected));
        }

        [Test]
        public void ToInt32_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int32 expected = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02 };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToInt32(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region UInt32

        #region SetBytes

        [Test]
        public void SetBytes_UInt32OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            UInt32 value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_UInt32OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            UInt32 value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_UInt32OverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt32 value = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            String expected = "10011111110110101100111000000010";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_UInt32OverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt32 value = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010
            Int32 offset = 2;

            String expected = "100111111101101011001110000000100000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToUInt32

        [Test]
        public void ToUInt32_UInt32OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToUInt32(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToUInt32_UInt32OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToUInt32(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToUInt32_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt32 expected = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02 };

            instance.SetBytes(array);

            Assert.That(instance.ToUInt32(), Is.EqualTo(expected));
        }

        [Test]
        public void ToUInt32_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt32 expected = 47110815; // LE: 0x9FDACE02 => 10011111 11011010 11001110 00000010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02 };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToUInt32(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Int64

        #region SetBytes

        [Test]
        public void SetBytes_Int64OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int64 value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_Int64OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int64 value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_Int64OverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int64 value = 47110815; // LE: 0x9FDACE0200000000 => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00000000

            String expected = "1001111111011010110011100000001000000000000000000000000000000000";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_Int64OverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int64 value = 47110815; // LE: 0x9FDACE0200000000 => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00000000
            Int32 offset = 2;

            String expected = "10011111110110101100111000000010000000000000000000000000000000000000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToInt64

        [Test]
        public void ToInt64_Int64OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToInt64(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToInt64_Int64OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToInt64(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToInt64_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int64 expected = 3026418949640084127; // LE: 0x9FDACE020000002A => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00101010

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02, 0x00, 0x00, 0x00, 0x2A };

            instance.SetBytes(array);

            Assert.That(instance.ToInt64(), Is.EqualTo(expected));
        }

        [Test]
        public void ToInt64_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            Int64 expected = 3026418949640084127; // LE: 0x9FDACE020000002A => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00101010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02, 0x00, 0x00, 0x00, 0x2A };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToInt64(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region UInt64

        #region SetBytes

        [Test]
        public void SetBytes_UInt64OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            UInt64 value = 42;

            Assert.That(() => instance.SetBytes(value), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_UInt64OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            UInt64 value = 42;
            Int32 offset = 42;

            Assert.That(() => instance.SetBytes(value, offset), Throws.ArgumentNullException);
        }

        [Test]
        public void SetBytes_UInt64OverloadOnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt64 value = 47110815; // LE: 0x9FDACE0200000000 => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00000000

            String expected = "1001111111011010110011100000001000000000000000000000000000000000";

            instance.SetBytes(value);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        [Test]
        public void SetBytes_UInt64OverloadValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt64 value = 47110815; // LE: 0x9FDACE0200000000 => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00000000
            Int32 offset = 2;

            String expected = "10011111110110101100111000000010000000000000000000000000000000000000000000000000";

            instance.SetBytes(value, offset);

            Assert.That(instance.Content, Is.EqualTo(expected));
        }

        #endregion

        #region ToUInt64

        [Test]
        public void ToUInt64_UInt64OverloadOnlyValueInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Assert.That(() => instance.ToUInt64(), Throws.ArgumentNullException);
        }

        [Test]
        public void ToUInt64_UInt64OverloadValueAndLengthInstanceIsNull_ThrowsArgumentNullException()
        {
            IBitCharger instance = null;

            Int32 offset = 42;

            Assert.That(() => instance.ToUInt64(offset), Throws.ArgumentNullException);
        }

        [Test]
        public void ToUInt64_OnlyValue_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt64 expected = 3026418949640084127; // LE: 0x9FDACE020000002A => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00101010

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02, 0x00, 0x00, 0x00, 0x2A };

            instance.SetBytes(array);

            Assert.That(instance.ToUInt64(), Is.EqualTo(expected));
        }

        [Test]
        public void ToUInt64_ValueAndOffset_ResultAsExpected()
        {
            IBitCharger instance = this.CreateInstance();

            UInt64 expected = 3026418949640084127; // LE: 0x9FDACE020000002A => 10011111 11011010 11001110 00000010 00000000 00000000 00000000 00101010
            Int32 offset = 2;

            Byte[] array = new Byte[] { 0x9F, 0xDA, 0xCE, 0x02, 0x00, 0x00, 0x00, 0x2A };

            instance.SetBytes(array, offset);

            Assert.That(instance.ToUInt64(offset), Is.EqualTo(expected));
        }

        #endregion

        #endregion

        #region Helpers

        private IBitCharger CreateInstance()
        {
            return this.CreateInstance(0);
        }

        private IBitCharger CreateInstance(Int32 length)
        {
            IBitCharger instance = new BitCharger(this.byteOrder?.Object);

            if (length > 0)
            {
                instance.SetBitAt(length - 1, false);
            }

            return instance;
        }

        #endregion
    }
}
