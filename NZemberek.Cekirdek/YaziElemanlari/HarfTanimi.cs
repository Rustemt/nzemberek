using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.YaziElemanlari
{
    public class HarfTanimi
    {
        /// <summary>
        /// B�y�k harf kar��l��� karakter.
        /// </summary>
        public char BuyukKarakter;

        /// <summary>
        /// K���k harf kar��l��� karakter.
        /// </summary>
        public char KucukKarakter;

        /// <summary>
        /// Ascii d��� bir harf i�in Ascii benzeri olan harfi temsil eden karakter.
        /// Harf ascii d��� de�ilse Alfabedeki tan�ms�z karaktere e�ittir.
        /// </summary>
        public char AsciiBenzeri;

        /// <summary>
        /// Ascii bir harf i�in Ascii d��� benzeri olan harfi temsil eden karakter.
        /// Harf ascii de�ilse Alfabedeki tan�ms�z karaktere e�ittir.
        /// </summary>
        public char AsciiDisiBenzeri;
    }
}
