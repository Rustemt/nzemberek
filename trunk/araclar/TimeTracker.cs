using System;
using System.Collections.Generic;
using System.Text;
using Iesi.Collections.Generic;
using log4net;


namespace net.zemberek.araclar
{
    /**
     * Hassas kronometre ihtiya�lar� i�in tasarlanm��t�r.
     * <p/>
     * Kullanmak i�in timeTracker.startClock(isim) dedikten sonra
     * TimeTracker.stopClock(isim)'un d�nd�rd��� String'i ge�en s�reyi g�stermek 
     * i�in kullanabilirsiniz. Stop'tan �nce ara ad�mlar� izlemek istiyorsan�z 
     * TimeTracker.getElapsedTimeString(isim) veya getElapsedTimeStringAsMillis
     * metodlarini kullanabilirsiniz. Start ile ba�latt���n�z saatleri isiniz 
     * bittigindemutlaka stop ile durdurman�z gerekiyor, ��nk� ancak stop ile register
     * olmu� bir saat nesnesini unregister edebilirsiniz.
     * <p/>
     * Olusan saatler globaldir, yani programin icinde istediginiz her yerde
     * kullanabilirsiniz.
     *
     * @author M.D.A
     */
    public sealed class TimeTracker
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int MAX_TIMETRACKER_USERS = 500;
        public static readonly long BOLUCU = 1000000000L;
        private static IDictionary<String, TimerElement> users = new Dictionary<String, TimerElement>();

        /**
         * Yeni bir saat olu�turur ve listeye register eder.
         * @param name : saat ad�
         */
        public static void startClock(String name)
        {
            if (users.Count > MAX_TIMETRACKER_USERS)
            {
                logger.Error("Max Saat izleyici say�s� a��ld�. (" + MAX_TIMETRACKER_USERS + ")");
                return;
            }
            if (users[name] != null)
            {
                logger.Error(name + " isminde bir zaman izleyici zaten var.");
                return;
            }
            TimerElement timer = new TimerElement(name);
            users.Add(name, timer);
        }

        /**
         * ismi verilen saat i�in ba�lang��tan bu yana bu yana ne kadar zaman 
         * ge�ti�ini milisaniye cinsinden d�nd�r�r.
         *
         * @param name : saatin ad�
         * @return :Bir �nceki tick'ten bu yana ge�en s�re (milisaniye cinsinden)
         */
        public static long getElapsedTime(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            return timer.getElapsedTime();
        }

        /**
         * ismi verilen saatin en son kontrol�nden bu yana ne kadar zaman ge�ti�ini
         * milisaniye cinsinden d�nd�r�r.
         *
         * @param name :  saatin ad�
         * @return :Bir �nceki tick'ten bu yana ge�en s�re (milisaniye cinsinden)
         */
        public static long getTimeDelta(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            return timer.getDiff();
        }

        /**
         * ismi verilen saatin en son kontrolunden (baslangic veya bir onceki tick) 
         * bu yana ne kadar zaman gecti�ini ve ba�lang��tan bu yana ge�en s�reyi 
         * virg�lden sonra 3 basamakl� saniyeyi ifade eden String cinsinden d�nd�r�r.
         *
         * @param name : saatin ad�
         * @return : Bir �nceki tick'ten bu yana ge�en s�re (Binde bir hassasiyetli saniye cinsinden cinsinden)
         */
        public static String getElapsedTimeString(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return "Ge�ersiz Kronometre: " + name;
            timer.refresh();
            return "Delta: " + (double)timer.getDiff() / BOLUCU + " s. Elapsed: " + (double)timer.getElapsedTime() / BOLUCU + " s.";
        }

        /**
         * @param name : saatin ad�
         * @return : Bir �nceki tick'ten bu yana ge�en s�re (milisaniye cinsinden)
         */
        public static String getElapsedTimeStringAsMillis(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return "Ge�ersiz Kronometre: " + name;
            timer.refresh();
            return "Delta: " + timer.getDiff() + "ms. Elapsed: " + timer.getElapsedTime() + "ms.";
        }

        /**
         * @param name      : saatin ad�
         * @param itemCount : sure zarf�nda islenen nesne sayisi
         * @return : baslangictan bu yana islenen saniyedeki eleman sayisi
         */
        public static long getItemsPerSecond(String name, long itemCount)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            long items = 0;
            if (timer.getElapsedTime() > 0)
                items = (BOLUCU * itemCount) / timer.getElapsedTime();
            return items;
        }

        /**
         * Saati durdurur ve ba�lang��tan bu yana ge�en s�reyi saniye ve ms 
         * cinsinden d�nd�r�r. Ayr�ca saati listeden siler. 
         *
         * @param name Saat ismi
         * @return ba�lang��tan bu yana ge�en s�re
         */
        public static String stopClock(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return name + " : Ge�ersiz Kronometre";
            timer.refresh();
            users.Remove(name);
            return "" + (float)timer.getElapsedTime() / BOLUCU + "sn."
                   + "(" + timer.getElapsedTime() + " ms.)";
        }

        /**
        * isimlendirilmi� Zaman bilgisi ta��y�c�.
        *
        * @author MDA
        */
        class TimerElement
        {
            String name;
            long startTime = 0;
            long stopTime = 0;
            long lastTime = 0;
            long creationTime = 0;
            long elapsedTime = 0;
            long diff = 0;

            public TimerElement(String name)
            {
                //TODO : Eger DateTime.Now.Ticks yeterince hassas de�ilse 
                //bunu implement edebiliriz : http://www.codeproject.com/csharp/highperformancetimercshar.asp
                creationTime = DateTime.Now.Ticks;
                startTime = creationTime;
                lastTime = creationTime;
                this.name = name;
            }

            public void refresh()
            {
                diff = DateTime.Now.Ticks - lastTime;
                lastTime = DateTime.Now.Ticks;
                elapsedTime = lastTime - startTime;
            }

            public long getDiff()
            {
                return diff;
            }

            public long getElapsedTime()
            {
                return elapsedTime;
            }

            public long getLastTime()
            {
                return lastTime;
            }

            public String getName()
            {
                return name;
            }

            public long getStartTime()
            {
                return startTime;
            }

            public long getStopTime()
            {
                return stopTime;
            }
        }
    }
}
    




