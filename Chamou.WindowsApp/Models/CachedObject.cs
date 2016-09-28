using Chamou.AppCommon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;

namespace Chamou.WindowsApp.Models
{
    public class CachedObject
    {
        #region Private Fields and Properties
        private static readonly string LocalSettingsKey = "CachedObject";
        private static CachedObject _instance;
        private static ApplicationDataContainer LocalSettings =>
            ApplicationData.Current.LocalSettings;
        #endregion


        private CachedObject() { }

        public void Clean()
        {
            LocalSettings.Values.Remove(LocalSettingsKey);
            _instance = null;
        }

        public void UpdateStorage() => 
            LocalSettings.Values[LocalSettingsKey] = JsonConvert.SerializeObject(_instance);

        #region Properties
        public static CachedObject Instance => GetInstance();

        public BasicGeoposition? Geoposition { get; set; }

        public Place Place { get; set; } 
        #endregion


        private static CachedObject GetInstance()
        {
            if (_instance == null)
            {
                object o;
                if (LocalSettings.Values.TryGetValue(LocalSettingsKey, out o))
                {
                    _instance = JsonConvert.DeserializeObject<CachedObject>(o as string);
                }
                else
                {
                    _instance = new CachedObject();
                    LocalSettings.Values.Add(LocalSettingsKey, JsonConvert.SerializeObject(_instance));
                }
            }
            return _instance;
        }
        
    }
}
