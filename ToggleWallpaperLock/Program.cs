using System;
using Microsoft.Win32;

namespace ToggleWallpaperLock
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get the key, make sure we get the permission to change it as well
                using (var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop", true))
                {
                    // Make sure the path exists
                    if (key != null)
                    {
                        // Try to pull the value
                        var registryKey = key.GetValue("NoChangingWallPaper", 0x00000000);
                        int parsedKey;

                        // If there is none, it'll return null
                        if (registryKey != null)
                        {
                            // Needs to be an int. If it isn't set it to one
                            if (int.TryParse(registryKey.ToString(), out parsedKey))
                            {
                                key.SetValue("NoChangingWallPaper", (int)registryKey == 0 ? 0x00000001 : 0x00000000, RegistryValueKind.DWord);
                            }
                            else
                            {
                                key.SetValue("NoChangingWallPaper", 0x00000001, RegistryValueKind.DWord);
                            }
                        }
                        else
                        {
                            // It's null, so they want it on
                            key.SetValue("NoChangingWallPaper", 0x00000001, RegistryValueKind.DWord);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot add the key, 'HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\ActiveDesktop' does not exist!");
                        Console.WriteLine("Press any key to close...");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                Console.WriteLine("Press any key to close...");
                Console.ReadLine();
            }
        }
    }
}
