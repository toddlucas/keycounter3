/*
   Copyright (C) 2007 OBACHT & UNCLE JAMAL

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace KeyCounter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // generate mutex to allow only one instance of keycounter
            Mutex mutex = new Mutex(false, Application.ProductName + "ad90g7a9d7");
            if (mutex.WaitOne(0, false))
            {
                FormDebug formDebug;
                FormMain formMain;
                WinHook.KeyboardHook keyboardHook;
                WinHook.MouseHook mouseHook;
                List<WinHook.UniversalHook> hookList;

                using (keyboardHook = new WinHook.KeyboardHook())
                {
                    using (mouseHook = new WinHook.MouseHook())
                    {
                        hookList = new List<KeyCounter.WinHook.UniversalHook>();
                        hookList.Add(keyboardHook);
                        hookList.Add(mouseHook);

                        formDebug = new FormDebug();
                        formMain = new FormMain(formDebug, hookList);
                        // don't show the form, to not confuse the alt-tab mechanism
                        Application.Run();
                    }
                }
            }
            else
                MessageBox.Show("You can start only one instance of " + Application.ProductName + "!", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public interface ITextDebugger
    {
        void Debug(string msg);
        void Debug(string msg, DebugLevel msgLevel);
        void SetLevel(DebugLevel level);
        DebugLevel GetLevel();
    }

    public enum DebugLevel
    {
        None = 0,
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4,
        Debug = 5
    }

    public interface IFileAccessChecker
    {
        FileAccessError CheckPathName(string path, string name);
    }

    public enum FileAccessError
    {
        None,
        DirectoryNotValid,
        FilenameNotValid,
        Unauthorized,
        IllegalCharacter
    }
}