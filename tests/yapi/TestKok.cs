﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;


namespace net.zemberek.tests.yapi
{
    [TestFixture]
    public class TestKok
    {

        KokOzelDurumBilgisi koz;

        public void setUp()
        {
            koz = dilBilgisi.kokOzelDurumlari();
        }

        public void testDegismisIcerikOlustur()
        {


            Kok kok = new Kok("ara", KelimeTipi.FIIL);
            ozelDurumTest(kok, "ar");
            assertTrue(kok.ozelDurumIceriyormu(SIMDIKI_ZAMAN));
            kok = new Kok("kitap", KelimeTipi.ISIM);
            kok.ozelDurumEkle(koz.ozelDurum(SESSIZ_YUMUSAMASI));
            ozelDurumTest(kok, "kitab");

            String str = "al" + Alfabe.CHAR_ii + "n";
            kok = new Kok(str, KelimeTipi.ISIM);
            kok.ozelDurumEkle(koz.ozelDurum(ISIM_SESLI_DUSMESI));
            ozelDurumTest(kok, "aln");

            kok = new Kok("nakit", KelimeTipi.ISIM);
            kok.ozelDurumEkle(koz.ozelDurum(ISIM_SESLI_DUSMESI));
            kok.ozelDurumEkle(koz.ozelDurum(SESSIZ_YUMUSAMASI));
            ozelDurumTest(kok, "nakd");

            kok = new Kok("ben", KelimeTipi.ZAMIR);
            kok.ozelDurumEkle(koz.ozelDurum(TEKIL_KISI_BOZULMASI));
            ozelDurumTest(kok, "ban");

            kok = new Kok("sen", KelimeTipi.ZAMIR);
            kok.ozelDurumEkle(koz.ozelDurum(TEKIL_KISI_BOZULMASI));
            ozelDurumTest(kok, "san");

            kok = new Kok("de", KelimeTipi.FIIL);
            kok.ozelDurumEkle(koz.ozelDurum(FIIL_KOK_BOZULMASI));
            ozelDurumTest(kok, "di");

            kok = new Kok("ye", KelimeTipi.FIIL);
            kok.ozelDurumEkle(koz.ozelDurum(FIIL_KOK_BOZULMASI));
            ozelDurumTest(kok, "yi");
        }

        private void ozelDurumTest(Kok kok, String beklenen)
        {
            String[] results = koz.ozelDurumUygula(kok);
            assertTrue(results.length > 0);
            String sonuc = results[0];
            assertEquals(sonuc, beklenen);
        }

        public void testEqual()
        {
            Kok kok1 = new Kok("kitap", KelimeTipi.ISIM);
            Kok kok2 = new Kok("kitap", KelimeTipi.ISIM);
            Kok kok3 = new Kok("kitab", KelimeTipi.ISIM);
            assertTrue(kok1.equals(kok2));
            assertTrue(kok1.equals(kok3) == false);
        }

        public void testilkEkBelirle()
        {
            /*        Kok kok = new Kok("kedi", KelimeTipi.ISIM);
                    assertEquals(TurkceEkYonetici.ref().ilkEkBelirle(), Ekler.ISIM_YALIN);
                    kok.setOzelDurumlar(new HashSet());
                    kok.ozelDurumlar().add(TurkceKokOzelDurumlari.YALIN);
                    assertEquals(kok.ilkEkBelirle(), Ekler.GENEL_YALIN);
                    kok = new Kok("almak", KelimeTipi.FIIL);
                    assertEquals(kok.ilkEkBelirle(), Ekler.FIIL_YALIN);
                    kok = new Kok("on", KelimeTipi.SAYI);
                    assertEquals(kok.ilkEkBelirle(), Ekler.SAYI_YALIN);*/
        }
    }
}
