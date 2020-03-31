<p align="center">
  <a href="https://github.com/akesseler/Plexdata.BitCharger/blob/master/LICENSE.md" alt="license">
    <img src="https://img.shields.io/github/license/akesseler/Plexdata.BitCharger.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.BitCharger/releases/latest" alt="latest">
    <img src="https://img.shields.io/github/release/akesseler/Plexdata.BitCharger.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.BitCharger/archive/master.zip" alt="master">
    <img src="https://img.shields.io/github/languages/code-size/akesseler/Plexdata.BitCharger.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.BitCharger/wiki" alt="wiki">
    <img src="https://img.shields.io/badge/wiki-API-orange.svg" />
  </a>
</p>

## Plexdata Bit Charger

The _Plexdata Bit Charger_ is a library that allows users to easily charge (almost) unlimited 
bits. Main feature of this library is that bits can be charged one by one or at once from byte 
arrays. Several extension methods allow to charge bits from framework types, such as shorts, 
ints, longs, etc. Taking the system's endianness into account at runtime is guaranteed as well.

## Examples

The code snippet below demonstrates how to charge the bits for a particular integer value. For 
sure, this is a pretty simple example, but it shows how the bit charger might be used.

Assuming to charge the bits of integer value `47110815`. This integer value would have a byte 
order of `0x9FDACE02` on little-endian systems. After charging all bits, the binary result would 
be `10011111 11011010 11001110 00000010`.

```
static void Main(String[] args)
{
    IBitCharger charger = BitChargerFactory.Create();

    charger.SetBitAt(1, true);
    charger.SetBitAt(9, true);
    charger.SetBitAt(10, true);
    charger.SetBitAt(11, true);
    charger.SetBitAt(14, true);
    charger.SetBitAt(15, true);
    charger.SetBitAt(17, true);
    charger.SetBitAt(19, true);
    charger.SetBitAt(20, true);
    charger.SetBitAt(22, true);
    charger.SetBitAt(23, true);
    charger.SetBitAt(24, true);
    charger.SetBitAt(25, true);
    charger.SetBitAt(26, true);
    charger.SetBitAt(27, true);
    charger.SetBitAt(28, true);
    charger.SetBitAt(31, true);

    Console.WriteLine(charger);
    Console.ReadKey();
}
```