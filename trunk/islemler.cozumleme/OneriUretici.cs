/*
 * Created on 27.May.2005
 * MDA
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using net.zemberek.bilgi;
using net.zemberek.islemler;
using net.zemberek.yapi;
using Iesi.Collections;
using Iesi.Collections.Generic;
using net.zemberek.javaporttemp;


namespace net.zemberek.islemler.cozumleme
{
    public class OneriUretici
    {
        private KelimeCozumleyici cozumleyici, asciiToleransliCozumleyici;
        private ToleransliCozumleyici toleransliCozumleyici;
        private CozumlemeYardimcisi yardimci;
        private ZemberekAyarlari ayarlar;


        public OneriUretici(CozumlemeYardimcisi yardimci,
                            KelimeCozumleyici cozumleyici,
                            KelimeCozumleyici asciiToleransliCozumleyici,
                            ToleransliCozumleyici toleransliCozumleyici,
                            ZemberekAyarlari ayarlar)
        {
            this.yardimci = yardimci;
            this.toleransliCozumleyici = toleransliCozumleyici;
            this.cozumleyici = cozumleyici;
            this.asciiToleransliCozumleyici = asciiToleransliCozumleyici;
            this.ayarlar = ayarlar;
        }

        /**
         * Verilen kelime i�in �neri �retir.
         * Yap�lan �neriler �u �ekildedir:
         * - K�kte 1, ekte 1 mesafeye kadar olmak �zere Levenshtein d�zeltme mesafesine uyan t�m �neriler
         * - Deasciifier'den d�n�� de�eri olarak gelen �neriler
         * - Kelimenin ayr�k iki kelimeden olu�mas� durumu i�in �neriler
         *
         * @param kelime : �neri yap�lmas� istenen giri� kelimesi
         * @return String[] olarak �neriler
         *         E�er �neri yoksa sifir uzunluklu dizi.
         */
        public String[] oner(String kelime) {
        // Once hatal� kelime i�in tek kelimelik �nerileri bulmaya �al��
        Kelime[] oneriler = toleransliCozumleyici.cozumle(kelime);
        
        //Deasciifierden bir �ey var m�?
        Kelime[] asciiTurkceOneriler = new Kelime[0];
        if (ayarlar.oneriDeasciifierKullan())
            asciiTurkceOneriler = asciiToleransliCozumleyici.cozumle(kelime);

        Set<String> ayriYazimOnerileri = Collections.EMPTY_SET_STRING;

        // Kelime yanlislikla bitisik yazilmis iki kelimeden mi olusmus?
        if (ayarlar.oneriBilesikKelimeKullan()) {
            for (int i = 1; i < kelime.Length; i++) {
                String s1 = kelime.Substring(0, i);
                String s2 = kelime.Substring(i, kelime.Length-i);
                if (cozumleyici.denetle(s1) && cozumleyici.denetle(s2)) {

                    Set<String> set1 = new HashedSet<String>();
                    Kelime[] kelimeler1 = cozumleyici.cozumle(s1);
                    foreach (Kelime kelime1 in kelimeler1) {
                        yardimci.kelimeBicimlendir(kelime1);
                        set1.Add(kelime1.icerik().ToString());
                    }

                    Set<String> set2 = new HashedSet<String>();
                    Kelime[] kelimeler2 = cozumleyici.cozumle(s2);
                    foreach (Kelime kelime1 in kelimeler2) {
                        yardimci.kelimeBicimlendir(kelime1);
                        set2.Add(kelime1.icerik().ToString());
                    }

                    if (ayriYazimOnerileri.Count == 0) {
                        ayriYazimOnerileri = new HashedSet<String>();
                    }

                    foreach (String str1 in set1) {
                        foreach (String str2 in set2) {
                            ayriYazimOnerileri.Add(str1 + " " + str2);
                        }
                    }
                }
            }
        }

        // erken donus..
        if (oneriler.Length == 0 && ayriYazimOnerileri.Count == 0 && asciiTurkceOneriler.Length == 0) {
            return new String[0];
        }

        // Onerileri puanland�rmak i�in bir listeye koy
        List<Kelime> oneriList = new List<Kelime>();
        oneriList.AddRange(new List<Kelime>(oneriler));
        oneriList.AddRange(new List<Kelime>(asciiTurkceOneriler));

        // Frekansa g�re s�rala
        oneriList.Sort(new KelimeKokFrekansKiyaslayici());

        // D�n�� listesi string olacak, Yeni bir liste olu�tur. 
        List<String> sonucListesi = new List<String>();
        foreach (Kelime anOneriList in oneriList) {
            sonucListesi.Add(anOneriList.icerik().ToString());
        }

        //�ift sonu�lar� liste siras�n� bozmadan iptal et.
        List<String> rafineListe = new List<String>();
        foreach (String aday in sonucListesi) {
            bool aynisiVar = false;
            foreach (String aRafineListe in rafineListe) {
                if (aday.Equals(aRafineListe)) {
                    aynisiVar = true;
                    break;
                }
            }
            if (!aynisiVar && rafineListe.Count < ayarlar.getOneriMax()) {
                rafineListe.Add(aday);
            }
        }
        	
        // Son olarak yer kalm��sa ayr� yaz�l�m �nerilerini ekle
        foreach (String oneri in ayriYazimOnerileri) {
            if (rafineListe.Count < ayarlar.getOneriMax())
                rafineListe.Add(oneri);
            else
                break;
        }

        return rafineListe.ToArray();
    }
    }
}
