using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.yapi.kok
{
    /**
     * Basitce harf dizisinin son harfini siler.
     */
    public class SonHarfDusmesi : HarfDizisiIslemi
    {
        #region HarfDizisiIslemi Members

        public void uygula(HarfDizisi dizi)
        {
            if (dizi.length() > 0)
                dizi.harfSil(dizi.length() - 1);
        }

        #endregion
    }
}
