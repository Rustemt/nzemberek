﻿/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.yapi.ek
{
    public class ZamanKiOzelDurumu : EkOzelDurumu
    {
        public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici)
        {
            TurkceHarf sonSesli = kelime.icerik().sonSesli();
            if (sonSesli.CharDeger == 'u' || sonSesli.CharDeger == Alfabe.CHAR_uu)
                return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, _uretimBilesenleri);
            else
                return null;
        }

        public override HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return cozumlemeIcinUret(kelime, null, null);
        }
    }
}
