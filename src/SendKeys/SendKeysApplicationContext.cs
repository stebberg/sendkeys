using SendKeys.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendKeys
{
    public class SendKeysApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public SendKeysApplicationContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.AppIcon,
                ContextMenu = new ContextMenu(GetMenuItems()),
                Visible = true,
                Text = "SendKeys"
            };
        }

        private MenuItem[] GetMenuItems()
        { 
            SettingsFile settings = new SettingsFile();

            var menuItems = new List<MenuItem>();

            if (settings?.Settings?.Senders != null)
            {
                foreach (var send in settings.Settings.Senders)
                {
                    var itm = new MenuItem(send.Name, SendKeys, Shortcut.None);
                    itm.Tag = send;
                    menuItems.Add(itm);
                }
            }

            menuItems.Add(new MenuItem("-"));
            menuItems.Add(new MenuItem("Reload settings", ReloadSettings));
            menuItems.Add(new MenuItem("Open settings", OpenSettings));
            menuItems.Add(new MenuItem("About", About));
            menuItems.Add(new MenuItem("Exit", Exit));

            return menuItems.ToArray();
        }

        void SendKeys(object sender, EventArgs e)
        {
            MenuItem itm = sender as MenuItem;
            SettingsFile.SendKeyObj toSend = itm.Tag as SettingsFile.SendKeyObj;
            System.Threading.Thread.Sleep(toSend.Delay);
            System.Windows.Forms.SendKeys.Send(toSend.Keys);
        }

        void About(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        void ReloadSettings(object sender, EventArgs e)
        {
            trayIcon.ContextMenu = new ContextMenu(GetMenuItems());
        }

        void OpenSettings(object sender, EventArgs e)
        {
            SettingsFile settings = new SettingsFile();
            try
            {
                System.Diagnostics.Process.Start(settings.Settings.TextEditor, settings.GetFileName());
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

    }
}
