using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.javaporttemp;
using Iesi.Collections.Generic;



namespace net.zemberek.bilgi.kokler
{
    /*
    * Created on 24.Eki.2004
    * 
    * K�k d���m� s�n�f� K�k a�ac�n�n yap�ta��d�r. Her d���m, k�kler, e�seli k�kler,
    * de�i�mi� halleri ifade eden bir string ve uygun �ekilde bellek kullan�m� i�in 
    * haz�rlanm�� �zel bir alt d���m listesi nesnesi ta��r.
    * <p/>
    * �e�itli nedenlerle de�i�ikli�e u�rayabilecek olan k�kler a�aca eklenirken
    * de�i�mi� halleri ile beraber eklenirler. �rne�in kitap k�k� hem kitab hem de
    * kitap hali ile s�zl��e eklenir, ancak bu iki kelime i�in olu�an d���m de
    * ayn� k�k� g�sterirler. B�ylece Kitab�na gibi kelimeler i�in k�k adaylar�
    * aran�rken kitap k�k�ne eri�ilmi� olur.
    * <p/>
    * E� sesli olan k�kler ayn� d���me ba�lan�rlar. A�ac�n olu�umu s�ras�nda ilk 
    * gelen k�k d���mdeki k�k de�i�kenne, sonradan gelenler de esSesliler listesine 
    * eklenirler. Arama s�ras�nda bu k�k te aday olarak d�nd�r�l�r.
    *
    * @author MDA
    */
    public class KokDugumu
    {
        private AltDugumListesi altDugumler = null;
        // Her d���m bir harfle ifade edilir.
        private char harf;
        // e� seslileri ta��yan liste (Kok nesneleri ta��r)
        private List<Kok> esSesliler = null;
        // D���m�n ta��d��� k�k
        private Kok kok = null;
        // K�k�n de�i�mi� halini tutan string
        private IEnumerable<char> kelime = null;

        public KokDugumu()
        {
        }

        public KokDugumu(char harf)
        {
            this.harf = harf;
        }

        public KokDugumu(char harf, IEnumerable<char> icerik, Kok kok)
        {
            this.harf = harf;
            this.kok = kok;
            if (!icerik.Equals(kok.icerik())) this.kelime = icerik;
        }

        /**
         * Verilen karakteri ta��yan alt d���m� getirir.
         *
         * @param in
         * @return E�er verilen karakteri ta��yan bir alt d���m varsa
         * o d���m�, yoksa null.
         */
        public KokDugumu altDugumGetir(char cin)
        {
            if (altDugumler == null)
                return null;
            else
                return altDugumler.altDugumGetir(cin);
        }

        /**
         * Verilen d���m� bu d���me alt d���m olarak ekler.
         * D�n�� de�eri eklenen d���md�r
         *
         * @param dugum
         * @return Eklenen d���m
         */
        public KokDugumu addNode(KokDugumu dugum)
        {
            if (altDugumler == null)
            {
                altDugumler = new AltDugumListesi();
            }
            altDugumler.ekle(dugum);
            return dugum;
        }

        /**
         * @return tum alt dugumler. dizi olarak.
         */
        public KokDugumu[] altDugumDizisiGetir()
        {
            if (altDugumler == null)
            {
                return new KokDugumu[0];
            }
            return altDugumler.altDugumlerDizisiGetir();
        }

        public bool altDugumVarMi()
        {
            return !(altDugumler == null || altDugumler.size() == 0);
        }
        /**
         * E�er D���me ba�l� bir k�k zaten varsa esSesli olarak ekle, 
         * yoksa sadece kok'e yaz.
         *
         * @param kok
         */
        public void kokEkle(Kok kok)
        {
            if (this.kok != null)
            {
                if (esSesliler == null) esSesliler = new List<Kok>(1);
                esSesliler.Add(kok);
            }
            else
            {
                this.kok = kok;
            }
        }

        public Kok getKok()
        {
            return this.kok;
        }

        public List<Kok> getEsSesliler()
        {
            return esSesliler;
        }

        public IEnumerable<char> getKelime()
        {
            if (kelime != null) return kelime;
            if (kok != null) return kok.icerik();
            return null;
        }

        public void setKelime(IEnumerable<char> kelime)
        {
            this.kelime = kelime;
        }

        /**
         * @return d���me ba�l� k�k ve e� seslilerin hepsini bir listeye 
         * koyarak geri d�nd�r�r.
         */
        public List<Kok> tumKokleriGetir()
        {
            if (kok != null)
            {
                List<Kok> kokler = new List<Kok>();
                kokler.Add(kok);
                if (esSesliler != null)
                {
                    kokler.AddRange(esSesliler);
                }
                return kokler;
            }
            return null;
        }

        /**
         * Verilen collectiona d���me ba�l� t�m k�kleri ekler. 
         *
         * @param kokler
         */
        public void tumKokleriEkle(List<Kok> kokler)
        {
            if (kok != null && !kokler.Contains(kok))
            {
                kokler.Add(kok);
                if (esSesliler != null)
                {
                    kokler.AddRange(esSesliler);
                }
            }
        }

        public void temizle()
        {
            this.kok = null;
            this.kelime = null;
            this.esSesliler = null;
        }

        public void kopyala(KokDugumu kaynak)
        {
            this.kok = kaynak.getKok();
            this.kelime = kaynak.getKelime();
            this.esSesliler = kaynak.getEsSesliler();
        }

        public char getHarf()
        {
            return harf;
        }

        public void setHarf(char harf)
        {
            this.harf = harf;
        }

        /**
         * D���m�n ve alt d���mlerinin a�a� yap�s� �eklinde string g�sterimini d�nd�r�r.
         * sadece debug ama�l�d�r.
         *
         * @param level
         * @return dugumun string halini dondurur.
         */
        public String getStringRep(int level)
        {
            char[] indentChars = new char[level * 2];
            for (int i = 0; i < indentChars.Length; i++)
                indentChars[i] = ' ';
            String indent = new String(indentChars);
            String str = indent + " Harf: " + harf;
            if (kelime != null)
            {
                str += " [Kelime: " + kelime + "] ";
            }
            if (kok != null)
            {
                str += " [Kok: " + kok + "] ";
            }

            if (esSesliler != null)
            {
                str += " [Es sesli: ";
                foreach (Kok anEsSesliler in esSesliler)
                {
                    str += (anEsSesliler) + " ";
                }
                str += " ]";
            }

            KokDugumu[] subNodes = altDugumDizisiGetir();
            if (subNodes != null)
            {
                str += "\n " + indent + " Alt dugumler:\n";
                foreach (KokDugumu subNode in subNodes)
                {
                    if (subNode != null)
                    {
                        str += subNode.getStringRep(level + 1) + "\n";
                    }
                }
            }
            return str;
        }

        public String toString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("harf:").Append(harf);
            if (altDugumler != null)
                buf.Append(" alt dugum sayisi:").Append(altDugumler.size());
            return buf.ToString();
        }

        /**
         * K�k agacindaki d���mlerin alt d���mleri i�in bu sinifi kullanirlar.
         * �zellikle bellek kullaniminin �nemli oldugu Zemberek-Pardus ve OOo 
         * eklentisi gibi uygulamalarda bu yapinin kullanilmasi bellek kazanci 
         * getirmektedir. 
         * Asagidaki sinifta alt dugum sayisi CEP_BUYUKLUGU degerinden
         * az ise sadece CEP_BUYUKLUGU elemanli bir dizi acar. Bu dizi �zerinde 
         * Arama yapmak biraz daha yavas olsa da ortalama CEP_BUYUKLUGU/2 aramada 
         * sonuca eri�ildi�i i�in verilen ceza minimumda kalir. 
         *
         */
        private static readonly int CEP_BUYUKLUGU = 3;
        private sealed class AltDugumListesi
        {
            KokDugumu[] dugumler = new KokDugumu[CEP_BUYUKLUGU];
            int index = 0;
            IDictionary<Char, KokDugumu> tumDugumler = null;

            /**
             * Verilen d���m� alt d���m olarak ekler. eger alt d���mlerinin sayisi
             * CEP_BUYUKLUGU degerini asmissa bir HashMap olu�turur
             * @param dugum
             */
            public void ekle(KokDugumu dugum)
            {
                if (index == CEP_BUYUKLUGU)
                {
                    if (tumDugumler == null)
                    {
                        tumDugumler = new Dictionary<Char, KokDugumu>(CEP_BUYUKLUGU + 2);
                        for (int i = 0; i < CEP_BUYUKLUGU; i++)
                        {
                            tumDugumler.Add(dugumler[i].getHarf(), dugumler[i]);
                        }
                        dugumler = null;
                    }
                    tumDugumler.Add(dugum.getHarf(), dugum);
                }
                else
                {
                    dugumler[index++] = dugum;
                }
            }

            /**
             * Verilen karaktere sahip alt d���m� d�nd�r�r.
             * @param giris
             * @return ilgili KokDugumu
             */
            public KokDugumu altDugumGetir(char giris)
            {
                if (dugumler != null)
                {
                    for (int i = 0; i < index; i++)
                    {
                        if (dugumler[i].getHarf() == giris)
                        {
                            return dugumler[i];
                        }
                    }
                    return null;
                }
                else
                {
                    return tumDugumler[giris];
                }
            }

            /**
             * Alt d���mleri dizi olarak d�nd�r�r.
             * @return KokDugumu[] cinsinden alt d���mler dizisi
             */
            public KokDugumu[] altDugumlerDizisiGetir()
            {
                if (dugumler != null)
                {
                    return dugumler;
                }
                else
                {
                    KokDugumu[] ret = new KokDugumu[tumDugumler.Values.Count];
                    tumDugumler.Values.CopyTo(ret, 0);
                    return ret;
                }
            }

            public int size()
            {
                if (dugumler != null)
                {
                    return index;
                }
                else
                {
                    return tumDugumler.Count;
                }
            }

        }
    }
}
