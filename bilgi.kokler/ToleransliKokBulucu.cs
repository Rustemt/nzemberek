﻿using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.araclar;

namespace net.zemberek.bilgi.kokler
{
    /**
 * Hata toleranslı kök bulucu hatalı girişler için öneri üretmekte kullanılır.
 * <p/>
 * Ağacın "e" harfi ile başlayan kolu aşağıdaki gibi olsun:
 * <p/>
 * <pre>
 * e
 * |---l(el)
 * |  |---a(ela)
 * |  |  |--s-(elastik)
 * |  |
 * |  |---b
 * |  |  |--i-(elbise)
 * |  |
 * |  |---m
 * |  |  |--a(elma)
 * |  |  |  |--c-(elmacık)
 * |  |  |  |--s-(elmas)
 * |  | ...
 * | ...
 * ...
 * </pre>
 * <p/>
 * "elm" girişi için ağaç üzerinde ilerlerken hata mesafesi 1 olduğu müddetçe 
 * ilerlenir. bu sırada "el, ela, elma" kökleri toplanır.
 *  @author MDA
 */
    public class ToleransliKokBulucu : KokBulucu
    {
        private KokAgaci agac = null;
        private int tolerans = 0;
        private int distanceCalculationCount = 0;

        public int getDistanceCalculationCount()
        {
            return distanceCalculationCount;
        }

        public ToleransliKokBulucu(KokAgaci agac, int tolerans)
        {
            this.agac = agac;
            this.tolerans = tolerans;
        }

        public List<Kok> getAdayKokler(String giris)
        {
            return benzerKokleriBul(giris);
        }

        private String giris = null;
        private List<Kok> adaylar = null;

        private List<Kok> benzerKokleriBul(String giris)
        {
            this.giris = giris;
            adaylar = new List<Kok>();
            yuru(agac.getKokDugumu(), "");
            return adaylar;
        }

        private void yuru(KokDugumu dugum, String olusan) {
        String tester = olusan;
        tester += dugum.getHarf();
        if (dugum.getKok() != null) {
            distanceCalculationCount++;
            if (MetinAraclari.isInSubstringEditDistance((String) dugum.getKelime(), giris, tolerans)) {
            	// Aday kök bulundu
                adaylar.Add(dugum.getKok());
            } else {
                // Mesafe sınırı aşıldı.
                return;
            }
        } else {
            if (!MetinAraclari.isInSubstringEditDistance(tester.Trim(), giris, tolerans)) {
            	// Ara stringde mesafe sınırı aşıldı
                return;
            }
        }

        foreach (KokDugumu altDugum in dugum.altDugumDizisiGetir()) {
            if (altDugum != null) {
                this.yuru(altDugum, tester);
            }
        }
    }
    }
}
