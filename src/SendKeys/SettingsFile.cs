using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendKeys
{
    public class SettingsFile
    {

        public class SettingsObj
        {
            public string TextEditor { get; set; }
            public SendKeyObj[] Senders { get; set; }
        }

        public class SendKeyObj
        { 
            public string Name { get; set; }
            public string Keys { get; set; }
            public int Delay { get; set; }
        }

        public string GetFileName()
        {
            return Path.Combine(Application.StartupPath, "settings.json");
        }

        public SettingsObj Settings
        {
            get
            {
                if (!File.Exists(GetFileName()))
                    Settings = new SettingsObj() 
                    { 
                        TextEditor = "Notepad.exe", 
                        Senders = new SendKeyObj[] 
                        { 
                            new SendKeyObj() { Delay = 1500, Keys = "Hello{ENTER}", Name = "Send hello" } 
                        } 
                    };

                try
                {
                    string json = File.ReadAllText(GetFileName());
                    return JsonConvert.DeserializeObject<SettingsObj>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(null, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return new SettingsObj();
            }
            set 
            {
                string json = JsonConvert.SerializeObject(value);
                try
                {
                    File.WriteAllText(GetFileName(), json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(null, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        
        }

    }
}
